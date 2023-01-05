
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
        public Dictionary<string, HashSet<string>> milsetones = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> responses = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> excludes = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> includes = new Dictionary<string, HashSet<string>>();
        public DCR_Marking marking =  new DCR_Marking();

        
        /// captures state of the graphHashSet<string>
        public DCR_Graph(List<som_relations> events, List<som_relations> conditions, List<som_relations> milestones, List<som_relations> responses, List<som_relations> excludes, List<som_relations> includes) {
            foreach (var even in events) {

                //Console.WriteLine(even.Sender);
                //Console.WriteLine(even.Receiver);
                //Add events
                //IN this case sender is event and reciever is the activity
                
                this.events.Add(even.Receiver,even.Receiver);
                this.conditions.Add(even.Receiver, new HashSet<string>());
                this.milsetones.Add(even.Receiver, new HashSet<string>());
                this.responses.Add(even.Receiver, new HashSet<string>());
                this.excludes.Add(even.Receiver, new HashSet<string>());
                this.includes.Add(even.Receiver, new HashSet<string>());

            }
            
            ///Conditions
            foreach (var cond in conditions) {
                this.conditions[cond.Sender].Add(cond.Receiver);
            }
            ///milestones
            foreach (var cond in milestones) {
                //Console.WriteLine(cond.Sender);
                //Console.WriteLine(cond.Receiver);
                this.milsetones[cond.Sender].Add(cond.Receiver);
                Console.WriteLine(cond.Sender);
            }
            ///responses
            foreach (var cond in responses) {
                //Console.WriteLine(cond.Sender);
                this.responses[cond.Sender].Add(cond.Receiver);
            }
            ///excludes
            foreach (var cond in excludes) {
                this.excludes[cond.Sender].Add(cond.Receiver);
            }
            ///includes
            foreach (var cond in includes) {
                this.includes[cond.Sender].Add(cond.Receiver);
            }
        }
        public bool enables(DCR_Graph graph, string ev){
            if (!this.events.ContainsKey(ev)) {return true;}
            if (!this.marking.included.Contains(ev)){return false;}
            
            //Select included conditions
            HashSet<string> inccon = new HashSet<string>();
            foreach (var item in conditions[ev]) {   
                if (this.marking.included.Contains(item)) {
                    inccon.Add(item);
                }
            }
            foreach (var item in inccon){
                if (!this.marking.executed.Contains(item)){return false;}
            }
            
            //Select included milestones
            HashSet<string> included_mile = new HashSet<string>();
            foreach (var item in this.milsetones[ev]) {
                if (this.marking.included.Contains(item)){included_mile.Add(item);}

            }
            foreach (var item in marking.pending) {
                if (included_mile.Contains(item)){return false;}                
            }
            return true;
        }

    }
}