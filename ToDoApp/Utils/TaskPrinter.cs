using ToDoApp.Model;
using ToDoApp.Shared.Interfaces;

namespace ToDoApp.Utils;

public class TaskPrinter(IMenu menu)
{
    public void Print(IEnumerable<ToDoItem> tasks)
    {
        int i = 0;

        foreach (var task in tasks)
        {
            string status = task.IsDone ? "[X]" : "[ ]";
            menu.PrintInfoForTasks(++i, status, task.Description);
        }
    }
}