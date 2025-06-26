using System;
using ToDoApp.Model;
using ToDoApp.Repository;
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
            return Result<IReadOnlyList<ToDoItem>>.Fail("No tasks found.");
        }

        return Result<IReadOnlyList<ToDoItem>>.Ok(allTasks);
    }

    public Result<IReadOnlyList<ToDoItem>> GetAllIncompleteTasks()
    {
        var incompleteTasks = _repository.GetIncompleteTasks();

        if (incompleteTasks.Count == 0)
        {
            return Result<IReadOnlyList<ToDoItem>>.Fail("No incomplete tasks found.");
        }

        return Result<IReadOnlyList<ToDoItem>>.Ok(incompleteTasks);
    }

    public Result<IReadOnlyList<ToDoItem>> GetAllCompletedTasks()
    {
        var completedTasks = _repository.GetCompletedTasks();

        if (completedTasks.Count == 0)
        {
            return Result<IReadOnlyList<ToDoItem>>.Fail("No completed tasks found.");
        }

        return Result<IReadOnlyList<ToDoItem>>.Ok(completedTasks);
    }

    public OperationResult AddNewTask(string description)
    {
        if (!InputValidator.IsInputValid(description))
        {
            return new OperationResult(false, "Task description cannot be empty.");
        }

        if (IsDuplicateDescription(description))
        {
            return new OperationResult(false, "A task with this description already exits.");
        }

        return _repository.CreateTask(description.Trim());
    }

    public OperationResult CompleteExistingTask(ToDoItem task)
    {
        if (task.IsDone)
        {
            return new OperationResult(false, "That task is already marked as done.");
        }

        return _repository.MarkTaskAsDone(task);
    }

    public OperationResult DeleteExistingTask(ToDoItem task)
    {
        return _repository.DeleteTask(task);
    }

    public OperationResult UpdateExistingTask(ToDoItem task ,string description)
    {
        if (!InputValidator.IsInputValid(description))
        {
            return new OperationResult(false, "Task description cannot be empty.");
        }

        if (IsDuplicateDescription(description, task))
        {
            return new OperationResult(false, "A task with this description already exits.");
        }

        return _repository.UpdateTask(task, description);
    }

    private bool IsDuplicateDescription(string description, ToDoItem? task = null)
    {
        var allTasks = _repository.GetAllTasks();
        return allTasks.Any(t => t.Description
            .Equals(description.Trim(), StringComparison.OrdinalIgnoreCase) && (task == null || t != task));
    }
}
