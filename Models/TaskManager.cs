class TaskManager()
{
    readonly List<ToDoItem> tasks = [];

    public void GetAllTasks()
    {
        if (tasks.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Your tasks:");
            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                string status = task.IsDone ? "[X]" : "[]";
                Console.WriteLine($"{i + 1}. {status} {task.Description}");
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("No tasks found.");
        }
    }

    public void CreateTask(string description)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            tasks.Add(new ToDoItem(description));
            Console.WriteLine();
            Console.WriteLine("Task added.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Cannot add an empty task.");
        }
    }

    public bool IsIndexValid(int index)
    {
        if (index < 0 || index >= tasks.Count)
        {
            Console.WriteLine();
            Console.Write("Invalid task number.");
            return false;
        }

        return true;
    }

    public void MarkTaskAsDone(int index)
    {
        if (!IsIndexValid(index))
        {
            return;
        }

        if (tasks[index].IsDone)
        {
            Console.Write("That task is already marked as done.");
            return;
        }

        tasks[index].IsDone = true;
        Console.WriteLine("Task marked as done.");
    }

    public void DeleteTask(int index)
    {
        if (!IsIndexValid(index))
        {
            return;
        }
        tasks.RemoveAt(index);
        Console.WriteLine("Task deleted.");
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
                bool isDone = parts[0] == "true";
                string description = parts[1];
                tasks.Add(new ToDoItem(description) { IsDone = isDone });
            }
        }
    }
}