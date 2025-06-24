namespace ToDoApp.Model;

public class ToDoItem
{
    public string Description { get; set; }
    public bool IsDone { get; set; } = false;

    public ToDoItem(string description)
    {
        Description = description;
    }

    public override string ToString()
    {
        return $"{(IsDone ? "[X]" : [ ] )} {Description}"; 
    }
}