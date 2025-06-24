using ToDoApp.Controller;
using ToDoApp.View;

class MenuHandler
{
    public static void HandleMenuSelection(string input, ref bool running, TaskController taskController)
    {
        switch (input)
        {
            case "1":
                taskController.ViewAllTasks();
                break;
            case "2":
                taskController.ViewIncompleteTasks();
                break;
            case "3":
                taskController.ViewCompletedTasks();
                break;
            case "4":
                taskController.AddTask();
                break;
            case "5":
                taskController.CompleteTask();
                break;
            case "6":
                taskController.RemoveTask();
                break;
            case "7":
                taskController.EditTask();
                break;
            case "8":
                Menu.PrintPrompt("Exiting To-Do App...");
                running = false;
                break;
            default:
                Menu.PrintPrompt("Invalid option. Please try again.");
                break;
        }
    }
}