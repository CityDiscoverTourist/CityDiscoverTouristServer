namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CommentResponseModel
{
    public int Id { get; set; }
    public string? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? ImagePath { get; set; }
    public string? FeedBack { get; set; }
    public int Rating { get; set; }
    public DateTime? CreatedDate { get; set; }
    public bool IsFeedbackApproved { get; set; }
}