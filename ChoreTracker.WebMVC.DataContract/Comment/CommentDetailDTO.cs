namespace ChoreTracker.WebMVC.DataContract.Comment
{
    public class CommentDetailDTO
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string Poster { get; set; }
        public int GroupId { get; set; }
    }
}
