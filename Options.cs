using QuickFix.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
namespace InitiatorLD
{
    internal class Options
    {
        [Option(shortName: 'o', longName: "order", Required = false, HelpText = "Pass Your Order Plz :")]
        public string Order { get; set; }
        [Option(shortName: 'i', longName: "ID", Required = false, HelpText = "Pass Your ID Plz :")]
        public ClOrdID ID { get; set; }
        [Option(shortName: 's', longName: "symbol", Required = false, HelpText = "Pass Your symbol Plz :")]
        public Symbol Symbol { get; set; }
        [Option(shortName: 'b', longName: "buy", Required = false, HelpText = "Pass Your Buy Plz :")]
        public string bSide { get; set; }
        /*[Option(shortName: 'S', longName: "side", Required = false, HelpText = "Pass Your Side Plz :")]
        public string side { get; set; }*/
        [Option(shortName: 't', longName: "type", Required = false, HelpText = "Pass Your Market Plz :")]
        public string Ordtype { get; set; }
        [Option(shortName: 'p', longName: "prize", Required = false, HelpText = "Pass Your prize Plz :")]
        public string Prize { get; set; }
        [Option(shortName: 'a', longName: "account", Required = false, HelpText = "Pass Your account Plz :")]
        public Account Acc { get; set; }
        [Option(shortName: 'q', longName: "quantity", Required = false, HelpText = "Pass Your quantity Plz :")]
        public string qty { get; set; }
    }
}
