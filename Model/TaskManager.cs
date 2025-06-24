namespace ToDoApp.Model;

public class TaskManager
{
    private readonly List<ToDoItem> tasks = [];
    private readonly string filePath;

    public TaskManager(string path)
    {
        filePath = path;
        LoadFromFile(filePath);
    }

    public void GetAllTasks()
    {
        if (GetAllTasksCount() == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No tasks found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Your tasks: ");
        GetTaskInfo(tasks);
    }

    public void CreateTask(string description)
    {
        tasks.Add(new ToDoItem(description));
        Console.WriteLine();
        Console.WriteLine("Task added.");
        SaveToFile(filePath);
    }

    public void MarkTaskAsDone(int index)
    {
        if (tasks[index].IsDone)
        {
            Console.WriteLine();
            Console.WriteLine("That task is already marked as done.");
            return;
        }

        tasks[index].IsDone = true;
        Console.WriteLine("Task marked as done.");
        SaveToFile(filePath);
    }

    public void DeleteTask(int index)
    {
        tasks.RemoveAt(index);
        Console.WriteLine("Task deleted.");
        SaveToFile(filePath);
    }

    public void LoadFromFile(string path)
    {
        if (!File.Exists(path)) return;

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

    public void SaveToFile(string path)
    {
        List<string> lines = [];

        foreach (var task in tasks)
        {
            var line = $"{task.IsDone}|{task.Description}";
            lines.Add(line);
        }

        File.WriteAllLines(path, lines);
    }

    public void UpdateTask(int index, string input)
    {
        tasks[index].Description = input;
        Console.WriteLine("Task updated.");
        SaveToFile(filePath);
    }

    public void GetIncompleteTasks()
    {
        IEnumerable<ToDoItem> query = FilterTasks(false);

        if (!query.Any())
        {
            Console.WriteLine();
            Console.WriteLine("No incomplete tasks found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Your incomplete tasks: ");
        GetTaskInfo(query);
    }

    public void GetCompletedTasks()
    {
        IEnumerable<ToDoItem> query = FilterTasks(true);

        if (!query.Any())
        {
            Console.WriteLine();
            Console.WriteLine("No completed tasks found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Your completed tasks: ");
        GetTaskInfo(query);
    }

    private static void GetTaskInfo(IEnumerable<ToDoItem> tasks)
    {
        int i = 0;

        foreach (var task in tasks)
        {
            string status = task.IsDone ? "[X]" : "[ ]";
            Console.WriteLine($"{++i}. {status} {task.Description}");
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