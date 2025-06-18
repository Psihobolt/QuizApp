using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("MediaContent")]
public class MediaContentModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Хранит медиа файлы (картинки) в формате base64
    /// </summary>
    [Required]
    public byte[] Data { get; set; } = null!;
}
