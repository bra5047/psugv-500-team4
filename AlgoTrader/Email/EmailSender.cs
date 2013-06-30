﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

using AlgoTrader.Interfaces;

namespace AlgoTrader.Email
{
    class EmailSender:IEmail
    {
        public void sendEmail(string Recipient, string SymbolName)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("AlgTrader500@Gmail.com", "algSweng500");
                var mail = new MailMessage();
                mail.To.Add(Recipient);
                mail.Subject = SymbolName + " request to sell";
                mail.From = new MailAddress("AlgTrader500@gmail.com");
                mail.Body = "AlgoTrader software is sending this email because the stock " + SymbolName + " has dropped below the required parameters. <br/>" +
                    "Do you wish to sell? <br/>" +
                    "Please respond with either YES or NO";
                mail.IsBodyHtml = true;
                client.EnableSsl = true;
                client.Send(mail);
            }
        }

    }
}
