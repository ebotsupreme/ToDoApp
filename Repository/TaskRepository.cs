using ToDoApp.View;

namespace ToDoApp.Model;

public class TaskRepository
{
    private readonly List<ToDoItem> tasks = [];
    private readonly string filePath;

    public TaskRepository(string path)
    {
        filePath = path;
        LoadFromFile(filePath);
    }

    public void GetAllTasks()
    {
        if (GetAllTasksCount() == 0)
        {
            Menu.PrintPrompt("No tasks found.");
            return;
        }

        Menu.PrintPrompt("Your tasks: ");
        GetInfoForTasks(tasks);
    }

    public void GetIncompleteTasks()
    {
        IEnumerable<ToDoItem> query = FilterTasks(false);

        if (!query.Any())
        {
            Menu.PrintPrompt("No incomplete tasks found.");
            return;
        }

        Menu.PrintPrompt("Your incomplete tasks: ");
        GetInfoForTasks(query);
    }

    public void GetCompletedTasks()
    {
        IEnumerable<ToDoItem> query = FilterTasks(true);

        if (!query.Any())
        {
            Menu.PrintPrompt("No completed tasks found.");
            return;
        }

        Menu.PrintPrompt("Your completed tasks: ");
        GetInfoForTasks(query);
    }

    public void CreateTask(string description)
    {
        tasks.Add(new ToDoItem(description));
        Menu.PrintPrompt("Task added.");
        SaveToFile(filePath);
    }

    public bool MarkTaskAsDone(int index)
    {
        if (tasks[index].IsDone)
        {
            return false;
        }

        tasks[index].IsDone = true;
        SaveToFile(filePath);
        return true;
    }

    public void DeleteTask(int index)
    {
        tasks.RemoveAt(index);
        Menu.PrintPrompt("Task deleted.");
        SaveToFile(filePath);
    }
    
    public void UpdateTask(int index, string input)
    {
        tasks[index].Description = input;
        Menu.PrintPrompt("Task updated.");
        SaveToFile(filePath);
    }

    private void LoadFromFile(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                Menu.PrintPrompt("No saved tasks found - starting fresh.");
                return;
            }
            ;

            string[] lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                string[] parts = line.Split("|");

                if (parts.Length == 2)
                {
                    bool isDone = parts[0].Equals("true", StringComparison.OrdinalIgnoreCase);
                    string description = parts[1];
                    tasks.Add(new ToDoItem(description) { IsDone = isDone });
                }
            }
        }
        catch (System.Exception ex)
        {
            Menu.LoadingErrorExeptionMessage(ex.Message);
        }
    }

    private void SaveToFile(string path)
    {
        try
        {
            List<string> lines = [];

            foreach (var task in tasks)
            {
                var line = $"{task.IsDone}|{task.Description}";
                lines.Add(line);
            }

            File.WriteAllLines(path, lines);
        }
        catch (System.Exception ex)
        {
            Menu.SavingErrorExeptionMessage(ex.GetType().Name, ex.Message);
        }
    }

    private static void GetInfoForTasks(IEnumerable<ToDoItem> tasks)
    {
        int i = 0;

        foreach (var task in tasks)
        {
            string status = task.IsDone ? "[X]" : "[ ]";
            Menu.PrintInfoForTasks(++i, status, task.Description);
        }
    }

    private IEnumerable<ToDoItem> FilterTasks(bool isDone)
    {
        IEnumerable<ToDoItem> query =
            tasks.Where((task) => task.IsDone == isDone);
        return query;
    }

    public int GetAllTasksCount()
    {
        return tasks.Count;
    }

    public string GetTaskDescription(int index)
    {
        return tasks[index].Description;
    }
}