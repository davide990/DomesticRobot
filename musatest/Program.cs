using AgentLibrary;
using MusaCommon;
using MusaLogger;
using MusaInitializer;

namespace musatest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize MUSA
            MUSAInitializer.Initialize();

            //Add a console logger
            ModuleProvider.Get().Resolve<ILogger>().AddFragment(new ConsoleLoggerFragment());

            /* FILE LOGGING
            ModuleProvider.Get().Resolve<ILogger>().AddFragment(new FileLoggerFragment());
            ModuleProvider.Get().Resolve<ILogger>().GetFragment<IFileLoggerFragment>().SetFilename(@"/tmp/musa_log.txt"); */

            //Set the minimum log level to INFO
            ModuleProvider.Get().Resolve<ILogger>().SetMinimumLogLevel(2);

            //Discover the agents within this project
            MUSAInitializer.DiscoverAgents();

            //Wait for agents
            AgentEnvironement.GetRootEnv().WaitForAgents();
        }
    }
}
