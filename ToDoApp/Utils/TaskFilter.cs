using ToDoApp.Model;

namespace ToDoApp.Utils;

public static class TaskFilter
{
    public static IEnumerable<ToDoItem> FilterTasks(List<ToDoItem> allTasks, bool isDone)
    {
        return allTasks.Where((task) => task.IsDone == isDone);
    }
}