using QuickFix;
using QuickFix.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using InitiatorLD;
using QuickFix.Transport;
using System.Reflection;

namespace InitiatorLD
{
    internal class InitiatorApp 
    {
        public static void Run()
        {
            try
            {
                var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                SessionSettings settings = new SessionSettings(outputDirectory + "\\InitiatorConfig.cfg");
                InitiatorService myApp = new InitiatorService();
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
                ILogFactory logFactory = new FileLogFactory(settings);
                SocketInitiator initiator = new SocketInitiator(
                    myApp,
                    storeFactory,
                    settings,
                    logFactory);
                Console.WriteLine("===========================================");
            Console.WriteLine("           FIX Protocol Connector          ");
            Console.WriteLine("===========================================");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Attempting to establish connection to the simulator...");
            Console.ResetColor();

            initiator.Start();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Connection established successfully.");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any key to proceed...");
            Console.ResetColor();
            Console.ReadKey();
            while (true)
            {
                var act = InitiatorApp.Menu();

                if (act == "1")
                {

                    var order = new QuickFix.FIX44.NewOrderSingle(
                    InitiatorApp.EnterClOrdID(),
                    InitiatorApp.EnterSymbol(),
                    InitiatorApp.EnterSide(),
                    new TransactTime(DateTime.Now),
                    InitiatorApp.EnterOrdType());
                    Options opt = new Options();
                    order.Set(InitiatorApp.EnterPrice());
                    order.Set(InitiatorApp.EnterOrderQty());
                    order.Set(InitiatorApp.EnterAccount());

                    Session.SendToTarget(order, myApp.SessionID);

                    Console.ReadLine();

                }
                else if (act == "2")
                {
                    var mySessionID = new SessionID("FIX4.4", "CLIENT1", "SIMPLE");
                    var order = new QuickFix.FIX44.OrderCancelRequest(
                    InitiatorApp.EnterOrigClOrdID(),
                    InitiatorApp.EnterClOrdID(),
                    InitiatorApp.EnterSymbol(),
                    InitiatorApp.EnterSide(),
                    new TransactTime(DateTime.Now));

                    order.Set(InitiatorApp.EnterOrderQty());
                    Session.SendToTarget(order, myApp.SessionID);
                    Console.ReadLine();
                }
                else if (act == "3")
                {
                    var mySessionID = new SessionID("FIX4.4", "CLIENT1", "SIMPLE");
                    var order = new QuickFix.FIX44.OrderCancelReplaceRequest(
                    InitiatorApp.EnterOrigClOrdID(),
                    InitiatorApp.EnterClOrdID(),
                    InitiatorApp.EnterSymbol(),
                    InitiatorApp.EnterSide(),
                    new TransactTime(DateTime.Now),
                    InitiatorApp.EnterOrdType());
                    order.Set(InitiatorApp.EnterPrice());
                    order.Set(InitiatorApp.EnterOrderQty());
                    Session.SendToTarget(order, myApp.SessionID);
                    Console.ReadLine();
                }
                else if (act == "q" || act == "Q")
                    break;
            }
            Console.WriteLine("Program Shutdown !!!");
            Session _session;
            initiator.Stop();

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            Environment.Exit(1);
            
        }
        public static string Menu()
        {

            while (true)
            {
                Console.Title = "Broker";
                Console.Clear();
                Title();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("*********************");
                Console.WriteLine("Make your order");
                Say("1", "Enter Order");
                Say("2", "Cancel Order");
                Say("3", "Replace Order");
                Say("4", "Market data test");
                Say("Q", "Quit");
                Console.WriteLine("Action: ");
                string act = Console.ReadLine().Trim();
                Console.ResetColor();
                return act;




            }

        }


        public static void Say(string p, string msg)
        {
            Console.Write("[");
            Console.Write(p);
            Console.WriteLine("] " + msg);
        }
        public static void Title()
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("           This is the Initiator           ");
            Console.WriteLine("===========================================");

        }

        public static ClOrdID EnterClOrdID()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("ClOrdID? ");
                string input = Console.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return new ClOrdID(input);
                }
                Console.WriteLine("Invalid input. Please enter a non-empty ClOrdID.");
            }
        }

        public static OrigClOrdID EnterOrigClOrdID()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("OrigClOrdID? ");
                string input = Console.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return new OrigClOrdID(input);
                }
                Console.WriteLine("Invalid input. Please enter a non-empty OrigClOrdID.");
            }
        }

        public static Symbol EnterSymbol()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Symbol? ");
                string input = Console.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return new Symbol(input);
                }
                Console.WriteLine("Invalid input. Please enter a non-empty Symbol.");
            }
        }

        public static Side EnterSide()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("[1] Buy");
                Console.WriteLine("[2] Sell");
                Console.WriteLine("[3] Sell Short");
                Console.WriteLine("[4] Sell Short Exempt");
                Console.WriteLine("[5] Cross");
                Console.WriteLine("[6] Cross Short");
                Console.WriteLine("[7] Cross Short Exempt");
                Console.Write("Side? ");
                string s = Console.ReadLine().Trim();

                char c = ' ';
                switch (s)
                {
                    case "1": c = Side.BUY; break;
                    case "2": c = Side.SELL; break;
                    case "3": c = Side.SELL_SHORT; break;
                    case "4": c = Side.SELL_SHORT_EXEMPT; break;
                    case "5": c = Side.CROSS; break;
                    case "6": c = Side.CROSS_SHORT; break;
                    case "7": c = 'A'; break;
                    default:
                        Console.WriteLine("Unsupported input. Please enter a number between 1 and 7.");
                        continue;
                }
                return new Side(c);
            }
        }

        public static OrdType EnterOrdType()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("[1] Market");
                Console.WriteLine("[2] Limit");
                Console.WriteLine("[3] Stop");
                Console.WriteLine("[4] Stop Limit");
                Console.Write("OrdType? ");
                string s = Console.ReadLine().Trim();

                char c = ' ';
                switch (s)
                {
                    case "1": c = OrdType.MARKET; break;
                    case "2": c = OrdType.LIMIT; break;
                    case "3": c = OrdType.STOP; break;
                    case "4": c = OrdType.STOP_LIMIT; break;
                    default:
                        Console.WriteLine("Unsupported input. Please enter a number between 1 and 4.");
                        continue;
                }
                return new OrdType(c);
            }
        }

        public static OrderQty EnterOrderQty()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("OrderQty? ");
                string input = Console.ReadLine().Trim();
                if (decimal.TryParse(input, out decimal orderQty) && orderQty > 0)
                {
                    return new OrderQty(orderQty);
                }
                Console.WriteLine("Invalid input. Please enter a positive decimal number for OrderQty.");
            }
        }

        public static Price EnterPrice()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Price? ");
                string input = Console.ReadLine().Trim();
                if (decimal.TryParse(input, out decimal price) && price > 0)
                {
                    return new Price(price);
                }
                Console.WriteLine("Invalid input. Please enter a positive decimal number for Price.");
            }
        }

        public static StopPx EnterStopPx()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("StopPx? ");
                string input = Console.ReadLine().Trim();
                if (decimal.TryParse(input, out decimal stopPx) && stopPx > 0)
                {
                    return new StopPx(stopPx);
                }
                Console.WriteLine("Invalid input. Please enter a positive decimal number for StopPx.");
            }
        }
        public static Account EnterAccount()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Account? ");
                string input = Console.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return new Account(input);
                }
                Console.WriteLine("Invalid input. Please enter a non-empty account name.");
            }
        }
    }}


