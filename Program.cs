using QuickFix.Transport;
using QuickFix;
using System.Reflection;
using InitiatorLD;

public class Program {
    static void Main(string[] args)
    {



        var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        SessionSettings settings = new SessionSettings(outputDirectory + "\\InitiatorConfig.cfg");
        InitiatorApp myApp = new InitiatorApp();
        IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
        ILogFactory logFactory = new FileLogFactory(settings);
        SocketInitiator initiator = new SocketInitiator(
            myApp,
            storeFactory,
            settings,
            logFactory);
    }

}
