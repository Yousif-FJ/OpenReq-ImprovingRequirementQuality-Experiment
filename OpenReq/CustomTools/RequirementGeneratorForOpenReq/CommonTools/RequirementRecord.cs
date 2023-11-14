namespace CommonTools;
public class RequirementRecord
{
    public string? Id { get; set; }
    public string? Text { get; set; }
}

public class RequirementsOpenReqRoot
{
    public IList<RequirementRecord> Requirements { get; set; } = new List<RequirementRecord>();
}
