using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace QuestAPI.Data{

    public class QuestDbContext : DbContext
    {
        public QuestDbContext(DbContextOptions<QuestDbContext> options) : base(options)
        {}
        public DbSet<QuestState> QuestStates { get; set;}

    }
    public class QuestState
    {
        [Key]
        public string PlayerId { get; set;} = "";
        public int TotalQuestPoints { get; set;}
        public int LastMilestoneIndexCompleted { get; set;}
    }
}