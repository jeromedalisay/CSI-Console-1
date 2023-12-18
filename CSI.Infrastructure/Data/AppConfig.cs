using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.Infrastructure.Data
{
    public class AppConfig
    {
        public string ConnectionString { get; }

        public AppConfig(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DatabaseConnection");
        }
    }
}
