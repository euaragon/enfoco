using System;
using System.ComponentModel.DataAnnotations;



namespace enfoco2.Models
{

    public enum NoticeCategory
    {
        tribunal,
        fiscalia,
        etica
    }

    public enum NoticeSection
    {
        category1,
        category2,
        category3
    }

    public class Notice
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Subtitle { get; set; }

        public required string Issue { get; set; }

        public required string Text { get; set; }

        public required string Img { get; set; }

        public bool IsFeatured { get; set; } = false;

        public NoticeCategory Category { get; set; }

        public NoticeSection Section { get; set; }

       
    }

    public class NoticeDto : Notice
    {
    }

}
