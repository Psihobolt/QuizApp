using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizAnswers"), Index([nameof(QuizQuestionId), nameof(Order)], IsUnique = true)]
public class QuizAnswerModel : IEntity
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
    public QuizQuestionModel? QuizQuestion { get; set; }

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
    /// Id картинки, которая относится к ответу и считается альтернативной
    /// </summary>
    public Guid? AlternativeMediaContentId { get; set; }

    /// <summary>
    /// Картинка, которая относится к ответу и считается альтернативной
    /// </summary>
    /// <remarks>
    /// Такая картинка нужна в случаях, когда ответ должен по разному отображаться на этапа "Отображение вопроса" и "Отображение правильного ответа".
    /// И в этом случае, например, на этапе "Отображение вопроса" отображается <see cref="MediaContent"/>,
    /// а если есть в ответе <see cref="AlternativeMediaContent"/> - то на этапе "Отображение правильного ответа" отображается он.
    /// </remarks>
    public MediaContentModel? AlternativeMediaContent { get; set; }

    /// <summary>
    /// Порядок ответа. Нумерация только в рамках вопроса
    /// </summary>
    public int Order { get; set; } = 0;
}
