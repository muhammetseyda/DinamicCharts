using DinamicCharts.Application.DTOs.ConnectionInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinamicCharts.Application.Interfaces
{
    public interface IDbInspectorServices
    {
        Task<List<string>> GetStoredProceduresAsync(ConnectionInfoDto connection);
        Task<List<string>> GetViewsAsync(ConnectionInfoDto connection);
        Task<List<string>> GetFunctionsAsync(ConnectionInfoDto connection);
        Task<List<Dictionary<string, object>>> ExecuteObjectAsync(ConnectionInfoDto connection, string objectType, string objectName);
    }
}
