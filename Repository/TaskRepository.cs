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

    public Result<ToDoItem> CreateTask(string description)
    {
        try
        {
            var newTask = new ToDoItem(Guid.NewGuid(), description);
            tasks.Add(newTask);
            SaveToFile(filePath);
            return Result<ToDoItem>.Ok(newTask);
        }
        catch (System.Exception ex)
        {
            return Result<ToDoItem>.Fail(string.Format(ErrorMessages.RepositoryAddErrorFormat, ex.Message));
        }
    }

    public Result<ToDoItem> MarkTaskAsDone(ToDoItem task)
    {
        try
        {
            task.IsDone = true;
            SaveToFile(filePath);
            return Result<ToDoItem>.Ok(task);
        }
        catch (System.Exception ex)
        {
            return Result<ToDoItem>.Fail(string.Format(ErrorMessages.RepositoryCompleteErrorFormat, ex.Message));
        }
    }

    public Result<Unit> DeleteTask(ToDoItem task)
    {
        try
        {
            tasks.Remove(task);
            SaveToFile(filePath);
            return Result<Unit>.Ok(Unit.Value);
        }
        catch (System.Exception ex)
        {
            return Result<Unit>.Fail(string.Format(ErrorMessages.RepositoryDeleteErrorFormat, ex.Message));
        }
    }

    public Result<ToDoItem> UpdateTask(ToDoItem task, string input)
    {
        try
        {
            task.Description = input;
            SaveToFile(filePath);
            return Result<ToDoItem>.Ok(task);
        }
        catch (System.Exception ex)
        {
            return Result<ToDoItem>.Fail(string.Format(ErrorMessages.RepositoryUpdateErrorFormat, ex.Message));
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

                if (parts.Length == 3)
                {
                    if (!Guid.TryParse(parts[0], out Guid id))
                    {
                        Menu.PrintPrompt($"Invalid GUID format in task data: {parts[0]} - skipping this entry.");
                        continue;
                    }    
                    bool isDone = parts[1].Equals("true", StringComparison.OrdinalIgnoreCase);
                    string description = parts[2];
                    tasks.Add(new ToDoItem(id, description) { IsDone = isDone });
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
                var line = $"{task.Id}|{task.IsDone}|{task.Description}";
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
}