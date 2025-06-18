using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Models;

[Table("MediaContent")]
public class MediaContentModel : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Хранит медиа файлы (картинки) в формате base64
    /// </summary>
    [Required]
    public byte[] Data { get; set; } = null!;
}
