using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizState")]
public class QuizStateModel : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Какое активное состояние у викторины. Активным может быть только одно состояние
    /// </summary>
    [Required]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Этап викторины, на котором сейчас викторина
    /// </summary>
    [Required]
    public QuizStateEnum State { get; set; }

    /// <summary>
    /// Внешний ключ к таблице QuizQuestion
    /// <remarks>Если активное состояние - WaitingStart - то внешний ключ должен ссылаться на первый вопрос в первом туре</remarks>
    /// </summary>
    public Guid QuizQuestionId { get; set; }

    public QuizQuestionModel? QuizQuestion { get; set; }
}

public enum QuizStateEnum
{
    // Этап пока викторина не началась и возможна регистрация
    WaitingForStart = 0,
    // Этап вопрос
    DisplayQuestion = 1,
    // Этап статистики полученных ответов
    DisplayAnswerStats = 2,
    // Этап отображения ответов
    DisplayCorrectAnswer = 3,
    DisplayFullStats = 4,
    DisplayTop5 = 5
}
