namespace Shared.Utils;

public static class StringTools
{
    public static string GenerateSuccessMessage(string message)
    {
        return $"({DateTime.Now}) Message: {message}.";
    }
}