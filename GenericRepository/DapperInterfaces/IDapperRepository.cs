using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.DapperInterfaces
{
    public interface IDapperRepository : IDapperQueries, IDapperCommand
    {
    }
}
