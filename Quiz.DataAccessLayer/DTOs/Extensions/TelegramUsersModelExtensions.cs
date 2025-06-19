using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs.Extensions;

public static class TelegramUsersModelExtensions
{
    public static TelegramUserDto ToDto(this TelegramUsersModel model) =>
        new(model.Id, model.TelegramUserId, model.Username, model.CustomName);

    public static TelegramUsersModel FromDto(this TelegramUserDto dto) => new()
    {
        Id = dto.Id,
        TelegramUserId = dto.TelegramUserId,
        Username = dto.Username,
        CustomName = dto.CustomName
    };
}
