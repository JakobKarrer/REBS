using System.Xml.Linq;

namespace HelloWorld
{
    static class xml_parser {
        public static void parse_xml() {


            var xml = XDocument.Load("C:/Users/JHK/Downloads/a1.2.2 (1).xml");

            // We pair the label names e.g. "Fill out application" with their corresponding eventname e.g. "Activity0" here.
            // Then the pairs are inserted into a list of tuples
            var labelNames = xml.Descendants("labelMapping")
                .Attributes("labelId")
                .Select(element => element.Value);

            var eventNames = xml.Descendants("labelMapping")
                .Attributes("eventId")
                .Select(element => element.Value);

            var labelEventPairs = labelNames.Zip(eventNames, (x, y) => new Tuple<string, string>(x, y)).ToList();

            foreach (Tuple<string, string> row in labelEventPairs)
            {
                Console.WriteLine(row);
            }

            // Conditions
            var condition_source = xml.Descendants("condition")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var condition_target = xml.Descendants("condition")
                .Attributes("targetId")
                .Select(element => element.Value);

            var conditionPairs = condition_source.Zip(condition_target, (x, y) => new Tuple<string, string>(x, y)).ToList();

            foreach (Tuple<string, string> row in conditionPairs)
            {
                Console.WriteLine("Condition {0}: ",row);
            }


            //Responses
            var response_source = xml.Descendants("response")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var response_target = xml.Descendants("response")
                .Attributes("targetId")
                .Select(element => element.Value);

            var responsePairs = response_source.Zip(response_target, (x, y) => new Tuple<string, string>(x, y)).ToList();

            foreach (Tuple<string, string> row in responsePairs)
            {
                Console.WriteLine("Response {0}: ",row);
            }

            //Exclude
            var exclude_source = xml.Descendants("exclude")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var exclude_target = xml.Descendants("exclude")
                .Attributes("targetId")
                .Select(element => element.Value);

            var excludePairs = exclude_source.Zip(exclude_target, (x, y) => new Tuple<string, string>(x, y)).ToList();

            foreach (Tuple<string, string> row in excludePairs)
            {
                Console.WriteLine("Exclude {0}: ",row);
            }


            //Include
            var include_source = xml.Descendants("include")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var include_target = xml.Descendants("include")
                .Attributes("targetId")
                .Select(element => element.Value);

            var includePairs = include_source.Zip(include_target, (x, y) => new Tuple<string, string>(x, y)).ToList();

            foreach (Tuple<string, string> row in includePairs)
            {
                Console.WriteLine("Include {0}: ",row);
            }


            //Milestone
            var milestone_source = xml.Descendants("milestone")
                .Attributes("sourceId")
                .Select(element => element.Value);

            var milestone_target = xml.Descendants("milestone")
                .Attributes("targetId")
                .Select(element => element.Value);

            var milestonePairs = milestone_source.Zip(milestone_target, (x, y) => new Tuple<string, string>(x, y)).ToList();

            foreach (Tuple<string, string> row in milestonePairs)
            {
                Console.WriteLine("Milestone {0}: ",row);
            }
        }
        
    }
}