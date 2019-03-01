using System.Collections.Generic;

namespace BookAPP.Entity
{
    public class GetChaptersResponse : BaseResponse<List<ChapterModel>>
    {
    }

    public class ChapterModel
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public int SiteID { get; set; }
    }
}
