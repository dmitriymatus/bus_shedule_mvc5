using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Excel;
using Domain.Models;
using Domain.SheduleParsers.Abstract;

namespace Domain.SheduleParsers.Concrete
{
    public class BrestSheduleParser : ISheduleParser
    {
        const int dataStart = 4;
        const int busNumberOffset = 0;
        const int stopNameOffset = 3;
        const int finalStopOffset = 2;
        const int daysOffset = 1;
        const int sheduleStartOffset = 7;
        const int endOffset = 11;


        public IEnumerable<TimeTable> Parse(string fileName, City city)
        {
            List<List<string>> rows = new List<List<string>>();
            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    DataSet result = excelReader.AsDataSet();
                    DataTable table = result.Tables[0];

                    for (int i = dataStart; i < table.Rows.Count; i++)
                    {
                        if (table.Rows[i][0].ToString().StartsWith("№") == true)
                        {
                            List<string> item = new List<string>();
                            for (int j = 0; j < table.Columns.Count; j++)
                            {
                                if (table.Rows[i][j].ToString() != null && table.Rows[i][0].ToString().StartsWith("№"))
                                {
                                    item.Add(table.Rows[i][j].ToString());
                                }
                            }
                            rows.Add(item);
                        }
                    }
                }
            }
            List<TimeTable> stops = Parse(rows, city).ToList();

            var groupByStopName = stops.GroupBy(x => x.Stop.Name);
            foreach (var stopNameGroup in groupByStopName)
            {
                var busStop = stopNameGroup.First().Stop;
                foreach (var item in stopNameGroup)
                {
                    item.Stop = busStop;
                }
            }

            var groupByBusNumber = stops.GroupBy(x => x.Bus.Number);
            foreach (var busNumberGroup in groupByBusNumber)
            {
                var bus = busNumberGroup.First().Bus;
                foreach (var item in busNumberGroup)
                {
                    item.Bus = bus;
                }

                var groupByFinalStops = busNumberGroup.GroupBy(x => x.FinalStop.Name);
                foreach (var item in groupByFinalStops)
                {
                    var finalStop = item.Last().Stop;
                    var count = item.Count();
                    for (int i = 0; i < count; i++)
                    {
                        item.ElementAt(i).PreviousStop = i > 0 ? item.ElementAt(i - 1).Stop : null;
                        item.ElementAt(i).NextStop = i < (count - 1) ? item.ElementAt(i + 1).Stop : null;
                        item.ElementAt(i).FinalStop = finalStop;
                    }
                }
            }

            return stops;
        }


        private IEnumerable<TimeTable> Parse(List<List<string>> rows, City city)
        {
            string busNumber;
            string stopName;
            string finalStop;
            Days days = new Days();
            IEnumerable<Shedule> stops;

            List<TimeTable> Result = new List<TimeTable>();

            foreach (List<string> row in rows)
            {
                if (!String.IsNullOrEmpty(row[0] as string) && !String.IsNullOrEmpty(row[1] as string) && !String.IsNullOrEmpty(row[2] as string) && !String.IsNullOrEmpty(row[3] as string))
                {
                    busNumber = row[busNumberOffset].Remove(0, 1);
                    stopName = row[stopNameOffset].Trim(' ');
                    finalStop = row[finalStopOffset].Trim(' ');
                    var daysList = row[daysOffset].Split(',');
                    days = new Days();
                    foreach (var item in daysList)
                    {
                        switch (item.Trim(' '))
                        {
                            case "ПН": { days |= Days.Monday; break; }
                            case "ВТ": { days |= Days.Tuesday; break; }
                            case "СР": { days |= Days.Wednesday; break; }
                            case "ЧТ": { days |= Days.Thursday; break; }
                            case "ПТ": { days |= Days.Friday; break; }
                            case "СБ": { days |= Days.Saturday; break; }
                            case "ВС": { days |= Days.Sunday; break; }
                            case "Р": { days |= Days.Working; break; }
                            case "В": { days |= Days.Weekend; break; }
                        }

                    }
                    stops = Convert(row.Skip(sheduleStartOffset).Take(row.Count() - endOffset), days);

                    if (!Result.Where(x => x.Stop.Name == stopName && x.Bus.Number == busNumber && x.FinalStop.Name == finalStop).Any())
                    {
                        Result.Add(new TimeTable
                        {
                            Bus = new Bus { Number = busNumber, CityId = city.Id },
                            Stop = new Stop { Name = stopName, CityId = city.Id },
                            FinalStop = new Stop { Name = finalStop, CityId = city.Id },
                            Shedules = stops.ToList()
                        });
                    }
                    else
                    {
                        var shedules = Result.Where(x => x.Stop.Name == stopName && x.Bus.Number == busNumber && x.FinalStop.Name == finalStop).First().Shedules;
                        foreach (var temp in stops)
                        {
                            shedules.Add(temp);
                        }
                    }
                }
            }
            return Result;
        }

        //======================================================//
        private IEnumerable<Shedule> Convert(IEnumerable<string> values, Days days)
        {
            return values.Where(x => !String.IsNullOrEmpty(x)).Select(x => new Shedule { Days = days, Time = Helper(x) });
        }

        private TimeSpan Helper(string value)
        {
            TimeSpan result;
            if (TimeSpan.TryParse(value, out result))
            {
                if (result.Days > 0) result -= TimeSpan.FromDays(result.Days);
                return result;
            }
            else
            {
                double temp;
                if (double.TryParse(value, out temp))
                {
                    var hours = (int)(temp * 24);
                    var minutes = (int)Math.Round((((temp * 24) - (double)hours) * 60));
                    if (minutes >= 60)
                    {
                        hours += 1;
                        minutes -= 60;
                    }
                    if (hours >= 24)
                    {
                        hours -= 24;
                    }
                    return TimeSpan.Parse(string.Format("{0:00}:{1:00}", hours, minutes));
                }
                return new TimeSpan(0, 0, 0);
            }
        }
        //========================================================//

    }
}
