class TaskManager
{
    readonly List<string> tasks = [];

    public void GetAllTasks()
    {
        if (tasks.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Your tasks:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("No tasks found.");
        }
    }

    public void CreateTask(string task)
    {
        if (!string.IsNullOrWhiteSpace(task))
        {
            tasks.Add(task);
            Console.WriteLine();
            Console.WriteLine("Task added.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Cannot add an empty task.");
        }
    }
}