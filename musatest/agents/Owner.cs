using AgentLibrary;
using AgentLibrary.Attributes;
using MusaCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusaCommon.Common.Agent.Networking;
using PlanLibrary;
using System.Threading;
using PlanLibrary.Attributes;

namespace domesticRobot.agents
{
    [Agent]
    public class Owner : Agent
    {
        public override void onInit()
        {
            AddEvent("has(owner,product)", AgentPerceptionType.AddBelief, typeof(drink));
            AddEvent("has(owner,product)", AgentPerceptionType.RemoveBelief, typeof(get));

            AchieveGoal(typeof(get));
            AchieveGoal(typeof(check_bored));
        }

        [Plan]//("!has(owner,product)")]
        public class check_bored : PlanModel
        {
            [PlanEntryPoint]
            void entry(PlanArgs args)
            {
                var rand = new Random().Next(1, 5);

                Logger.Log(LogLevel.Info, string.Format("[{0}] I'm bored...", Parent.GetName()));

                Console.WriteLine("--> " + rand * 1000);
                Thread.Sleep(rand * 1000);
                /*if (rand > 2)
                {
                    Console.WriteLine("Achieving get");
                    Parent.AchieveGoal(typeof(get));
                }

                Console.WriteLine("Achieving check bored");*/
                Parent.AchieveGoal(typeof(check_bored));
            }
        }

        [Plan]//("!has(owner,product)")]
        public class get : PlanModel
        {
            [PlanEntryPoint]
            void entry(PlanArgs args)
            {
                Logger.Log(LogLevel.Info, string.Format("[{0}] Robot, go buy me some beer!", Parent.GetName()));
                
                AgentMessage mm = new AgentMessage(string.Format("buy(\"{0}\",{1})", "beer", 1), InformationType.Achieve);
                Parent.SendMessage("Robot", mm);
            }
        }

        [Plan]//("has(owner,product)", typeof(check_bored))]
        [Parameter("product","quantity")]
        public class drink : PlanModel
        {
            [PlanEntryPoint]
            void entry(PlanArgs args)
            {
                var what_I_received = args.GetArg<string>("product");
                var how_many = args.GetArg<string>("quantity");
                Logger.Log(LogLevel.Info, string.Format("[{0}] I'm drinking {1} {2}", Parent.GetName(), how_many, what_I_received));
                Parent.RemoveBelief(ModuleProvider.Get().Resolve<IFormulaUtils>().Parse("has(owner,product)"));
            }
        }
    }
}
