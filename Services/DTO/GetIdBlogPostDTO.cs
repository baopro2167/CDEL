namespace Services.DTO
{
    public class GetIdBlogPostDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
       
        public string Author { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt{ get; set;}
    }
}
