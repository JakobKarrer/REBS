    
using System;
using System.Collections.Generic;

    
namespace HelloWorld {

    public class program {
        static void Main(string[] args) {
            List<List<som_relations>> relations = xml_parser.parse_xml();
            List<List<record_event>> all_events = csv_reader.read_csv();

            DCR_Graph graph =  new DCR_Graph(relations[0], relations[1], relations[2], 
                                                relations[3], relations[4], relations[5]);


            Console.WriteLine(graph.events.ToList());

            
        }
    }
}