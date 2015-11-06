﻿using System;
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


        public IEnumerable<Shedule> Parse(string fileName, City city)
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
            List<Shedule> stops = Parse(rows,city).ToList();

            var groupByBusNumber = stops.GroupBy(x => x.Bus.Number);
            foreach (var busNumberGroup in groupByBusNumber)
            {
                var bus = busNumberGroup.First().Bus;
                foreach(var item in busNumberGroup)
                {
                   item.Bus = bus;
                   item.Direction.Bus = bus;
                }
            }

            var groupByStopName = stops.GroupBy(x => x.BusStop.Name);
            foreach (var stopNameGroup in groupByStopName)
            {
                var busStop = stopNameGroup.First().BusStop;
                busStop.Buses = new List<Bus>(stops.Where(x => x.BusStop.Name == busStop.Name).Select(x => x.Bus).Distinct());
                foreach (var item in stopNameGroup)
                {
                    item.BusStop = busStop;
                }
            }

            var groupByDirection = stops.GroupBy(x => x.Direction.Name);
            foreach (var directionGroup in groupByDirection)
            {
                var direction = directionGroup.First().Direction;
                foreach (var item in directionGroup)
                {
                    item.Direction = direction;
                }
            }

            var groupByDays = stops.GroupBy(x => x.Days.Name);
            foreach (var daysGroup in groupByDays)
            {
                var days = daysGroup.First().Days;
                days.Buses = new List<Bus>(stops.Where(x => x.Days.Name == days.Name).Select(x => x.Bus).Distinct());
                foreach (var item in daysGroup)
                {
                    item.Days = days;
                }
            }


            stream.Dispose();
            excelReader.Dispose();

            return stops;
        }


        private IEnumerable<Shedule> Parse(List<StringBuilder> rows, City city)
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
                    busNumber = cols[busNumberOffset].Remove(0, 1);
                    stopName = cols[stopNameOffset];
                    finalStop = cols[finalStopOffset];
                    days = cols[daysOffset] == "Р" ? "Рабочие" : cols[daysOffset] == "В" ? "Выходные" : cols[daysOffset] == "Р,В" ? "Ежедневно" : cols[daysOffset];
                    stops = Convert(cols.Skip(sheduleStartOffset).Take(cols.Count() - endOffset));
                    yield return new Shedule
                    {
                        Bus = new Bus { Number = busNumber, CityId = city.Id },
                        BusStop = new BusStop { Name = stopName, City = city },
                        Direction = new Direction { Name = finalStop,  Bus = new Bus { Number = busNumber, City = city } },
                        Days = new Days { Name = days },
                        City = city,
                        Items = stops
                    };
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
