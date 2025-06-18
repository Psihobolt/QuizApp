using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizState")]
public class QuizStateModel
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

    public QuizQuestionModel QuizQuestion { get; set; }
}

public enum QuizStateEnum
{
    // Этап пока викторина не началась и возможна регистрация
    WaitingStart = 0,
    // Пауза между турами
    Waiting = 1,
    // Этап вопрос
    Question = 2,
    // Этап статистики полученных ответов
    Statistics = 3,
    // Этап отображения ответов
    Answer = 4,
    Final = 5
}
