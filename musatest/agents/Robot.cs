using AgentLibrary;
using AgentLibrary.Attributes;
using MusaCommon;
using MusaCommon.Common.Agent.Networking;
using PlanLibrary;
using PlanLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace domesticRobot.agents
{
    [Agent]
    [Belief("limit(beer,10)")]
    public class Robot : Agent
    {
        public override void onInit()
        {
            AddEvent("delivered(product,quantity,ID)", AgentPerceptionType.AddBelief, typeof(have_beer));
        }


        [Plan]
        [Parameter("product", "quantity")]
        public class have_beer : PlanModel
        {
            [PlanEntryPoint]
            void entry(PlanArgs args)
            {
                string received_product = args.GetArg<string>("product");
                int qty = args.GetArg<int>("quantity");

                Console.WriteLine(string.Format("[{2}] I just received {0} {1}", qty, received_product, Parent.GetName()));

                //has(owner,product)
                Logger.Log(LogLevel.Info, string.Format("[{0}] I'm going to give {1} {2}", Parent.GetName(), qty, received_product));
                AgentMessage msg = new AgentMessage(string.Format("has(owner,\"{0}\")", received_product), InformationType.Tell);
                Parent.SendMessage("Owner", msg);
            }
        }



        /// <summary>
        /// Args:
        /// - "product" --> string
        /// - "quantity" --> int
        /// </summary>
        [Plan]
        [Parameter("product", "quantity")]
        public class buy : PlanModel
        {
            [PlanEntryPoint]
            void entry(PlanArgs args)
            {
                AgentMessage msg = new AgentMessage(string.Format("order(\"{0}\",{1})", args.GetArg<string>("product"), args.GetArg<int>("quantity")), InformationType.Achieve);
                
                Logger.Log(LogLevel.Info, string.Format("[{0}] I'm going to buy ", Parent.GetName(), args.GetArg<string>("product")));
                Parent.SendMessage("Supermarket", msg);
            }
        }
    }
}
