using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Adventure
{
    internal sealed class Application
    {
        public void OnConfigure()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
            // config.
        }
        // todo: add dependency injection/loading stuff/creating stuff
    }
}
