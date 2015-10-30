using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Domain.Abstract;
using System.Text.RegularExpressions;

namespace Domain.Concrete
{
    public class EFSheduleRepository : ISheduleRepository
    {
        SheduleDbContext context = new SheduleDbContext();

        public IEnumerable<City> Cities
        {
            get { return context.Cities; }
        }


        public IEnumerable<BusStop> BusStops
        {
            get { return context.BusStops; }
        }

        public void AddStopsRange(IEnumerable<BusStop> entities)
        {
            context.BusStops.AddRange(entities);
            context.SaveChanges();
        }

        public IEnumerable<Bus> Buses
        {
            get { return context.Buses; }
        }

        public void AddBus(Bus entity)
        {
            context.Buses.Add(entity);
            context.SaveChanges();
        }

        public void AddBusesRange(IEnumerable<Bus> entities)
        {
            var aaa = context.Buses.AddRange(entities);
            context.SaveChanges();
        }

        public IEnumerable<Days> Days
        {
            get { return context.Days; }
        }
        public void AddDaysRange(IEnumerable<Days> entities)
        {
            context.Days.AddRange(entities);
            context.SaveChanges();
        }

        public IEnumerable<Shedule> Shedule
        {
            get { return context.Shedule; }
        }

        public void AddSheduleRange(IEnumerable<Shedule> entities)
        {
            var buses = entities.Select(x => x.Bus).Distinct(new Compare());
            context.Shedule.AddRange(entities);
            context.SaveChanges();
        }

        public void AddShedule(Shedule entity)
        {
            context.Shedule.Add(entity);
            context.SaveChanges();
        }
        //public void AddStops(IEnumerable<BusStop> stops)
        //{

        //    context.Stops.AddRange(stops);
        //    context.SaveChanges();
        //}


        class Compare : IEqualityComparer<Bus>
        {
            public bool Equals(Bus bus1, Bus bus2)
            {
                return (bus1.Number == bus2.Number && bus1.City == bus2.City);
            }
            public int GetHashCode(Bus bus)
            {
                return bus.Number.GetHashCode();
            }
        }

        //-------------------------------------------------------------------------------
        //#region getMethods

        //public IEnumerable<string> GetBuses(int? city)
        //{
        //    return context.Stops.Where(x=>x.CityId == city).AsEnumerable()
        //                        .OrderBy(x => x.Id)
        //                        .Select(x => x.BusNumber)
        //                        .Distinct();
        //}

        //public IEnumerable<string> GetAllStops(int? city)
        //{
        //    return context.Stops.Where(x=>x.CityId == city).AsEnumerable()
        //               .OrderBy(x => x.StopName)
        //               .Select(x => x.StopName)
        //               .Distinct();
        //}

        //public IEnumerable<string> GetStops(string busNumber, int? city)
        //{
        //    return context.Stops.AsEnumerable()
        //                        .Where(x => x.BusNumber == busNumber && x.CityId == city)
        //                        .Select(x => x.StopName)
        //                        .Distinct();
        //}

        //public IEnumerable<string> GetOtherBuses(string stopName, string busNumber, int? city)
        //{
        //    return context.Stops.Where(x => x.StopName == stopName && x.BusNumber != busNumber && x.CityId == city)
        //                        .Select(x => x.BusNumber)
        //                        .Distinct();
        //}


        //public IEnumerable<string> GetFinalStops(string stopName, string busNumber, int? city)
        //{
        //    return context.Stops.Where(x => x.StopName == stopName && x.BusNumber == busNumber && x.CityId == city)
        //                        .Select(x => x.FinalStop)
        //                        .Distinct();
        //}

        //public IEnumerable<string> GetDays(string stopName, string busNumber, string endStop, int? city)
        //{
        //    return context.Stops.Where(x => x.StopName == stopName && x.BusNumber == busNumber && x.FinalStop == endStop && x.CityId == city)
        //                        .Select(x => x.Days)
        //                        .Distinct();
        //}

        //public IEnumerable<string> GetItems(string stopName, string busNumber, string endStop, string days, int? city)
        //{
        //    String stops = context.Stops.FirstOrDefault(x => x.BusNumber == busNumber && x.StopName == stopName && x.FinalStop == endStop && x.Days == days && x.CityId == city).Stops;
        //    Regex reg = new Regex(@"\d{1,2}:\d{1,2}");
        //    MatchCollection matches = reg.Matches(stops);

        //    return matches.Cast<Match>().Select(x => x.Value);
        //}
        //#endregion
        ////----------------------------------------------------------------------------------------------------------

        ////--------------------------------------------------------------------------------------------------
        //#region adminMethods

        //public void AddStop(string busNumber, string stopName, string finalStop, string days, int? city)
        //{
        //    BusStop stop = new BusStop
        //    {
        //        BusNumber = busNumber,
        //        StopName = stopName,
        //        FinalStop = finalStop,
        //        Days = days,
        //        CityId = city
        //    };
        //    context.Stops.Add(stop);
        //    context.SaveChanges();

        //}

        //public void AddStops(IEnumerable<BusStop> stops)
        //{

        //    context.Stops.AddRange(stops);
        //    context.SaveChanges();
        //}

        //public void DeleteAll(int? city)
        //{
        //    IEnumerable<BusStop> stops = context.Stops.Where(x=>x.CityId == city);
        //    context.Stops.RemoveRange(stops);
        //    context.SaveChanges();
        //}

        //public bool Contain(string busNumber, string stopName, string finalStop, string days, int? city)
        //{
        //    BusStop stop = new BusStop
        //    {
        //        BusNumber = busNumber,
        //        StopName = stopName,
        //        FinalStop = finalStop,
        //        Days = days,
        //        CityId = city
        //    };
        //    return Filter(stop).Any();
        //}

        //public bool Update(string busNumber, string stopName, string finalStop, string days, string stops, int? city)
        //{
        //    BusStop stop = new BusStop
        //    {
        //        BusNumber = busNumber,
        //        StopName = stopName,
        //        FinalStop = finalStop,
        //        Days = days,
        //        Stops = stops,
        //        CityId = city
        //    };
        //    BusStop item = Filter(stop).FirstOrDefault();
        //    if (item == null)
        //        return false;
        //    item.Stops = stop.Stops;
        //    context.SaveChanges();
        //    return true;
        //}

        //public bool Delete(string busNumber, string stopName, string finalStop, string days, int? city)
        //{
        //    BusStop stop = new BusStop
        //    {
        //        BusNumber = busNumber,
        //        StopName = stopName,
        //        FinalStop = finalStop,
        //        Days = days,
        //        CityId = city
        //    };
        //    BusStop item = Filter(stop).FirstOrDefault();
        //    if (item != null)
        //    {
        //        context.Stops.Remove(item);
        //        context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
        //#endregion
        ////-----------------------------------------------------------------------------------

        //private IEnumerable<BusStop> Filter(BusStop stop)
        //{
        //    return context.Stops.Where(x => x.BusNumber == stop.BusNumber && x.StopName == stop.StopName && x.FinalStop == stop.FinalStop && x.Days == stop.Days && x.CityId == stop.CityId);
        //}
    }
}
