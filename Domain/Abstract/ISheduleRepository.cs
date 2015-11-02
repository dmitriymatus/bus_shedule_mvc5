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

        IEnumerable<Direction> Directions { get; }

        IEnumerable<Days> Days { get ; }
        void AddDaysRange(IEnumerable<Days> entities);

        IEnumerable<Shedule> Shedule { get ; }
        void AddSheduleRange(IEnumerable<Shedule> entities);
        void AddShedule(Shedule entity);
        bool UpdateShedule(Shedule entity, string newShedule);
        bool DeleteShedule(Shedule entity);
        bool DeleteAllShedule(City city);

    }
}
