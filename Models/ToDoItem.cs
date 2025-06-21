class ToDoItem
{
    public required string Description { get; set; }
    public bool IsDone { get; set; } = false;

    public ToDoItem(string description)
    {
        Description = description;
    }
}