using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    public class SystemSetting
    {
        [Column(Order=0), Key]
        public string Module { get; set; }
        [Column(Order=1), Key]
        public string Name { get; set; }
        public string Value { get; set; }

        public SystemSetting()
        {
            Module = string.Empty;
            Name = string.Empty;
            Value = string.Empty;
        }

        public SystemSetting(string module, string name, string val)
        {
            Module = module;
            Name = name;
            Value = val;
        }
    }
}