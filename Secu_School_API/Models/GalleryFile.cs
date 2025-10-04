using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Secu_School_API.Models
{
    [Table("gallery_file")]
    public class GalleryFile
    {
        [Key]
        [Column("file_id")]
        public int FileId { get; set; }

        [Required]
        [Column("event_gallery_id")]
        public int EventGalleryId { get; set; }

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

        public EventGallery EventGallery { get; set; }
    }
}
