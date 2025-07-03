using ToDoApp.Config;
using ToDoApp.Controller;
using ToDoApp.View;
using ToDoApp.Repository;
using ToDoApp.Service;
using ToDoApp.Shared.Interfaces;

class App
{
    static void Main(string[] args)
    {
        bool running = true;
        TaskRepository taskRepository = new(AppConfig.FilePath);
        TaskService taskService = new(taskRepository);
        IMenu menu = new Menu();
        TaskController taskController = new(taskService, menu);

        while (running)
        {
            Menu.Show();
            string input = menu.UserInput();
            MenuHandler.HandleMenuSelection(input, ref running, taskController, menu);
        }
    }
}