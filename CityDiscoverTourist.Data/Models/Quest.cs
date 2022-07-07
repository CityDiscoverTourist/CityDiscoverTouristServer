using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CityDiscoverTourist.Data.Models;

public class Quest: BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? ImagePath { get; set; }
    public string? EstimatedTime { get; set; }
    public string? AvailableTime { get; set; }
    public string? EstimatedDistance { get; set; }
    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }

    public QuestType? QuestType { get; set; }
    public int QuestTypeId { get; set; }

    public Area? Area { get; set; }
    public int AreaId { get; set; }

    public QuestOwner? QuestOwner { get; set; }
    public int? QuestOwnerId { get; set; }

    public List<CustomerQuest>? CustomerQuests { get; set; }

    public List<QuestItem>? QuestItems { get; set; }

    //public List<QuestReward>? QuestRewards { get; set; }
    public List<OwnerPayment>? OwnerPayments { get; set; }
    public List<Payment>? Payments { get; set; }
}