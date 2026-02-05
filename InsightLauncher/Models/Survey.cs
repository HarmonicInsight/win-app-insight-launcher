namespace InsightLauncher.Models;

public class Survey
{
    public string Id { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public SurveyType Type { get; set; }
    public List<SurveyOption> Options { get; set; } = new();
    public DateTime? Deadline { get; set; }
    public bool IsCompleted { get; set; }
}

public class SurveyOption
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public enum SurveyType
{
    Single,
    Multiple,
    Text
}
