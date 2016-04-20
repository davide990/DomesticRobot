using AgentLibrary;
using AgentLibrary.Attributes;
using MusaCommon;
using PlanLibrary;
using PlanLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 last_order_id(1). // initial belief

// plan to achieve the goal "order" for agent Ag
+!order(Product,Qtd)[source(Ag)] : true
  <- ?last_order_id(N);
     OrderId = N + 1;
     -+last_order_id(OrderId);
     deliver(Product,Qtd);
     .send(Ag, tell, delivered(Product,Qtd,OrderId)).
 */

namespace domesticRobot.agents
{
    [Agent]
    [Belief("last_order_id(1)")]
    public class Supermarket : Agent
    {
        [Plan]
        [Parameter("product", "quantity")]
        public class order : PlanModel
        {
            //For parsing formula
            IFormulaUtils fp = ModuleProvider.Get().Resolve<IFormulaUtils>();
            //For creating assignments
            IAssignmentFactory af = ModuleProvider.Get().Resolve<IAssignmentFactory>();

            [PlanEntryPoint]
            void entry(PlanArgs args)
            {
                string product = args.GetArg<string>("product");
                int quantity = args.GetArg<int>("quantity");

                var last_order_formula = fp.Parse("last_order_id(n)");
                List<IAssignment> assignments;
                Parent.TestCondition(last_order_formula, out assignments);

                if (assignments.Count <= 0)
                    return;

                var new_order_ID = (int)assignments[0].GetValue() + 1;

                last_order_formula.Unify(af.CreateAssignment(assignments[0].GetName(), new_order_ID));

                //Parent == Supermarket
                Parent.UpdateBelief(last_order_formula);

                var delivered_formula = string.Format("delivered(\"{0}\",{1},{2})", product, quantity, new_order_ID);

                Logger.Log(LogLevel.Info, string.Format("[{0}] Delivering {0} {1} to {2}", Parent.GetName(), quantity, product, SourceAgent.AgentName));

                AgentMessage msg = new AgentMessage(delivered_formula, InformationType.Tell);
                Parent.SendMessage(this.SourceAgent, msg);
            }
        }

    }
}
