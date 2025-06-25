using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskService
{
    OperationResult AddNewTask(string description);
    OperationResult UpdateExistingTask(ToDoItem task, string description);
}
