using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DataAccessLayer.Models;

[Table("TelegramUsers")]
public class TelegramUsersModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Id пользователя, получаемого из Telegram
    /// </summary>
    [Required]
    public string TelegramUserId { get; set; }

    /// <summary>
    /// Тег пользователя в telegram
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// Имя пользователя, которая устанавливается самим пользователем при регистрации на викторину
    /// </summary>
    [Required]
    public string CustomName { get; set; }
}
