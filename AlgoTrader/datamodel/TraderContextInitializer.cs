﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AlgoTrader.Interfaces;

namespace AlgoTrader.datamodel
{
    class TraderContextInitializer : DropCreateDatabaseAlways<TraderContext>
    {
        protected override void Seed(TraderContext context)
        {
            Portfolio portfolio = new Portfolio();
            portfolio.Cash = 10000;
            context.Portfolios.Add(portfolio);
            context.SaveChanges();

            Symbol s = new Symbol("GOOG");
            context.Symbols.Add(s);
            context.SaveChanges();

            Position pos1 = new Position();
            pos1.price = 100;
            pos1.quantity = 5;
            pos1.status = positionStatus.Open;
            pos1.Symbol = s;
            pos1.Portfolio = portfolio;
            context.Positions.Add(pos1);
            context.SaveChanges();

            Trade t1 = new Trade();
            t1.Symbol = s;
            t1.Position = pos1;
            t1.price = 20;
            t1.quantity = 5;
            t1.type = tradeTypes.Buy;
            t1.TransactionId = Guid.NewGuid().ToString();
            t1.timestamp = DateTime.Now;
            context.Trades.Add(t1);
            context.SaveChanges();

            Quote q1 = new Quote();
            q1.Symbol = s;
            q1.timestamp = DateTime.Now;
            q1.price = 20.15;
            context.Quotes.Add(q1);
            context.SaveChanges();

            Alert a1 = new Alert();
            a1.AlertId = Guid.NewGuid();
            a1.Symbol = s;
            a1.Timestamp = DateTime.Now;
            a1.Type = tradeTypes.Buy;
            a1.Quantity = 100;
            a1.SentTo = "bra5047@psu.edu";
            a1.Price = 10.45;
            a1.ResponseCode = responseCodes.Pending;
            context.Alerts.Add(a1);
            context.SaveChanges();
        }
    }
}