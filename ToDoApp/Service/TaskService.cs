using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Utils;

namespace ToDoApp.Service;

public class TaskService(ITaskRepository repository) : ITaskService
{
    private readonly ITaskRepository _repository = repository;

    public Result<IReadOnlyList<ToDoItem>> GetAllExistingTasks()
    {
        var allTasks = _repository.GetAllTasks();

        if (allTasks.Count == 0)
        {
            return Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoTasks);
        }

        return Result<IReadOnlyList<ToDoItem>>.Ok(allTasks);
    }

    public Result<IReadOnlyList<ToDoItem>> GetAllIncompleteTasks()
    {
        var incompleteTasks = _repository.GetIncompleteTasks();

        if (incompleteTasks.Count == 0)
        {
            return Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoIncompleteTasks);
        }

        return Result<IReadOnlyList<ToDoItem>>.Ok(incompleteTasks);
    }

    public Result<IReadOnlyList<ToDoItem>> GetAllCompletedTasks()
    {
        var completedTasks = _repository.GetCompletedTasks();

        if (completedTasks.Count == 0)
        {
            return Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoCompletedTasks);
        }

        return Result<IReadOnlyList<ToDoItem>>.Ok(completedTasks);
    }

    public Result<ToDoItem> AddNewTask(string description)
    {
        if (!InputValidator.IsInputValid(description))
        {
            return Result<ToDoItem>.Fail(ErrorMessages.EmptyDescription);
        }

        if (IsDuplicateDescription(description))
        {
            return Result<ToDoItem>.Fail(ErrorMessages.DuplicateDescription);
        }

        return _repository.CreateTask(description.Trim());
    }

    public Result<ToDoItem> CompleteExistingTask(ToDoItem task)
    {
        if (task.IsDone)
        {
            return Result<ToDoItem>.Fail(ErrorMessages.TaskAlreadyDone);
        }

        return _repository.MarkTaskAsDone(task);
    }

    public Result<Unit> DeleteExistingTask(ToDoItem task)
    {
        return _repository.DeleteTask(task);
    }

    public Result<ToDoItem> UpdateExistingTask(ToDoItem task ,string description)
    {
        if (!InputValidator.IsInputValid(description))
        {
            return Result<ToDoItem>.Fail(ErrorMessages.EmptyDescription);
        }

        if (IsDuplicateDescription(description, task))
        {
            return Result<ToDoItem>.Fail(ErrorMessages.DuplicateDescription);
        }

        return _repository.UpdateTask(task, description);
    }

    private bool IsDuplicateDescription(string description, ToDoItem? task = null)
    {
        var allTasks = _repository.GetAllTasks();
        return allTasks.Any(t => t.Description
            .Equals(description.Trim(), StringComparison.OrdinalIgnoreCase) && (task == null || t != task));
    }

    public Result<ToDoItem> GetTaskByIndex(int index)
    {
        var allTasks = _repository.GetAllTasks();
        if (index < 0 || index >= allTasks.Count)
        {
            return Result<ToDoItem>.Fail(string.Format(ErrorMessages.TaskOutOfRangeFormat, index));
        }

        var task = allTasks[index];
        if (task == null)
        {
            return Result<ToDoItem>.Fail(ErrorMessages.TaskNotFound);
        }

        return Result<ToDoItem>.Ok(task);
    }

    public void StoreCurrentTasksList(List<ToDoItem> currentTaskList)
    {
        if (currentTaskList == null || currentTaskList.Count == 0)
        {
            // TODO: log here
            //logger.LogWarning("Attempted to store empty currentTaskList.";
            return;
        }

        _repository.StoreCurrentTasks(currentTaskList);
    }

    public Result<IReadOnlyList<ToDoItem>> GetCurrentTasks()
    {
        var currentTasks = _repository.GetCurrentTasks();
        
        if (currentTasks.Count == 0)
        {
            // TODO: log here
            //logger.Log("An error occurred: " + ex.Message);
            return Result<IReadOnlyList<ToDoItem>>.Fail(ErrorMessages.NoTasks);
        }

        return Result<IReadOnlyList<ToDoItem>>.Ok(currentTasks);
    }
}
