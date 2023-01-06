    
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
    
namespace HelloWorld {

    public class program {
        static void Main(string[] args) 
        {
            List<List<som_relations>> xml_relations = xml_parser.parse_xml();
            List<List<record_event>> csv_events = csv_reader.read_csv();
            
            // foreach(KeyValuePair<string, string> entry in graph.events)
            // {
            //     Console.WriteLine("Sender: {0} ----- Receiver: {1}",entry.Key, entry.Value);
            //     // Console.WriteLine(graph.milestones[entry.Value]);
            // }
            int accepted_trace_count= 0;
            int denied_trace_count =0;
            csv_events.RemoveAt(0);
            foreach (var list in csv_events)
            {
                DCR_Graph graph =  new DCR_Graph(xml_relations[0], xml_relations[1], xml_relations[2], 
                                xml_relations[3], xml_relations[4], xml_relations[5]);
                foreach(var item in list)
                {
                    Console.WriteLine(item.event_name);
                    if (!graph.execute(graph.events[item.event_name]))
                    {
                        denied_trace_count++;
                        break;
                    }
                    if (graph.isAccepting())
                    {
                        accepted_trace_count++;
                    }
                    denied_trace_count++;
                }
            }
            Console.WriteLine((accepted_trace_count,denied_trace_count));
        }
    }
}