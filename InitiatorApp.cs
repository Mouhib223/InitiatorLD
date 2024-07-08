using QuickFix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitiatorLD
{
    internal class InitiatorApp : IApplication
    {
        public SessionID SessionID
        {
            set;
            get;
        }

        public void FromApp(Message msg, SessionID sessionID)
        {
            Console.WriteLine(/*"This is from App..."*/);
            Console.WriteLine("This is the Execution Report :");
            Console.WriteLine(msg); ;
        }

        //public SessionID MySessionID { get; set; }
        public void OnCreate(SessionID sessionID)
        {

            Console.WriteLine("-Session Created-");
            this.SessionID = sessionID;
        }
        public void OnLogout(SessionID sessionID) { Console.WriteLine("Offline..."); }
        public void OnLogon(SessionID sessionID) { Console.WriteLine("You are Connected !"); }
        public void FromAdmin(Message msg, SessionID sessionID) { /*Console.WriteLine("This is from Admin");*/ }
        public void ToAdmin(Message msg, SessionID sessionID) { /*Console.WriteLine("This is to Admin");*/ }
        public void ToApp(Message msg, SessionID sessionID)
        {
            try
            {
                bool possDupFlag = false;
                if (msg.Header.IsSetField(QuickFix.Fields.Tags.PossDupFlag))
                {
                    possDupFlag = QuickFix.Fields.Converters.BoolConverter.Convert(
                        msg.Header.GetString(QuickFix.Fields.Tags.PossDupFlag)); /// FIXME
                }
                if (possDupFlag)
                    throw new DoNotSend();
            }
            catch (FieldNotFoundException)
            { }

            Console.WriteLine("This is the Sent Message :");
            Console.WriteLine("OUT: " + msg.ToString());
            /*Console.WriteLine("This is to App...");*/
        }
    }

}
