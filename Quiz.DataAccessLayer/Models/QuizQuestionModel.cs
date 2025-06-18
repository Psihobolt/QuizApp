using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizQuestions")]
public class QuizQuestionModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string Question { get; set; }

    public List<QuizAnswerModel> Answers { get; set; } = [];

    public MediaContentModel? MediaContent { get; set; } = null;
}
