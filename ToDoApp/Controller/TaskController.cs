using ToDoApp.Shared.Interfaces;
using ToDoApp.Utils;
using ToDoApp.Model;
using ToDoApp.Shared;

namespace ToDoApp.Controller;

public class TaskController(ITaskService taskService, IMenu menu)
{
    // TODO not sure if taskRepository is needed for ids yet, currently unused.
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
        menu.PrintPrompt("Enter a new task");

        string input = menu.UserInput();
        if (!InputValidator.IsInputValid(input))
        {
            menu.PrintPrompt(ErrorMessages.EmptyTask);
            return;
        }

        var result = _taskService.AddNewTask(input);
        DisplaySingleTaskResult(result, $"Task added: {result.Data!.Description}");     
    }

    public void CompleteTask()
    {
        menu.PrintPrompt("Enter the number of the task to mark as done:");

        var currentTask = GetCurrentTask();
        if (!currentTask.Success || currentTask.Data == null)
        {
            menu.PrintPrompt(currentTask.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        var result = _taskService.CompleteExistingTask(currentTask.Data);
        DisplaySingleTaskResult(result, "Task is now marked as done.");
    }

    public void RemoveTask()
    {
        menu.PrintPrompt("Enter the number of the task to delete:");

        var currentTask = GetCurrentTask();
        if (!currentTask.Success || currentTask.Data == null)
        {
            menu.PrintPrompt(currentTask.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        var result = _taskService.DeleteExistingTask(currentTask.Data);
        DisplaySingleTaskResult(result, "Task deleted.");
    }

    public void EditTask()
    {
        menu.PrintPrompt("Enter the number of the task you want to edit:");

        var currentTask = GetCurrentTask();
        if (!currentTask.Success || currentTask.Data == null)
        {
            menu.PrintPrompt(currentTask.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        menu.PrintUpdateTaskDescriptionPrompt(currentTask.Data.Description);

        string input = menu.UserInput();
        if (!InputValidator.IsInputValid(input))
        {
            menu.PrintPrompt(ErrorMessages.EmptyDescription);
            return;
        }

        var result = _taskService.UpdateExistingTask(currentTask.Data, input);
        DisplaySingleTaskResult(result, "Task updated.");
    }
    
    public int GetValidIndexWithRetry(IReadOnlyList<ToDoItem> currentDisplayedTasks)
    {
        while (true)
        {
            string input = menu.UserInput();
            bool success = int.TryParse(input, out int index);

            if (!success)
            {
                menu.PrintPrompt(ErrorMessages.NotANumber);
                continue;
            }

            index -= 1;

            if (index < 0 || index >= currentDisplayedTasks.Count)
            {
                int maxTasks = currentDisplayedTasks.Count;
                menu.ShowOutOfRangeMessage(maxTasks);
                continue;
            }

            return index;
        }
    }

    private void DisplayTaskList(Result<IReadOnlyList<ToDoItem>> result, string heading)
    {
        if (!result.Success || result.Data == null)
        {
            menu.PrintPrompt(result.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        menu.PrintPrompt(heading);
        TaskPrinter.Print(result.Data);

        var currentTasks = result.Data.ToList();
        _taskService.StoreCurrentTasksList(currentTasks);
    }

    private void DisplaySingleTaskResult<T>(Result<T> result, string heading)
    {
        if (!result.Success || result.Data == null)
        {
            menu.PrintPrompt(result.ErrorMessage ?? ErrorMessages.ErrorOccurred);
            return;
        }

        menu.PrintPrompt(heading);
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