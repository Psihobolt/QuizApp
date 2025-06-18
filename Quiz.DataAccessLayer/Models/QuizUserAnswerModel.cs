using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizUserAnswer")]
public class QuizUserAnswerModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Id игрока
    /// </summary>
    [Required]
    public Guid PlayerId { get; set; }

    public TelegramUsersModel Player { get; set; }

    /// <summary>
    /// Id вопроса, на который дал ответ игрок
    /// </summary>
    [Required]
    public Guid QuizQuestionId { get; set; }

    /// <summary>
    /// Вопрос, на который дал ответ игрок
    /// </summary>
    public QuizQuestionModel QuizQuestion { get; set; }

    /// <summary>
    /// Id ответа, на который дал ответ игрок
    /// </summary>
    [Required]
    public Guid AnswerId { get; set; }

    /// <summary>
    /// Ответ, на который дал ответ игрок
    /// </summary>
    public QuizAnswerModel Answer { get; set; }
}
