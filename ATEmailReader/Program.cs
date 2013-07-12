using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System.Xml;
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

                map.SelectInbox();
                List<long> uids = map.Search(Flag.Unseen);

                foreach (long uid in uids)
                {
                    string eml = map.GetMessageByUID(uid);
                    IMail mail = new MailBuilder().CreateFromEml(eml);


                    string title = mail.Subject;
                    string message = mail.Text;

                    string[] Symbolsplit = title.Split(new char[0]);
                    string symbol = Symbolsplit[1].ToString();

                    message = message.ToLower();
                    title = title.ToLower();

                    PortfolioManager Manage = new PortfolioManager();
                    if (message.Contains("yes") && title.Contains("sell"))
                    {
                        Manage.sell(symbol, 5);
                    }
                    else if (message.Contains("yes") && title.Contains("buy"))
                    {
                        Manage.buy(symbol, 5);
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
