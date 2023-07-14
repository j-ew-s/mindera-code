using System;
namespace Model.Entities
{
	public abstract record BaseEntity
	{
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

