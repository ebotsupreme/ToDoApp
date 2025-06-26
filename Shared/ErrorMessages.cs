namespace ToDoApp.Shared;
public static class ErrorMessages
{
    public const string TaskNotFound = "Task not found.";
    public const string EmptyDescription = "Task description cannot be empty.";
    public const string DuplicateDescription = "A task with this description already exists.";
    public const string NoTasks = "No tasks found.";
    public const string NoIncompleteTasks = "No incomplete tasks found.";
    public const string NoCompletedTasks = "No completed tasks found.";
    public const string TaskAlreadyDone = "That task is already marked as done.";
    public const string EmptyTask = "Cannot add an empty task.";
    public const string NotANumber = "Input is not a number. Please enter a valid task number.";
    public const string ErrorOccurred = "An error occured.";
    public const string TaskOutOfRangeFormat = "Selected task number is out of range. Please enter a number between 1 and {0}.";
    public const string LoadingErrorFormat = "Error loading tasks: {0}";
    public const string SavingErrorFormat = "Could not save tasks: {0} - {1}";
    public const string RepositoryAddErrorFormat = "Error adding task: {0}";
    public const string RepositoryDeleteErrorFormat = "Error deleting task: {0}";
    public const string RepositoryCompleteErrorFormat = "Error completing task: {0}";
    public const string RepositoryUpdateErrorFormat = "Error updating task: {0}";
    public const string RepositorySaveErrorFormat = "Error saving tasks: {0}";
}
