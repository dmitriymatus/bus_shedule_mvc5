using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Excel;
using Domain.Models;
using Domain.Abstract;

namespace Domain.Concrete
{
    public class SheduleCreator: ISheduleCreator
    {
        const int dataStart = 4;
        public void Create(string fileName, IStopsRepository repository)
        {

            List<StringBuilder> rows = new List<StringBuilder>();
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            DataSet result = excelReader.AsDataSet();
            DataTable table = result.Tables[0];

            for (int i = dataStart; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][0].ToString().StartsWith("№") == true)
                {
                    StringBuilder item = new StringBuilder();
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (table.Rows[i][j].ToString() != null && table.Rows[i][0].ToString().StartsWith("№"))
                        {
                            item.Append(table.Rows[i][j].ToString() + "|");
                        }
                    }
                    rows.Add(item);
                }
            }
            IEnumerable<busStop> stops = Parse(rows);
            stream.Close();
            excelReader.Dispose();

            repository.DeleteAll();
            repository.AddStops(stops);
        }


        private IEnumerable<busStop> Parse(List<StringBuilder> rows)
        {
            string busNumber;
            string stopName;
            string finalStop;
            string days;
            string stops;
            string[] cols;
            char[] separator = new char[1];
            separator[0] = '|';

            foreach (StringBuilder row in rows)
            {
                cols = row.ToString().Split(separator, StringSplitOptions.None);
                if (!String.IsNullOrEmpty(cols[0] as string) && !String.IsNullOrEmpty(cols[1] as string) && !String.IsNullOrEmpty(cols[2] as string) && !String.IsNullOrEmpty(cols[3] as string))
                {
                    busNumber = cols[0].Remove(0, 1);
                    stopName = cols[3];
                    finalStop = cols[2];
                    days = cols[1] == "Р" ? "Рабочие" : cols[1] == "В" ? "Выходные" : cols[1] == "Р,В" ? "Ежедневно" : cols[1];
                    stops = Convert(cols.Skip(7).Take(cols.Count() - 11));
                    yield return new busStop { busNumber = busNumber, stopName = stopName, finalStop = finalStop, days = days, stops = stops };
                }
            }
        }


        private string Convert(IEnumerable<string> values)
        {
            return String.Join(" ", values.Where(x => !String.IsNullOrEmpty(x)).Select(x => Helper(x)));
        }

        private string Helper(string value)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result.ToString("HH:mm");
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
                    return string.Format("{0:00}:{1:00}", hours, minutes);
                }
                else
                {
                    return "empty";
                }
            }
        }

    }
}
