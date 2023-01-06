
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
        public HashSet<string> executed = new HashSet<string>();
        public HashSet<string> pending = new HashSet<string>();
        public HashSet<string> included = new HashSet<string>();
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
        public DCR_Marking marking =  new DCR_Marking();

        
        /// captures state of the graphHashSet<string>
        public DCR_Graph(List<som_relations> events_, List<som_relations> conditions_, List<som_relations> milestones_, List<som_relations> responses_, 
        List<som_relations> excludes_, List<som_relations> includes_) {
            foreach (var even in events_) {
                //For events dictionary - Sender: "First Payment" - Receiver "Activity15"
                this.events.Add(even.Sender,even.Receiver);
                this.conditions.Add(even.Receiver, new HashSet<string>());
                this.milestones.Add(even.Receiver, new HashSet<string>());
                this.responses.Add(even.Receiver, new HashSet<string>());
                this.excludes.Add(even.Receiver, new HashSet<string>());
                this.includes.Add(even.Receiver, new HashSet<string>());
            }
            
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

        public bool enabled(DCR_Graph graph, string ev){
            if (!this.events.ContainsKey(ev)) {return true;}
            if (!this.marking.included.Contains(ev)){return false;}
            
            //Select included conditions
            HashSet<string> incl_con = new HashSet<string>();
            foreach (var item in conditions[ev]) {   
                if (this.marking.included.Contains(item)) {
                    incl_con.Add(item);
                }
            }
            foreach (var item in incl_con){
                if (!this.marking.executed.Contains(item)){return false;}
            }
            
            //Select included milestones
            HashSet<string> included_mile = new HashSet<string>();
            foreach (var item in this.milestones[ev]) {
                if (this.marking.included.Contains(item)){included_mile.Add(item);}

            }
            foreach (var item in marking.pending) {
                if (included_mile.Contains(item)){return false;}                
            }
            return true;
        }

        public DCR_Marking execute(DCR_Marking marking, string ev){
            if (!this.events.ContainsKey(ev)){return true;}

            if (!this.enabled(marking,ev)) {return marking;}

            DCR_Marking result = marking.clone();

            result.executed.Add(ev);
            result.pending.Remove(ev);
            result.pending.AddRange(this.responses[ev].ToList());
            result.included.ExceptWith(this.excludes[ev].ToList());
            result.included.AddRange(this.includes[ev].ToList());

            return result;
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