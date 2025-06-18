using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("MediaContent")]
public class MediaContentModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Column(nameof(Data)), Required]
    public byte[] Data { get; set; } = null!;
}
