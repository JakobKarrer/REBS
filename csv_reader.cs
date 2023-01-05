using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace HelloWorld
{
    class csv_reader
  {
    public static List<List<record_event>> read_csv() {
        var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture) {
            HasHeaderRecord = false,
            Delimiter = ";",
        };

        using var streamReader = File.OpenText("log.csv");
        using var csvReader = new CsvReader(streamReader, csvConfig);

        var events = csvReader.GetRecords<record_event>();

        string current_id = "hello";
        List<List<record_event>> trace_list = new List<List<record_event>>();
        foreach (var ev in events) {
          if (current_id != ev.id) {
              List<record_event> trace = new List<record_event>();
              trace_list.Add(trace);
              current_id = ev.id;
          }
          trace_list.Last().Add(ev);
        }
        return trace_list;
    }
  }
}