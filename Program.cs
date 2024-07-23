using QuickFix.Transport;
using QuickFix;
using System.Reflection;
using InitiatorLD;
using static System.Net.Mime.MediaTypeNames;
using QuickFix.Fields;

public class Program {
    static void Main(string[] args)
    {

        try
        {
            InitiatorApp.Run();

        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
        Environment.Exit(1);



    }

}
