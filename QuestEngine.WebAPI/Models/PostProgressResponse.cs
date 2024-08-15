namespace QuestEngine.WebAPI.Models;

public class PostProgressResponse
{
    public int QuestPointsEarned {get; set;}
    public double TotalQuestPercentCompleted {get; set;}
    public Milestone? MilestonesCompleted {get; set;}
    
}
public class Milestone
{
    public int MilestoneIndex { get; set;}
    public int ChipsAwarded {get; set;}
}