using System.Threading.Tasks;
using ToDoApp.Model;
using ToDoApp.Shared;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Utils;

namespace ToDoApp.Service;

public class TaskService(ITaskRepository repository) : ITaskService
{
    private readonly ITaskRepository _repository = repository;

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
            .Equals(description.Trim(), StringComparison.OrdinalIgnoreCase) && (task == null | t != task));
    }
}
