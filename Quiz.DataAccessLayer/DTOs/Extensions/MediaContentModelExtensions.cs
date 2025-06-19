using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs.Extensions;

public static class MediaContentModelExtensions
{
    public static MediaContentDto ToDto(this MediaContentModel model) =>
        new(model.Id, model.Data);

    public static MediaContentModel FromDto(this MediaContentDto dto) => new()
    {
        Id = dto.Id,
        Data = dto.Data
    };
}
