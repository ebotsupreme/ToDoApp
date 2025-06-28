using ToDoApp.Model;
using ToDoApp.View;

namespace ToDoApp.Utils;

public static class TaskPrinter
{
    public static void Print(IEnumerable<ToDoItem> tasks)
    {
        int i = 0;

        foreach (var task in tasks)
        {
            string status = task.IsDone ? "[X]" : "[ ]";
            Menu.PrintInfoForTasks(++i, status, task.Description);
        }
    }
}