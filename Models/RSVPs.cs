using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class RSVPs : BaseEntity
    {
        [Key]
        public int RSVPid { get; set; }

        public int UsersUserId { get; set; }
        public Users UsersUser { get; set; }

        public int WeddingsWeddingId { get; set; }
        public Weddings WeddingsWedding { get; set; }
    }
    
}