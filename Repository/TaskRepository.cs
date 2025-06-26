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

    public IReadOnlyList<ToDoItem> GetAllTasks()
    {
        return tasks;
    }

    public IReadOnlyList<ToDoItem> GetIncompleteTasks()
    {
        return [.. TaskFilter.FilterTasks(tasks, false)];
    }

    public IReadOnlyList<ToDoItem> GetCompletedTasks()
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

    public OperationResult MarkTaskAsDone(ToDoItem task)
    {
        try
        {
            task.IsDone = true;
            SaveToFile(filePath);
            return new OperationResult(true);
        }
        catch (System.Exception ex)
        {
            return new OperationResult(false, $"Error completing task: {ex.Message}");
        }
    }

    public OperationResult DeleteTask(ToDoItem task)
    {
        try
        {
            tasks.Remove(task);
            SaveToFile(filePath);
            return new OperationResult(true);
        }
        catch (System.Exception ex)
        {
            return new OperationResult(false, $"Error deleting task: {ex.Message}");
        }
    }

    public OperationResult UpdateTask(ToDoItem task, string input)
    {
        try
        {
            task.Description = input;
            SaveToFile(filePath);
            return new OperationResult(true);
        }
        catch (System.Exception ex)
        {
            return new OperationResult(false, $"Error updating task: {ex.Message}");
        }
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

    public int GetAllTasksCount()
    {
        return tasks.Count;
    }

    public ToDoItem? GetTaskByIndex(int index)
    {
        if (index < 0 || index >= tasks.Count)
        {
            return null;
        }

        return tasks[index];
    }
}