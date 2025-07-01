using ToDoApp.View;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Utils;
using ToDoApp.Model;
using ToDoApp.Shared;

namespace ToDoApp.Controller;

public class TaskController(ITaskRepository taskRepository, ITaskService taskService)
{
    // TODO not sure if taskRepository is needed for ids yet, currently unused.
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
            Menu.PrintPrompt(ErrorMessages.EmptyTask);
            return;
        }

        var result = _taskService.AddNewTask(input);
        DisplaySingleTaskResult(result, $"Task added: {result.Data!.Description}");     
    }

    public void CompleteTask()
    {
        Menu.PrintPrompt("Enter the number of the task to mark as done:");

        var currentTask = GetCurrentTask();
        if (!currentTask.Success || currentTask.Data == null)
        {
            Menu.PrintPrompt(currentTask.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        var result = _taskService.CompleteExistingTask(currentTask.Data);
        DisplaySingleTaskResult(result, "Task is now marked as done.");
    }

    public void RemoveTask()
    {
        Menu.PrintPrompt("Enter the number of the task to delete:");

        var currentTask = GetCurrentTask();
        if (!currentTask.Success || currentTask.Data == null)
        {
            Menu.PrintPrompt(currentTask.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        var result = _taskService.DeleteExistingTask(currentTask.Data);
        DisplaySingleTaskResult(result, "Task deleted.");
    }

    public void EditTask()
    {
        Menu.PrintPrompt("Enter the number of the task you want to edit:");

        var currentTask = GetCurrentTask();
        if (!currentTask.Success || currentTask.Data == null)
        {
            Menu.PrintPrompt(currentTask.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        Menu.PrintUpdateTaskDescriptionPrompt(currentTask.Data.Description);

        string input = Menu.UserInput();
        if (!InputValidator.IsInputValid(input))
        {
            Menu.PrintPrompt(ErrorMessages.EmptyDescription);
            return;
        }

        var result = _taskService.UpdateExistingTask(currentTask.Data, input);
        DisplaySingleTaskResult(result, "Task updated.");
    }
    
    public static int GetValidIndexWithRetry(IReadOnlyList<ToDoItem> currentDisplayedTasks)
    {
        while (true)
        {
            string input = Menu.UserInput();
            bool success = int.TryParse(input, out int index);

            if (!success)
            {
                Menu.PrintPrompt(ErrorMessages.NotANumber);
                continue;
            }

            index -= 1;

            if (index < 0 || index >= currentDisplayedTasks.Count)
            {
                int maxTasks = currentDisplayedTasks.Count;
                Menu.ShowOutOfRangeMessage(maxTasks);
                continue;
            }

            return index;
        }
    }

    private void DisplayTaskList(Result<IReadOnlyList<ToDoItem>> result, string heading)
    {
        if (!result.Success || result.Data == null)
        {
            Menu.PrintPrompt(result.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        Menu.PrintPrompt(heading);
        TaskPrinter.Print(result.Data);

        var currentTasks = result.Data.ToList();
        _taskService.StoreCurrentTasksList(currentTasks);
    }

    private static void DisplaySingleTaskResult<T>(Result<T> result, string heading)
    {
        if (!result.Success || result.Data == null)
        {
            Menu.PrintPrompt(result.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        Menu.PrintPrompt(heading);
        Console.WriteLine(result.Data);
    }

    private Result<ToDoItem> GetCurrentTask()
    {
        var currentDisplayedTasksResult = _taskService.GetCurrentTasks();
        
        if (!currentDisplayedTasksResult.Success || currentDisplayedTasksResult.Data == null)
        {
            return Result<ToDoItem>.Fail(currentDisplayedTasksResult.ErrorMessage ?? ErrorMessages.ErrorOccurred);
        }

        var currentDisplayedTasks = currentDisplayedTasksResult.Data!;
        int index = GetValidIndexWithRetry(currentDisplayedTasks);
        var currentTask = currentDisplayedTasks[index];

        return Result<ToDoItem>.Ok(currentTask);
    }
}