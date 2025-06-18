using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizAnswers"), Index([nameof(QuizQuestionId), nameof(Order)], IsUnique = true)]
public class QuizAnswerModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Id вопроса, к которому относится ответ
    /// </summary>
    public Guid QuizQuestionId { get; set; }

    /// <summary>
    /// Вопрос, к которому относится ответ
    /// </summary>
    public QuizQuestionModel QuizQuestion { get; set; }

    /// <summary>
    /// Является ли ответ правильным для вопроса, к которому он относится
    /// <remarks>На некоторые вопросы может быть несколько правильных ответов, либо ни одного</remarks>
    /// </summary>
    [Required]
    public bool IsCorrect { get; set; }

    /// <summary>
    /// Текст ответа
    /// </summary>
    [Required]
    public string Answer { get; set; }

    /// <summary>
    /// Id картинки, которая относится к ответу
    /// </summary>
    public Guid? MediaContentId {get; set; }

    /// <summary>
    /// Картинка, которая относится к ответу
    /// </summary>
    public MediaContentModel? MediaContent { get; set; }

    /// <summary>
    /// Порядок ответа. Нумерация только в рамках вопроса
    /// </summary>
    public int Order { get; set; } = 0;
}
