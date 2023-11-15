using Newtonsoft.Json;
using OutputTypeCounter;

var inputFilePath = Directory.GetFiles(Directory.GetCurrentDirectory())
    .Where(file => file.EndsWith(".json"))
    .First();

var inputString = File.ReadAllText(inputFilePath);


var IdAmbiguityListKeyValuePair = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<AmbiguityModel>>>(inputString)!;

var titleCount = new Dictionary<string, int>();

foreach (var ambiguityList in IdAmbiguityListKeyValuePair.Values)
{
    foreach (var ambiguity in ambiguityList)
    {
        if (ambiguity is null)
        {
            continue;
        }

        if (titleCount.TryGetValue(ambiguity.Title!, out int count))
        {
            titleCount[ambiguity.Title!] = count + 1;
        }
        else
        {
            titleCount[ambiguity.Title!] = 1;
        }
    }
}

Console.WriteLine(JsonConvert.SerializeObject(titleCount, Formatting.Indented));
