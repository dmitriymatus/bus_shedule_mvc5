using System.Collections.Generic;
using Domain.Models;

namespace Domain.Abstract
{
    public interface IStopsRepository
    {
        IEnumerable<BusStop> Stops { get; }
        IEnumerable<string> GetBuses(int city);
        IEnumerable<string> GetStops(string busNumber);
        IEnumerable<string> GetOtherBuses(string stopName, string busNumber);
        IEnumerable<string> GetFinalStops(string stopName, string busNumber);
        IEnumerable<string> GetDays(string stopName, string busNumber, string endStop);
        IEnumerable<string> GetItems(string stopName, string busNumber, string endStop, string days);
        IEnumerable<string> GetAllStops();
        void AddStops(IEnumerable<BusStop> stops);
        void AddStop(string busNumber, string stopName, string finalStop, string days);
        bool Contain(string busNumber, string stopName, string finalStop, string days);
        void DeleteAll();
        bool Update(string busNumber, string stopName, string finalStop, string days, string stops);
        bool Delete(string busNumber, string stopName, string finalStop, string days);
    }
}
