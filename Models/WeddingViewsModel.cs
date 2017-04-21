using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class WeddingViewModel : BaseEntity
    {
        [Required(ErrorMessage="Wedder One is Required")]
        [MinLength(3, ErrorMessage="Wedder One must be 3 characters long")]
        public string wedderOne { get; set; }
        
        [Required(ErrorMessage="Wedder Two is Required")]
        [MinLength(3, ErrorMessage="Wedder Two must be 3 characters long")]
        public string wedderTwo { get; set; }
        
        [Required(ErrorMessage="Date is Required")]
        public DateTime? date { get; set; }
        
        [Required(ErrorMessage="Wedding Address is Required")]
        public string weddingAddress { get; set; }

    }
}