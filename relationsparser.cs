
using System;
using System.Collections.Generic;

namespace HelloWorld
{
    public struct som_relations
    {
        private readonly string sender;
        public string Sender { get { return sender; } }

        private readonly string receiver;
        public string Receiver { get { return receiver; } }

        public som_relations(string sender, string receiver)
        {
            this.sender = sender;
            this.receiver = receiver;
        }
    }

    class DCR_Marking {
        public HashSet<string> included {get; set;}
        public HashSet<string> executed {get; set;}
        public HashSet<string> pending {get; set;}

        public DCR_Marking(HashSet<string> included_,HashSet<string> executed_,HashSet<string> pending_){
            this.pending = included_;
            this.executed = executed_;
            this.included = included_;
        }
    }
    class DCR_Graph {

        //Store all the events that are possible

        public Dictionary<string,string> events = new Dictionary<string,string>();


        ///Brug dictionaries istedet her. Det er Csharp equvelant

        public Dictionary<string, HashSet<string>> conditions = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> milestones = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> responses = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> excludes = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> includes = new Dictionary<string, HashSet<string>>();
        public DCR_Marking marking {get; private set;}

        
        /// captures state of the graphHashSet<string>
        public DCR_Graph(List<som_relations> events_, List<som_relations> conditions_, List<som_relations> milestones_, List<som_relations> responses_, 
        List<som_relations> excludes_, List<som_relations> includes_,DCR_Marking marking_) {
            foreach (var even in events_) {
                //For events dictionary - Sender: "First Payment" - Receiver "Activity15"
                this.events.Add(even.Sender.ToLower(),even.Receiver);
                this.conditions.Add(even.Receiver, new HashSet<string>());
                this.milestones.Add(even.Receiver, new HashSet<string>());
                this.responses.Add(even.Receiver, new HashSet<string>());
                this.excludes.Add(even.Receiver, new HashSet<string>());
                this.includes.Add(even.Receiver, new HashSet<string>());
            }
            this.marking = marking_;
            
            // foreach (var item in marking_.included) {
            //     Console.WriteLine("included {0}",item);
            // }
            
            ///conditions - Sender og Receiver er byttet rundt
            foreach (var cond in conditions_) {
                this.conditions[cond.Receiver].Add(cond.Sender);
            }
            ///milestones - Sender og Receiver er byttet rundt
            foreach (var cond in milestones_) {
                this.milestones[cond.Receiver].Add(cond.Sender);
            }
            ///responses
            foreach (var cond in responses_) {
                this.responses[cond.Sender].Add(cond.Receiver);
            }
            ///excludes
            foreach (var cond in excludes_) {
                this.excludes[cond.Sender].Add(cond.Receiver);
            }
            ///includes
            foreach (var cond in includes_) {
                this.includes[cond.Sender].Add(cond.Receiver);
            }
        }

        public bool enabled(string ev){
            //line unnnesesary cause of bugfix kept for posterity
            if (!this.events.ContainsValue(ev)) {return true;}
            //Console.WriteLine("true by default");
            if (!this.marking.included.Contains(ev)){
                // Console.WriteLine("FALSE - ENABLED() l.95 - Not in Included: {0}",ev);
                return false;
            }
            
            //Select included conditions
            HashSet<string> incl_con = new HashSet<string>();
            foreach (var item in conditions[ev]) {   
                if (this.marking.included.Contains(item)) {
                    incl_con.Add(item);
                }
            }

            foreach (var item in incl_con){
                if (!this.marking.executed.Contains(item)){
                    // Console.WriteLine("FALSE - ENABLED() l.109 - Not in Executed: {0}",item);
                    return false;
                }
            }
            
            //Select included milestones
            HashSet<string> included_mile = new HashSet<string>();
            foreach (var item in this.milestones[ev]) {
                if (this.marking.included.Contains(item)){included_mile.Add(item);}

            }

            foreach (var item in this.marking.pending) {
                if (included_mile.Contains(item)){
                    // Console.WriteLine("FALSE - ENABLED() l.123 - Milestone pending error: {0}",item);
                    return false;
                }                
            }

            return true;
        }

        public bool execute(string ev){
            if (!this.events.ContainsKey(ev)) { 
                //Console.WriteLine("cleared by default");
                this.marking.executed.Add(ev);
                this.marking.pending.Remove(ev);
                return true;
            }
            
            ev = this.events[ev];
            if (!this.enabled(ev)) {
                // Console.WriteLine("{0} not enabled",ev);
                return false;
            }
            // DCR_Marking result = marking.clone();
            foreach (var item in this.marking.pending.ToList())
            {
                Console.WriteLine("{0} is pending",item);
            }
            this.marking.executed.Add(ev);
            this.marking.pending.Remove(ev);
            this.marking.pending.UnionWith(this.responses[ev]);
            this.marking.included.Add(ev);
            this.marking.included.ExceptWith(this.excludes[ev]);
            this.marking.included.UnionWith(this.includes[ev]);
            foreach (var item in this.marking.pending.ToList())
            {
                Console.WriteLine("{0} is pending",item);
            }
            return true;
        }

        public HashSet<string> getIncludedPending(){
            HashSet<string> result = new HashSet<string>(this.marking.included);
            result.IntersectWith(this.marking.pending);
            return result;
        }

        public bool isAccepting(){
            return (getIncludedPending().Count == 0);
        }

    }
}