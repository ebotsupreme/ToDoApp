using ToDoApp.Config;
using ToDoApp.Controller;
using ToDoApp.Model;
using ToDoApp.View;

class App
{
    static void Main(string[] args)
    {
        bool running = true;
        TaskManager taskManager = new(AppConfig.FilePath);
        TaskController taskController = new(taskManager);

        while (running)
        {
            Menu.Show();
            string input = Menu.UserInput();
            MenuHandler.HandleMenuSelection(input, ref running, taskController);
        }
    }
}