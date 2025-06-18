using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("QuizState")]
public class QuizStateModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Column(nameof(IsActive))]
    public bool IsActive { get; set; } = true;

    public QuizStateEnum State { get; set; }
}

public enum QuizStateEnum
{
    Waiting = 1,
    Question = 2,
    Statistics = 3,
    Answer = 4,
}
