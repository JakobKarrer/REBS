    
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
    
namespace HelloWorld {

    public class program {
        static void Main(string[] args) 
        {
            // DCR_Marking xml_markings = xml_parser.parse_markings_xml("code2.xml");

            List<List<record_event>> csv_events = csv_reader.read_csv();
            var debuggin = csv_events.Take(3);
            int accepted_trace_count= 0;
            int denied_trace_count =0;
            csv_events.RemoveAt(0);

            foreach (var list in debuggin)
            {
                // DCR_Marking markings_ = xml_markings.clone();
                List<List<som_relations>> xml_relations = xml_parser.parse_relations_xml("code2.xml");
                DCR_Marking xml_markings = xml_parser.parse_markings_xml("code2.xml");

                DCR_Graph graph =  new DCR_Graph(xml_relations[0], xml_relations[1], xml_relations[2], 
                                xml_relations[3], xml_relations[4], xml_relations[5],xml_markings);

                Console.WriteLine(list[0].id);
                var mistake = 0;
                foreach(var item in list)
                {
                    Console.WriteLine(item.event_name);         
                    if (!graph.execute(item.event_name))
                    {
                        Console.WriteLine("mistake");
                        mistake++;
                    }
                }
                foreach (var item in graph.marking.pending.ToList())
                {
                    Console.WriteLine(item);
                }
                
                if (graph.isAccepting()) {
                    if ((mistake ==0)) {
                        accepted_trace_count++;
                        continue;
                    } 
                }
                denied_trace_count++;
                
                

            }
            Console.WriteLine((accepted_trace_count,denied_trace_count));
        }
    }
}