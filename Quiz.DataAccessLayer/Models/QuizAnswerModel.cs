using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizAnswers")]
public class QuizAnswerModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey(nameof(QuizQuestion))]
    public Guid QuizQuestionId { get; set; }

    public QuizQuestionModel QuizQuestion { get; set; }

    [Column(nameof(Answer)), Required]
    public string Answer { get; set; } = null!;

    [ForeignKey(nameof(MediaContent))]
    public Guid? MediaContentId {get; set; } = null;

    public MediaContentModel? MediaContent { get; set; } = null;
}
