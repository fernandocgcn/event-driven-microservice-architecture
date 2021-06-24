namespace SocialMedia.PostService.Entities
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
