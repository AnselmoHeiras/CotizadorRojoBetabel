using LibreR.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizadorRojoBetabel.Models
{
    internal class Config
    {
        private static Config _current;

        public static Config Current { get => _current ?? (_current = new Config()); set => _current = value; }

        public int EarningsPercent { get; set; }

        public Config()
        {
            EarningsPercent = 100;
        }

        internal void Save()
        {
            var json = this.Serialize();
            File.WriteAllText("config.json", json);
        }
    }
}
