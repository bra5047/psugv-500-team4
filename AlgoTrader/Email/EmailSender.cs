using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using AlgoTrader.datamodel;
using AlgoTrader.Interfaces;


namespace AlgoTrader.Email
{
    class EmailSender:IEmail
    {
        public void sendEmail(string Recipient, string SymbolName, string CurrentPrice, tradeTypes TradeType, int Quantity)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("AlgTrader500@Gmail.com", "algSweng500");
                var mail = new MailMessage();
                mail.To.Add(Recipient);
                mail.From = new MailAddress("AlgTrader500@gmail.com");
                if (TradeType == tradeTypes.Sell)
                {
                    mail.Subject = SymbolName + " request to sell " + Quantity.ToString() + " shares";
                    mail.Body = "AlgoTrader software is sending this email because the stock " + SymbolName + " has dropped below the required parameters. <br/>" +
                        "We recommend selling " + Quantity.ToString() + " shares.<br/><br/>" +
                        "Do you wish to sell? <br/>" +
                        "Please respond with either YES or NO";
                }
                else
                {
                    mail.Subject = SymbolName + " request to buy " + Quantity.ToString() + " shares";
                    mail.Body = "AlgoTrader software is sending this email because the stock " + SymbolName + " has risen above the required parameters. <br/>" +
                        "We recommend buying " + Quantity.ToString() + " shares<br/><br/>" +
                        "Do you wish to sell? <br/>" +
                        "Please respond with either YES or NO";
                }

                mail.IsBodyHtml = true;
                client.EnableSsl = true;
                client.Send(mail);
  
            }
        }

    }
}
