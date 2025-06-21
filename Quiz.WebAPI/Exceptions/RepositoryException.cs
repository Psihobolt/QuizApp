using LightResults;

namespace Quiz.WebAPI.Exceptions;

public class RepositoryException : Exception
{
    public IError Errors { get; }
    public RepositoryException(IError errors)
    {
        Errors = errors;
    }
}
