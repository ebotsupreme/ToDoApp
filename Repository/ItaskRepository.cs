using ToDoApp.Model;

namespace ToDoApp.Repository;

public interface ITaskRepository
{
    List<ToDoItem> GetAllTasks();
    List<ToDoItem> GetIncompleteTasks();
    List<ToDoItem> GetCompletedTasks();
    void CreateTask(string description);
    bool MarkTaskAsDone(int index);
    void DeleteTask(int index);
    void UpdateTask(int index, string newDescription);
    int GetAllTasksCount();
    string GetTaskDescription(int index);
}