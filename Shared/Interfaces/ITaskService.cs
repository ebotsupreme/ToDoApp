using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskService
{
    Result<IReadOnlyList<ToDoItem>> GetAllExistingTasks();
    Result<IReadOnlyList<ToDoItem>> GetAllIncompleteTasks();
    Result<IReadOnlyList<ToDoItem>> GetAllCompletedTasks();
    Result<ToDoItem> AddNewTask(string description);
    Result<ToDoItem> CompleteExistingTask(ToDoItem task);
    OperationResult UpdateExistingTask(ToDoItem task, string description);
    OperationResult DeleteExistingTask(ToDoItem task);
}
