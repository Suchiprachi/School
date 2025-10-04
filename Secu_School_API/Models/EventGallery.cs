
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("event_gallery")]
    public class EventGallery
    {
        [Key]
        [Column("event_gallery_id")]
        public int EventGalleryId { get; set; }
        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }
        [Required]
        [Column("title")]
        [MaxLength(255)]
        public string Title { get; set; }
        [Column("description")]
        public string? Description { get; set; }

        [Column("media_type")]
        [MaxLength(10)]
        public string MediaType { get; set; } = "image";
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<GalleryFile> Files { get; set; } = new List<GalleryFile>();
        
    }
}

