using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskRepository
{
    IReadOnlyList<ToDoItem> GetAllTasks();
    List<ToDoItem> GetIncompleteTasks();
    List<ToDoItem> GetCompletedTasks();
    OperationResult CreateTask(string description);
    OperationResult MarkTaskAsDone(int index);
    OperationResult DeleteTask(int index);
    OperationResult UpdateTask(ToDoItem task, string newDescription);
    int GetAllTasksCount();
    ToDoItem? GetTaskByIndex(int index);
}