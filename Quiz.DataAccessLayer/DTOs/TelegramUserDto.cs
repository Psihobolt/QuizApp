namespace Quiz.DataAccessLayer.DTOs;

public readonly record struct TelegramUserDto(
    Guid Id,
    string TelegramUserId,
    string Username,
    string CustomName);