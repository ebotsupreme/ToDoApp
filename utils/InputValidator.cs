namespace ToDoApp.Utils;

public class InputValidator
{
    public static bool IsInputValid(string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }
}
