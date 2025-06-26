using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskService
{
    OperationResult AddNewTask(string description);
    OperationResult CompleteExistingTask(ToDoItem task);
    OperationResult UpdateExistingTask(ToDoItem task, string description);
}
