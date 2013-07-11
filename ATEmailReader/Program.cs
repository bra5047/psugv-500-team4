using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System.Xml;

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

                    if (message.Contains("yes") && title.Contains("sell"))
                    {
                        //sell stocks try and use brians sell methods
                    }
                    else if (message.Contains("yes") && title.Contains("buy"))
                    {
                        //buy stocks
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
