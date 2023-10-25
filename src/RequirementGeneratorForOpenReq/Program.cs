using CsvHelper;
using CsvHelper.Configuration;
using RequirementGeneratorForOpenReq;
using System.Globalization;
using System.Text.Json;

var currentDirectory = Directory.GetCurrentDirectory();


var inputFilePath = Directory.GetFiles(currentDirectory)
    .Where(file => file.EndsWith(".csv"))
    .FirstOrDefault();

if (inputFilePath is null)
{
    PrintMessageAndWaitForInput("Can not file csv file in the current directory");
    return;
}

try
{
    using var reader = new StreamReader(inputFilePath);

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false,
    };

    using var csv = new CsvReader(reader, config);

    var records = csv.GetRecords<RequirementRecord>();
    var openReqRoot = new RequirementsOpenReqRoot()
    {
        Requirements = records,
    };


    var jsonOption = new JsonSerializerOptions()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    var jsonOpenReqRoot = JsonSerializer.Serialize(openReqRoot, jsonOption);

    var resultFilePath = Path.Combine(currentDirectory,$"{Path.GetFileNameWithoutExtension(inputFilePath)}.json");
    File.WriteAllText(resultFilePath, jsonOpenReqRoot);

    PrintMessageAndWaitForInput("Done..");

}
catch(Exception e)
{
    PrintMessageAndWaitForInput($"Exception occured: {e.Message}");
    return;
}


static void PrintMessageAndWaitForInput(string EexitMessage)
{
    Console.WriteLine(EexitMessage);
    Console.ReadLine();
}