﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using SQ = ATQouteMngrUpdate.StockWebService;

namespace ATQouteMngrUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            SQ.StockQuote Qoute = new SQ.StockQuote();
            String Price, Time, Symbol;
            XmlDocument doc = new XmlDocument();
            XmlNode SelectName, SelectTotal, SelectTime, SelectDate;
            List<string> symbols = new List<string>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnString")))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT DISTINCT SymbolName FROM WatchListItems",conn);
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    symbols.Add(dr.GetString(0));
                }

            }

            if (symbols.Count > 0)
            {
                
                using (SqlConnection connUpdate = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnString")))
                {
                    connUpdate.Open();
                    SqlCommand SymUpdate = new SqlCommand("INSERT INTO Qoutes (Price, TimeStamp, Symbol) VALUES (@Price, @Time, @Symbol)", connUpdate);
                    foreach (string sym in symbols)
                    {
                        String QouteUpdate = Qoute.GetQuote(sym);
                        if (QouteUpdate != string.Empty)
                        {
                            doc.LoadXml(QouteUpdate);
                            SelectTotal = doc.DocumentElement.SelectSingleNode("//Stock/Last");
                            SelectName = doc.DocumentElement.SelectSingleNode("//Stock/Name");
                            SelectTime = doc.DocumentElement.SelectSingleNode("//Stock/Time");
                            SelectDate = doc.DocumentElement.SelectSingleNode("//Stock/Date");
                            Price = SelectTotal.InnerText;
                            Symbol = SelectName.InnerText;
                            Time = SelectDate.InnerText + " " + SelectTime.InnerText;

                            SymUpdate.Parameters.Add("@Price", Price);
                            SymUpdate.Parameters.Add("@Time", Time);
                            SymUpdate.Parameters.Add("@Symbol", Symbol);
                            SymUpdate.ExecuteNonQuery();
                        }
                    }

                }
            }
        }
    }
}