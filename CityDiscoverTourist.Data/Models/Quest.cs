using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CityDiscoverTourist.Data.Models;

public class Quest: BaseEntity
{
    /*[Key]
    public Guid Id { get; set; }*/
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimatedTime { get; set; }
    public string? EstimatedDistance { get; set; }
    public DateTime? AvailableTime { get; set; }
    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public QuestType? QuestType { get; set; }
    public int QuestTypeId { get; set; }

    public QuestOwner? QuestOwner { get; set; }
    public int QuestOwnerId { get; set; }

    public List<QuestItem>? QuestItems { get; set; }

    public List<Area>? Areas { get; set; }

    public List<Competition>? Competitions { get; set; }

    public List<OwnerPayment>? OwnerPayments { get; set; }

    public List<Reward>? Rewards { get; set; }

}