namespace QuestAPI.Models;

public class QuestConfig
{
    public double RateFromBet { get; set; }
    public double LevelBonusRate { get; set; }
    public int TotalQuestPointsToComplete { get; set; }
    public List<Milestone>? Milestones {get; set;}
}

public class Milestone
{
    public int MilestonePointsToComplete { get; set;}
    public int ChipsAward {get; set;}
}