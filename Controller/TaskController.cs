using ToDoApp.Model;
using ToDoApp.View;

namespace ToDoApp.Controller;

public class TaskController(TaskManager taskManager)
{
    private readonly TaskManager taskManager = taskManager;

    public void ViewAllTasks()
    {
        taskManager.GetAllTasks();
    }

    public void ViewIncompleteTasks()
    {
        taskManager.GetIncompleteTasks();
    }

    public void ViewCompletedTasks()
    {
        taskManager.GetCompletedTasks();
    }

    public void AddTask()
    {
        Menu.PrintPrompt("Enter a new task");

        string input = Menu.UserInput();
        bool isStringValidated = Menu.ValidateInput(input, "Cannot add an empty task.");

        if (isStringValidated) taskManager.CreateTask(input);
    }

    public void CompleteTask()
    {
        Menu.PrintPrompt("Enter the number of the task to mark as done:");

        int index = GetValidIndexWithRetry();
        bool result = taskManager.MarkTaskAsDone(index);

        if (result)
        {
            Console.WriteLine();
            Console.WriteLine("Task is now marked as done.");
        }
        else
        {
            Console.WriteLine("That task is already marked as done.");
        }
    }
    
    public void RemoveTask()
    {
        Menu.PrintPrompt("Enter the number of the task to delete:");

        int index = GetValidIndexWithRetry();
        taskManager.DeleteTask(index);
    }

    public void EditTask()
    {
        Menu.PrintPrompt("Enter the number of the task you want to edit:");

        int index = GetValidIndexWithRetry();
        UpdateTaskDescription(index);
    }

    private void UpdateTaskDescription(int index)
    {
        string currentTaskDescription = taskManager.GetTaskDescription(index);
        Menu.PrintUpdateTaskDescriptionPrompt(currentTaskDescription);

        string input = Menu.UserInput();
        bool isStringValidated = Menu.ValidateInput(input, "Cannot add an empty description.");

        if (isStringValidated) taskManager.UpdateTask(index, input);
    }
    
    public int GetValidIndexWithRetry()
    {
        while (true)
        {
            string input = Menu.UserInput();
            bool success = int.TryParse(input, out int index);

            if (!success)
            {
                Menu.PrintPrompt("Input is not a number. Please enter a valid task number.");
                continue;
            }

            index -= 1;

            if (index < 0 || index >= taskManager.GetAllTasksCount())
            {
                int maxTasks = taskManager.GetAllTasksCount();
                Menu.ShowOutOfRangeMessage(maxTasks);
                continue;
            }

            return index;
        }
    }
}