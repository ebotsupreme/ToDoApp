using ToDoApp.View;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Utils;
using ToDoApp.Model;
using ToDoApp.Shared;

namespace ToDoApp.Controller;

public class TaskController(ITaskRepository taskRepository, ITaskService taskService)
{
    private readonly ITaskRepository taskRepository = taskRepository;
    private readonly ITaskService _taskService = taskService;

    public void ViewAllTasks()
    {
        var result = _taskService.GetAllExistingTasks();
        DisplayTaskList(result, "Your tasks: ");
    }

    public void ViewIncompleteTasks()
    {
        var result = _taskService.GetAllIncompleteTasks();
        DisplayTaskList(result, "Your incomplete tasks: ");
    }

    public void ViewCompletedTasks()
    {
        var result = _taskService.GetAllCompletedTasks();
        DisplayTaskList(result, "Your completed tasks: ");
    }

    public void AddTask()
    {
        Menu.PrintPrompt("Enter a new task");

        string input = Menu.UserInput();
        if (!InputValidator.IsInputValid(input))
        {
            Menu.PrintPrompt("Cannot add an empty task.");
            return;
        }

        var result = _taskService.AddNewTask(input);
        DisplaySingleTaskResult(result, $"Task added: {result.Data!.Description}");     
    }

    public void CompleteTask()
    {
        Menu.PrintPrompt("Enter the number of the task to mark as done:");

        int index = GetValidIndexWithRetry();
        ToDoItem? task = taskRepository.GetTaskByIndex(index);
        
        if (task == null)
        {
            Menu.PrintPrompt("Task not found.");
            return;
        }

        var result = _taskService.CompleteExistingTask(task);
        DisplaySingleTaskResult(result, "Task is now marked as done.");
    }

    public void RemoveTask()
    {
        Menu.PrintPrompt("Enter the number of the task to delete:");

        int index = GetValidIndexWithRetry();
        ToDoItem? task = taskRepository.GetTaskByIndex(index);

        if (task == null) {
            Menu.PrintPrompt("Task not found.");
            return;
        }

        var (Success, ErrorMessage) = _taskService.DeleteExistingTask(task);

        if (!Success)
        {
            Menu.PrintPrompt(ErrorMessage ?? "An error occurred while deleting the task.");
            return;
        }
        
        Menu.PrintPrompt("Task deleted.");
    }

    public void EditTask()
    {
        Menu.PrintPrompt("Enter the number of the task you want to edit:");

        int index = GetValidIndexWithRetry();
        UpdateTaskDescription(index);
    }

    private void UpdateTaskDescription(int index)
    {
        var task = taskRepository.GetTaskByIndex(index);
        if (task == null)
        {
            Menu.PrintPrompt("Task not found.");
            return;    
        }

        Menu.PrintUpdateTaskDescriptionPrompt(task.Description);

        string input = Menu.UserInput();
        if (!InputValidator.IsInputValid(input))
        {
            Menu.PrintPrompt("Cannot add an empty description.");
            return;
        }

        var (Success, ErrorMessage) = _taskService.UpdateExistingTask(task, input);

        if (!Success)
        {
            Menu.PrintPrompt(ErrorMessage ?? "An error occurred while updating the task.");
            return;
        }

        Menu.PrintPrompt("Task updated.");
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

    private static void DisplayTaskList(Result<IReadOnlyList<ToDoItem>> result, string heading)
    {
        if (!result.Success || result.Data == null)
        {
            Menu.PrintPrompt(result.ErrorMessage ?? "An error occured.");
            return;
        }

        Menu.PrintPrompt(heading);
        TaskPrinter.Print(result.Data);
    }

    private static void DisplaySingleTaskResult<T>(Result<T> result, string heading)
    {
        if (!result.Success || result.Data == null)
        {
            Menu.PrintPrompt(result.ErrorMessage ?? "An error occured.");
            return;
        }

        Menu.PrintPrompt(heading);
        Console.WriteLine(result.Data);
    }
}