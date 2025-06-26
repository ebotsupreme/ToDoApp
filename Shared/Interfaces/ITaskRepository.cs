using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskRepository
{
    IReadOnlyList<ToDoItem> GetAllTasks();
    IReadOnlyList<ToDoItem> GetIncompleteTasks();
    IReadOnlyList<ToDoItem> GetCompletedTasks();
    Result<ToDoItem> CreateTask(string description);
    Result<ToDoItem> MarkTaskAsDone(ToDoItem task);
    OperationResult DeleteTask(ToDoItem task);
    OperationResult UpdateTask(ToDoItem task, string newDescription);
    int GetAllTasksCount();
    ToDoItem? GetTaskByIndex(int index);
}