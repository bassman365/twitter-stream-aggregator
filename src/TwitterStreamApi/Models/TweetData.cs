namespace TwitterStreamApi.Models
{
    public class TweetData
    {
        public string? Id { get; set; }
        public string? Text { get; set; }

        public TweetEntity Entities { get; set; } = new TweetEntity();
    }
}
