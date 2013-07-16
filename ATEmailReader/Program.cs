using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System.Xml;
using System.Text.RegularExpressions;
using AlgoTrader.portfolio;


namespace ATEmailReader
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Imap map = new Imap())
            {
                map.ConnectSSL("imap.gmail.com");
                map.Login("AlgTrader500@gmail.com", "algSweng500");

                //select only the inbox to search and only show unread messages
                map.SelectInbox();
                List<long> uids = map.Search(Flag.Unseen);

                foreach (long uid in uids)
                {
                    string eml = map.GetMessageByUID(uid);
                    IMail mail = new MailBuilder().CreateFromEml(eml);


                    string title = mail.Subject;
                    string message = mail.Text;

                    //get the stock symbol
                    string[] Symbolsplit = title.Split(new char[0]);
                    string symbol = Symbolsplit[1].ToString();

                    //get the amount to sell or buy
                    string AmountExtract = Regex.Match(message, @"\d+").Value;
                    int quantity = Int32.Parse(AmountExtract);
                    
                    //convert the message and title to lowercase so the if statement will have no errors
                    message = message.ToLower();
                    title = title.ToLower();

                    PortfolioManager Manage = new PortfolioManager();
                    if (message.Contains("yes") && title.Contains("sell"))
                    {
                        Manage.sell(symbol, quantity);
                    }
                    else if (message.Contains("yes") && title.Contains("buy"))
                    {
                        Manage.buy(symbol, quantity);
                    }
                    else
                    {
                        //adding just incase we find a need for it
                    }
                }
            }
        }
    }
}
