using ToDoApp.View;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Utils;
using ToDoApp.Model;

namespace ToDoApp.Controller;

public class TaskController(ITaskRepository taskRepository, ITaskService taskService)
{
    private readonly ITaskRepository taskRepository = taskRepository;
    private readonly ITaskService _taskService = taskService;

    public void ViewAllTasks()
    {
        var result = _taskService.GetAllExistingTasks();

        if (!result.Success || result.Data == null)
        {
            Menu.PrintPrompt(result.ErrorMessage ?? "An error occurred.");
            return;
        }

        Menu.PrintPrompt("Your tasks: ");
        TaskPrinter.Print(result.Data);
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
        TaskPrinter.Print(incompleteTasks);
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
        TaskPrinter.Print(completeTasks);
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

        var (Success, ErrorMessage) = _taskService.AddNewTask(input);

        if (!Success)
        {
            Menu.PrintPrompt(ErrorMessage ?? "An error occurred while adding the task.");
            return;
        }

        Menu.PrintPrompt("Task added.");        
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

        var (Success, ErrorMessage) = _taskService.CompleteExistingTask(task);

        if (!Success)
        {
            Menu.PrintPrompt(ErrorMessage ?? "An error occurred while marking the task as done.");
            return;
        }

        Menu.PrintPrompt("Task is now marked as done.");
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
}