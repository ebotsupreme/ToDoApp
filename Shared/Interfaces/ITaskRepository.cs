using ToDoApp.Model;

namespace ToDoApp.Shared.Interfaces;

public interface ITaskRepository
{
    List<ToDoItem> GetAllTasks();
    List<ToDoItem> GetIncompleteTasks();
    List<ToDoItem> GetCompletedTasks();
    OperationResult CreateTask(string description);
    OperationResult MarkTaskAsDone(int index);
    OperationResult DeleteTask(int index);
    OperationResult UpdateTask(int index, string newDescription);
    int GetAllTasksCount();
    string GetTaskDescription(int index);
}