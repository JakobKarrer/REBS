using System.Xml.Linq;

namespace HelloWorld
{
    static class xml_parser {
        public static List<List<som_relations>> parse_relations_xml(string path) {

            var xml = XDocument.Load(path);

            // We pair the label names e.g. "Fill out application" with their corresponding eventname e.g. "Activity0" here.
            // Then the pairs are inserted into a list of tuples
            var labelNames = xml.Descendants("labelMapping")
                .Attributes("labelId")
                .Select(element => element.Value);

            var eventNames = xml.Descendants("labelMapping")
                .Attributes("eventId")
                .Select(element => element.Value);

            var labelEventPairs = labelNames.Zip(eventNames, (x, y) => new som_relations(x, y)).ToList();

            // Conditions bytter rundt på sender og modtager
            var condition_source = xml.Descendants("condition")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var condition_target = xml.Descendants("condition")
                .Attributes("targetId")
                .Select(element => element.Value);

            var conditionPairs = condition_source.Zip(condition_target, (x, y) => new som_relations(x,y)).ToList();

            //Responses
            var response_source = xml.Descendants("response")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var response_target = xml.Descendants("response")
                .Attributes("targetId")
                .Select(element => element.Value);

            var responsePairs = response_source.Zip(response_target, (x, y) => new som_relations(x,y)).ToList();

            //Exclude
            var exclude_source = xml.Descendants("exclude")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var exclude_target = xml.Descendants("exclude")
                .Attributes("targetId")
                .Select(element => element.Value);

            var excludePairs = exclude_source.Zip(exclude_target, (x, y) => new som_relations(x,y)).ToList();

            //Include
            var include_source = xml.Descendants("include")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var include_target = xml.Descendants("include")
                .Attributes("targetId")
                .Select(element => element.Value);

            var includePairs = include_source.Zip(include_target, (x, y) => new som_relations(x,y)).ToList();


            //Milestone - byttet rundt på sender og modtager
            var milestone_source = xml.Descendants("milestone")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var milestone_target = xml.Descendants("milestone")
                .Attributes("targetId")
                .Select(element => element.Value);

            var milestonePairs = milestone_source.Zip(milestone_target, (x, y) => new som_relations(x,y)).ToList();
            
            List<List<som_relations>> retList = new List<List<som_relations>> {labelEventPairs,conditionPairs, milestonePairs,responsePairs,
                excludePairs,includePairs,};
            
            return retList;
        }
        public static DCR_Marking parse_markings_xml(string path) 
        {
            var xml = XDocument.Load(path);
            // We pair the label names e.g. "Fill out application" with their corresponding eventname e.g. "Activity0" here.
            // Then the pairs are inserted into a list of tuples
            var included = xml.Root
             .Descendants("included")
             .Elements("event")
             .Select(x => x.Attribute("id").Value).ToList();
            

            var executed = xml.Root
             .Descendants("executed")
             .Elements("event")
             .Select(x => x.Attribute("id").Value).ToList();

            var pending = xml.Root
             .Descendants("pendingResponses")
             .Elements("event")
             .Select(x => x.Attribute("id").Value).ToList();

            HashSet<string> inc = new HashSet<string>(included);
            HashSet<string> exec = new HashSet<string>(executed);
            HashSet<string> pend = new HashSet<string>(pending);
            // foreach (var item in included) {
            //     Console.WriteLine(item);
            // }
            // inc.UnionWith(included);
            // exec.UnionWith(executed);
            // pend.UnionWith(pending);
            DCR_Marking marking = new DCR_Marking(inc,exec,pend);

            return marking;

        }
    }
}
