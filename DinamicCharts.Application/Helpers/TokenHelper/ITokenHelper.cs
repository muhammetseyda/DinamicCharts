using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinamicCharts.Application.Helpers.TokenHelper
{
    public interface ITokenHelper
    {
        string GenerateToken(string username, string password);
    }
}
