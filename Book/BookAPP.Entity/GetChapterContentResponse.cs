namespace BookAPP.Entity
{
    public class GetChapterContentResponse : BaseResponse<ChapterContentModel>
    {
    }

    public class ChapterContentModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
