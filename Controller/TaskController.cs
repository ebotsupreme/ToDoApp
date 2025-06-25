using ToDoApp.View;
using ToDoApp.Repository;
using ToDoApp.Model;
namespace ToDoApp.Controller;

public class TaskController(TaskRepository taskRepository)
{
    private readonly TaskRepository taskRepository = taskRepository;

    public void ViewAllTasks()
    {
        var allTasks = taskRepository.GetAllTasks();

        if (allTasks.Count == 0)
        {
            Menu.PrintPrompt("No tasks found.");
            return;
        }

        Menu.PrintPrompt("Your tasks: ");
        TaskRepository.GetInfoForTasks(allTasks);
    }

    public void ViewIncompleteTasks()
    {
        var incompleteTasks  = taskRepository.GetIncompleteTasks();

        if (incompleteTasks.Count == 0)
        {
            Menu.PrintPrompt("No incomplete tasks found.");
            return;
        }

        Menu.PrintPrompt("Your incomplete tasks: ");
        TaskRepository.GetInfoForTasks(incompleteTasks);
    }

    public void ViewCompletedTasks()
    {
        var completeTasks = taskRepository.GetCompletedTasks();

        if (completeTasks.Count == 0)
        {
            Menu.PrintPrompt("No completed tasks found.");
            return;
        }

        Menu.PrintPrompt("Your completed tasks: ");
        TaskRepository.GetInfoForTasks(completeTasks);
    }

    public void AddTask()
    {
        Menu.PrintPrompt("Enter a new task");

        string input = Menu.UserInput();
        if (!Menu.ValidateInput(input, "Cannot add an empty task.")) return;

        var (Success, ErrorMessage) = taskRepository.CreateTask(input);

        if (!Success)
        {
            Menu.PrintPrompt("Task added.");
            return;
        }
        
        Menu.PrintPrompt(ErrorMessage ?? "An error occurred while adding the task.");
        
    }

    public void CompleteTask()
    {
        Menu.PrintPrompt("Enter the number of the task to mark as done:");

        int index = GetValidIndexWithRetry();
        bool result = taskRepository.MarkTaskAsDone(index);

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
        taskRepository.DeleteTask(index);
    }

    public void EditTask()
    {
        Menu.PrintPrompt("Enter the number of the task you want to edit:");

        int index = GetValidIndexWithRetry();
        UpdateTaskDescription(index);
    }

    private void UpdateTaskDescription(int index)
    {
        string currentTaskDescription = taskRepository.GetTaskDescription(index);
        Menu.PrintUpdateTaskDescriptionPrompt(currentTaskDescription);

        string input = Menu.UserInput();
        bool isStringValidated = Menu.ValidateInput(input, "Cannot add an empty description.");

        if (isStringValidated) taskRepository.UpdateTask(index, input);
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

            if (index < 0 || index >= taskRepository.GetAllTasksCount())
            {
                int maxTasks = taskRepository.GetAllTasksCount();
                Menu.ShowOutOfRangeMessage(maxTasks);
                continue;
            }

            return index;
        }
    }
}