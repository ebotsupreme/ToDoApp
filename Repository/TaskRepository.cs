using ToDoApp.Model;
using ToDoApp.View;
using ToDoApp.Utils;
using ToDoApp.Shared.Interfaces;
using ToDoApp.Shared;

namespace ToDoApp.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly List<ToDoItem> tasks = [];
    private readonly string filePath;

    public TaskRepository(string path)
    {
        filePath = path;
        LoadFromFile(filePath);
    }

    public List<ToDoItem> GetAllTasks()
    {
        return tasks;
    }

    public List<ToDoItem> GetIncompleteTasks()
    {
        return [.. TaskFilter.FilterTasks(tasks, false)];
    }

    public List<ToDoItem> GetCompletedTasks()
    {
        return [.. TaskFilter.FilterTasks(tasks, true)];
    }

    public OperationResult CreateTask(string description)
    {
        try
        {
            tasks.Add(new ToDoItem(description));
            SaveToFile(filePath);
            return new OperationResult(true);    
        }
        catch (System.Exception ex)
        {
            return new OperationResult(false, $"Error adding task: {ex.Message}");
        }
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

    public OperationResult DeleteTask(int index)
    {
        try
        {
            tasks.RemoveAt(index);
            SaveToFile(filePath);
            return new OperationResult(true);
        }
        catch (System.Exception ex)
        {
            return new OperationResult(false, $"Error deleting task: {ex.Message}");
        }
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

    public static void GetInfoForTasks(IEnumerable<ToDoItem> tasks)
    {
        int i = 0;

        foreach (var task in tasks)
        {
            string status = task.IsDone ? "[X]" : "[ ]";
            Menu.PrintInfoForTasks(++i, status, task.Description);
        }
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