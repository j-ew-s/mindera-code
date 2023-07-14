using System;

namespace Model.Entities
{
    public record Comment : BaseEntity
    {     
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public Post Post { get; set; }
    }
}