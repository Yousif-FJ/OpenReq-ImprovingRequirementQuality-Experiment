namespace RequirementGeneratorForOpenReq;
public class RequirementRecord
{
    public string? Id { get; set; }
    public string? Text { get; set; }
}

public class RequirementsOpenReqRoot
{
    public IEnumerable<RequirementRecord> Requirements { get; set; } = new List<RequirementRecord>();
}
