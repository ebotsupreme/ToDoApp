namespace ToDoApp.View;

public static class Menu
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

    public static string UserInput()
    {
        return Console.ReadLine() ?? "";
    }

    public static void PrintPrompt(string prompt)
    {
        Console.WriteLine();
        Console.WriteLine(prompt);
    }

    public static void ShowOutOfRangeMessage(int maxTaskNumber)
    {
        Console.WriteLine();
        Console.WriteLine($"Task number is out of range. Enter a number between 1 and {maxTaskNumber}.");
    }

    public static void PrintUpdateTaskDescriptionPrompt(string currentTaskDescription)
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
        Console.WriteLine($"Error loading tasks: {message}");
    }

    public static void SavingErrorExeptionMessage(string typeName, string message)
    {
        Console.WriteLine($"Could not save tasks: {typeName} - {message}");
    }
    
    public static void PrintInfoForTasks(int index, string status, string description)
    {
        Console.WriteLine($"{index}. {status} {description}");

    }
}