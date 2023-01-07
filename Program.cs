    
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
    
namespace HelloWorld {

    public class program {
        static void Main(string[] args) 
        {
            // DCR_Marking xml_markings = xml_parser.parse_markings_xml("code2.xml");

            List<List<record_event>> csv_events = csv_reader.read_csv();

            int accepted_trace_count= 0;
            int denied_trace_count =0;
            csv_events.RemoveAt(0);

            foreach (var list in csv_events)
            {
                // DCR_Marking markings_ = xml_markings.clone();
                List<List<som_relations>> xml_relations = xml_parser.parse_relations_xml("code2.xml");
                DCR_Marking xml_markings = xml_parser.parse_markings_xml("code2.xml");

                DCR_Graph graph =  new DCR_Graph(xml_relations[0], xml_relations[1], xml_relations[2], 
                                xml_relations[3], xml_relations[4], xml_relations[5],xml_markings);
                foreach(var item in list)
                {                    
                    if (!graph.execute(item.event_name))
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
                
                Console.WriteLine('\n');

            }
            Console.WriteLine((accepted_trace_count,denied_trace_count));
        }
    }
}