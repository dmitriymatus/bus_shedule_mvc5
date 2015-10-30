using System.Collections.Generic;
using Domain.Models;

namespace Domain.Abstract
{
    public interface ISheduleRepository
    {
        IEnumerable<BusStop> BusStops { get ; }
        void AddStopsRange(IEnumerable<BusStop> entities);

        IEnumerable<Bus> Buses { get ; }
        void AddBus(Bus entity);
        void AddBusesRange(IEnumerable<Bus> entities);

        IEnumerable<City> Cities { get; }

        IEnumerable<Days> Days { get ; }
        void AddDaysRange(IEnumerable<Days> entities);

        IEnumerable<Shedule> Shedule { get ; }
        void AddSheduleRange(IEnumerable<Shedule> entities);
        void AddShedule(Shedule entity);
        //IEnumerable<BusStop> Stops { get; }
        //IEnumerable<string> GetBuses(int? city);
        //IEnumerable<string> GetStops(string busNumber, int? city);
        //IEnumerable<string> GetOtherBuses(string stopName, string busNumber, int? city);
        //IEnumerable<string> GetFinalStops(string stopName, string busNumber, int? city);
        //IEnumerable<string> GetDays(string stopName, string busNumber, string endStop, int? city);
        //IEnumerable<string> GetItems(string stopName, string busNumber, string endStop, string days, int? city);
        //IEnumerable<string> GetAllStops(int? city);
        //void AddStops(IEnumerable<BusStop> stops);
        //void AddStop(string busNumber, string stopName, string finalStop, string days, int? city);
        //bool Contain(string busNumber, string stopName, string finalStop, string days, int? city);
        //void DeleteAll(int? city);
        //bool Update(string busNumber, string stopName, string finalStop, string days, string stops, int? city);
        //bool Delete(string busNumber, string stopName, string finalStop, string days, int? city);
    }
}
