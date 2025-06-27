namespace ToDoApp.Model;

public class ToDoItem(Guid id, string description)
{
    public Guid Id { get; set; } = id;
    public string Description { get; set; } = description;
    public bool IsDone { get; set; } = false;

    public override string ToString()
    {
        return $"{(IsDone ? "[X]" : [ ] )} {Description}"; 
    }
}