using AgentLibrary;
using MusaCommon;
using MusaLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musatest
{
    class Program
    {
        static void Main(string[] args)
        {
            MusaInitializer.MUSAInitializer.Initialize();

            ModuleProvider.Get().Resolve<ILogger>().AddFragment(new ConsoleLoggerFragment());
            ModuleProvider.Get().Resolve<ILogger>().AddFragment(new FileLoggerFragment());
            ModuleProvider.Get().Resolve<ILogger>().GetFragment<IFileLoggerFragment>().SetFilename(@"C:\\Users\davide\Documents\musa_log.txt");
            ModuleProvider.Get().Resolve<ILogger>().SetMinimumLogLevel(2);

            AgentEnvironement env = AgentEnvironement.GetInstance();

            env.WaitForAgents();
        }
    }
}
