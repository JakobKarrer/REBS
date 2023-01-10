    
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
    
namespace HelloWorld {

    public class program {
        static void Main(string[] args) 
        {
            // DCR_Marking xml_markings = xml_parser.parse_markings_xml("code2.xml");

            List<List<record_event>> csv_events = csv_reader.read_csv();
            // var debuggin = csv_events.Take(1);
            csv_events.RemoveAt(0);
            List<string> dcr_graphs = new List<string> {
                "dcr_graphs/r1.xml","dcr_graphs/r2.xml","dcr_graphs/r3.xml","dcr_graphs/r4.xml","dcr_graphs/r5.xml","dcr_graphs/r6.xml","dcr_graphs/r7.xml","dcr_graphs/r8.xml"
                };
            foreach(var dcr_xml in dcr_graphs)
            {
                int accepted_trace_count= 0;
                int denied_trace_count =0;
                foreach (var list in csv_events)
                {
                    // DCR_Marking markings_ = xml_markings.clone();
                    List<List<som_relations>> xml_relations = xml_parser.parse_relations_xml(dcr_xml);
                    DCR_Marking xml_markings = xml_parser.parse_markings_xml(dcr_xml);

                    DCR_Graph graph =  new DCR_Graph(xml_relations[0], xml_relations[1], xml_relations[2], 
                                    xml_relations[3], xml_relations[4], xml_relations[5],xml_markings);

                    // Console.WriteLine(list[0].id);
                    var mistake = 0;
                    foreach(var item in list)
                    {
                        // Console.WriteLine(item.event_name);         
                        if (!graph.execute(item.event_name))
                        {
                            // Console.WriteLine("FAIL - Execute returned False \n");
                            mistake++;
                        }
                    }
                    // foreach (var item in graph.marking.pending.ToList())
                    // {
                    //     // Console.WriteLine("{0} is pending",item);
                    // }
                    
                    if (graph.isAccepting()) {
                        if ((mistake ==0)) {
                            accepted_trace_count++;
                            continue;
                        } 
                    }
                    denied_trace_count++;
                    
                    

                }
                Console.WriteLine((accepted_trace_count,denied_trace_count));
                Console.WriteLine(dcr_xml);
            }
        }
    }
}