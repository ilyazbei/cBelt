using System;

namespace WeddingPlanner
{
    public abstract class BaseEntity
    {
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public BaseEntity() 
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
    }
}