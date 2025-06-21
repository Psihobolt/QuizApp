using LightResults;

namespace Quiz.WebAPI.Exceptions;

public class RepositoryException : Exception
{
    public IEnumerable<IError> Errors { get; }
    public RepositoryException(IEnumerable<IError> errors)
    {
        Errors = errors;
    }
}
