using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("gallery")]
    public class Gallery
    {
        [Key]
        [Column("gallery_id")]
        public int GalleryId { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [Column("title")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("file_name")]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [Column("file_path")]
        [MaxLength(500)]
        public string FilePath { get; set; }

        [Column("media_type")]
        [MaxLength(10)]
        public string MediaType { get; set; } = "image";

        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        // Optional navigation property
        public virtual School School { get; set; }
    }
}
