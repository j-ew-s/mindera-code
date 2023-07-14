using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public record Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }
    }
}