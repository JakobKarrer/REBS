using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace HelloWorld
{
    class csv_reader
  {
    public static void read_csv() {
        var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ";",
        };

        using var streamReader = File.OpenText("log.csv");
        using var csvReader = new CsvReader(streamReader, csvConfig);

        var events = csvReader.GetRecords<record_event>();


        foreach (var ev in events) {
                Console.Write(ev.id);
                Console.WriteLine(ev.event_name);
        }
        //record User(string FirstName, String LastName, string Occupation);

    }
  }
}