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

    public void MarkTaskAsDone(int index)
    {
        if (index < 0 || index >= tasks.Count)
        {
            Console.Write("Invalid task number.");
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
}