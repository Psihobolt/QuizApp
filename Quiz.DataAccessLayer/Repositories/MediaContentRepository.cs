using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.Repositories;

public class MediaContentRepository : Repository<MediaContentModel>, IRepository<MediaContentModel>
{
    public MediaContentRepository(QuizContext context) : base(context)
    {
    }
} 