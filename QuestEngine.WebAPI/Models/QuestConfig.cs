namespace QuestEngine.WebAPI.Models;

public class QuestConfig
{
    public double RateFromBet { get; set; }
    public double LevelBonusRate { get; set; }
    public int TotalQuestPointsToComplete { get; set; }
    public List<MilestoneConfig>? Milestones {get; set;}
}

public class MilestoneConfig
{
    public int MilestonePointsToComplete { get; set;}
    public int ChipsAward {get; set;}
}