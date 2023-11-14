using CommonTools;
using CsvHelper;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Json;

var currentDirectory = Directory.GetCurrentDirectory();

var inputDirectory = Path.Combine(currentDirectory, "sample");

var intermediateDirectory = Path.Combine(currentDirectory, "intermediate");
Directory.CreateDirectory(intermediateDirectory);


var outputDirectory = Path.Combine(currentDirectory, "output");
Directory.CreateDirectory(outputDirectory);

var inputFilePaths = Directory.GetFiles(inputDirectory)
    .Where(file => file.EndsWith(".txt"));


foreach (var filePath in inputFilePaths)
{
    var requirements = File.ReadAllText(filePath).Split(Environment.NewLine);

    var id = 1;
    var requestObj = new RequirementsOpenReqRoot();

    foreach (var requirement in requirements)
    {
        requestObj.Requirements.Add(new RequirementRecord
        {
            Id = id.ToString(),
            Text = requirement
        });

        ++id;
    }

    using var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("http://localhost:9799");
    var response = await httpClient.PostAsJsonAsync("/check-quality", requestObj);

    var result = await response.Content.ReadAsStringAsync();
    var json = JsonConvert.DeserializeObject(result);
    result = JsonConvert.SerializeObject(json, Formatting.Indented);

    var intermediateFilePath = Path.Combine(intermediateDirectory, $"{Path.GetFileNameWithoutExtension(filePath)}.csv");
    using var writer = new StreamWriter(intermediateFilePath);
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
    csv.WriteRecords(requestObj.Requirements);
    

    var resultFilePath = Path.Combine(outputDirectory, $"{Path.GetFileNameWithoutExtension(filePath)}.json");
    File.WriteAllText(resultFilePath, result);
}
