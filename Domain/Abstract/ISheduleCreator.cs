using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Abstract;

namespace Domain.Abstract
{
    public interface ISheduleCreator
    {
        void Create(string fileName, IStopsRepository repository, int? city);
    }
}
