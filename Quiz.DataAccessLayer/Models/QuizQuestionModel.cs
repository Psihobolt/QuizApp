using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizQuestions"), Index(nameof(Order), IsUnique = true)]
public class QuizQuestionModel : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Текст вопроса
    /// </summary>
    [Required, MaxLength(50)]
    public string Question { get; set; }

    /// <summary>
    /// Список ответов на вопрос
    /// </summary>
    public List<QuizAnswerModel> Answers { get; set; } = [];

    /// <summary>
    /// Id картинки, которая относится к вопросу
    /// </summary>
    public Guid? MediaContentId { get; set; }

    /// <summary>
    /// Картинка, которая относится к вопросу
    /// </summary>
    public MediaContentModel? MediaContent { get; set; } = null;

    /// <summary>
    /// Порядок вопроса. Нумерация сквозная
    /// </summary>
    public int Order { get; set; }
}
