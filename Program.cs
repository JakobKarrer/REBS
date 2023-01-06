    
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
    
namespace HelloWorld {

    public class program {
        static void Main(string[] args) {
            List<List<som_relations>> relations = xml_parser.parse_xml();
            List<List<record_event>> all_events = csv_reader.read_csv();

            DCR_Graph graph =  new DCR_Graph(relations[0], relations[1], relations[2], 
                                                relations[3], relations[4], relations[5]);
            
            foreach(KeyValuePair<string, string> entry in graph.events)
            {
                Console.WriteLine("Sender: {0} ----- Receiver: {1}",entry.Key, entry.Value);
                // Console.WriteLine(graph.milestones[entry.Value]);
            }
            // Console.WriteLine(graph.milestones.Count);
            // Console.WriteLine(graph.includes.Count);
            // Console.WriteLine(graph.excludes.Count);
            // Console.WriteLine(graph.conditions.Count);
            // Console.WriteLine(graph.events.Count);


            
        }
    }
}