using System.Runtime.Serialization;

namespace TasksAPI.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException(string message) : base(message)
        {
        }
    }
}