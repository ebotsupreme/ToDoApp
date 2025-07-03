using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;

namespace ToDoApp.View;

public class Menu : IMenu
{
    public static void Show()
    {
        Console.WriteLine();
        Console.WriteLine("What woud you like to do?");
        Console.WriteLine("1. View all tasks");
        Console.WriteLine("2. View incomplete tasks");
        Console.WriteLine("3. View completed tasks");
        Console.WriteLine("4. Add a task");
        Console.WriteLine("5. Mark task as done");
        Console.WriteLine("6. Delete a task");
        Console.WriteLine("7. Edit a task");
        Console.WriteLine("8. Exit");
        Console.Write("> ");
    }

    public string UserInput()
    {
        return Console.ReadLine() ?? "";
    }

    public void PrintPrompt(string prompt)
    {
        Console.WriteLine();
        Console.WriteLine(prompt);
    }

    public void ShowOutOfRangeMessage(int maxTaskNumber)
    {
        Console.WriteLine();
        Console.WriteLine(string.Format(ErrorMessages.TaskOutOfRangeFormat, maxTaskNumber));
    }

    public void PrintUpdateTaskDescriptionPrompt(string currentTaskDescription)
    {
        Console.WriteLine();
        Console.WriteLine("Current task description: " + currentTaskDescription);
        Console.WriteLine("Enter an updated description: ");
    }

    public static bool ValidateInput(string input, string message)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine(message);
            return false;
        }

        return true;
    }

    public static void LoadingErrorExeptionMessage(string message)
    {
        Console.WriteLine(string.Format(ErrorMessages.LoadingErrorFormat, message));
    }

    public static void SavingErrorExeptionMessage(string typeName, string message)
    {
        Console.WriteLine(string.Format(ErrorMessages.SavingErrorFormat, typeName, message));
    }
    
    public static void PrintInfoForTasks(int index, string status, string description)
    {
        Console.WriteLine($"{index}. {status} {description}");

    }
}