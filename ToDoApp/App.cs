using ToDoApp.Config;
using ToDoApp.Controller;
using ToDoApp.View;
using ToDoApp.Repository;
using ToDoApp.Service;

class App
{
    static void Main(string[] args)
    {
        bool running = true;
        TaskRepository taskRepository = new(AppConfig.FilePath);
        TaskService taskService = new(taskRepository);
        TaskController taskController = new(taskService);

        while (running)
        {
            Menu.Show();
            string input = Menu.UserInput();
            MenuHandler.HandleMenuSelection(input, ref running, taskController);
        }
    }
}