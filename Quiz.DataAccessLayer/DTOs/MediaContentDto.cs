namespace Quiz.DataAccessLayer.DTOs;

public readonly record struct MediaContentDto(
    Guid Id,
    byte[] Data);