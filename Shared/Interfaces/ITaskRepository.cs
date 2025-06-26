using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskRepository
{
    IReadOnlyList<ToDoItem> GetAllTasks();
    IReadOnlyList<ToDoItem> GetIncompleteTasks();
    IReadOnlyList<ToDoItem> GetCompletedTasks();
    Result<ToDoItem> CreateTask(string description);
    Result<ToDoItem> MarkTaskAsDone(ToDoItem task);
    Result<Unit> DeleteTask(ToDoItem task);
    Result<ToDoItem> UpdateTask(ToDoItem task, string newDescription);
    int GetAllTasksCount();
}