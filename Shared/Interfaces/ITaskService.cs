using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskService
{
    Result<IReadOnlyList<ToDoItem>> GetAllExistingTasks();
    Result<IReadOnlyList<ToDoItem>> GetAllIncompleteTasks();
    Result<IReadOnlyList<ToDoItem>> GetAllCompletedTasks();
    OperationResult AddNewTask(string description);
    OperationResult CompleteExistingTask(ToDoItem task);
    OperationResult UpdateExistingTask(ToDoItem task, string description);
    OperationResult DeleteExistingTask(ToDoItem task);
}
