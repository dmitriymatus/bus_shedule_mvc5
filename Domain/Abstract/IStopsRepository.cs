using System.Collections.Generic;
using Domain.Models;

namespace Domain.Abstract
{
    public interface IStopsRepository
    {
        IEnumerable<busStop> Stops { get; }
        IEnumerable<string> GetBuses();
        IEnumerable<string> GetStops(string busNumber);
        IEnumerable<string> GetOtherBuses(string stopName, string busNumber);
        IEnumerable<string> GetFinalStops(string stopName, string busNumber);
        IEnumerable<string> GetDays(string stopName, string busNumber, string endStop);
        IEnumerable<string> GetItems(string stopName, string busNumber, string endStop, string days);
        IEnumerable<string> GetAllStops();
        void AddStops(IEnumerable<busStop> stops);
        void AddStop(string busNumber, string stopName, string finalStop, string days);
        bool Contain(string busNumber, string stopName, string finalStop, string days);
        void DeleteAll();
        bool Update(string busNumber, string stopName, string finalStop, string days, string stops);
        bool Delete(string busNumber, string stopName, string finalStop, string days);
    }
}
