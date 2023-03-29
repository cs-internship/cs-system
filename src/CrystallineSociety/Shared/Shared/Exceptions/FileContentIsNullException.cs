using System.Runtime.Serialization;

namespace CrystallineSociety.Shared.Exceptions;

public class FileContentIsNullException : IOException
{
    public FileContentIsNullException()
    : base(nameof(AppStrings.FileContentIsNullException))
    {
    }

    public FileContentIsNullException(string message)
        : base(message)
    {
    }

    public FileContentIsNullException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
