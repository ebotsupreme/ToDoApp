namespace ToDoApp.Shared.Interfaces;

public interface IMenu
{
    void PrintPrompt(string prompt);
    string UserInput();
    void ShowOutOfRangeMessage(int maxTaskNumber);
    void PrintUpdateTaskDescriptionPrompt(string currentDescription);
}

