﻿using System;
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
    public class Portfolio : IPortfolio
    {
        [Key]
        public int PortfolioId { get; set; }
        public double Cash { get; set; }
        public virtual List<Position> Positions { get; set; }

        List<IPosition> IPortfolio.Positions
        {
            get
            {
                return Positions.ToList<IPosition>();
            }
        }

        double IPortfolio.Value
        {
            get
            {
                return Positions.Sum(p => p.price);
            }
        }
    }
}