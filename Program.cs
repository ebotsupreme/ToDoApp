using System.Net;
using ToDoApp.Config;
using ToDoApp.Model;
using ToDoApp.View;

class Program
{
    static void Main(string[] args)
    {
        bool running = true;
        TaskManager taskManager = new(AppConfig.FilePath);

        while (running)
        {
            Menu.Show();
            string input = Menu.UserInput();
            HandleMenuSelection(input, ref running, taskManager);
        }

        static void HandleMenuSelection(string input, ref bool running, TaskManager taskManager)
        {
            switch (input)
            {
                case "1":
                    // view all tasks
                    ViewTasks(taskManager);
                    break;
                case "2":
                    // view incomplete tasks
                    ViewIncompleteTasks(taskManager);
                    break;
                case "3":
                    // view complete tasks
                    ViewCompletedTasks(taskManager);
                    break;
                case "4":
                    // add tasks
                    AddTask(taskManager);
                    break;
                case "5":
                    // Mark task as done
                    CompleteTask(taskManager);
                    break;
                case "6":
                    // Delete a task
                    RemoveTask(taskManager);
                    break;
                case "7":
                    // Edit a task
                    EditTask(taskManager);
                    break;
                case "8":
                    // Exit
                    Menu.PrintPrompt("Exiting To-Do App...");
                    running = false;
                    break;
                default:
                    Menu.PrintPrompt("Invalid option. Please try again.");
                    break;
            }
        }

        static void ViewTasks(TaskManager taskManager)
        {
            taskManager.GetAllTasks();
        }

        static void AddTask(TaskManager taskManager)
        {
            Menu.PrintPrompt("Enter a new task");

            string input = Menu.UserInput();
            bool isStringValidated = Menu.ValidateInput(input, "Cannot add an empty task.");

            if (isStringValidated) taskManager.CreateTask(input);
        }

        static void CompleteTask(TaskManager taskManager)
        {
            Menu.PrintPrompt("Enter the number of the task to mark as done:");

            int index = GetValidIndexWithRetry(taskManager);
            taskManager.MarkTaskAsDone(index);
        }

        static void RemoveTask(TaskManager taskManager)
        {
            Menu.PrintPrompt("Enter the number of the task to delete:");

            int index = GetValidIndexWithRetry(taskManager);
            taskManager.DeleteTask(index);
        }

        static void EditTask(TaskManager taskManager)
        {
            Menu.PrintPrompt("Enter the number of the task you want to edit:");

            int index = GetValidIndexWithRetry(taskManager);
            UpdateTaskDescription(taskManager, index);
        }

        static void UpdateTaskDescription(TaskManager taskManager, int index)
        {
            string currentTaskDescription = taskManager.GetTaskDescription(index);
            Menu.PrintUpdateTaskDescriptionPrompt(currentTaskDescription);

            string input = Menu.UserInput();
            bool isStringValidated = Menu.ValidateInput(input, "Cannot add an empty description.");

            if (isStringValidated) taskManager.UpdateTask(index, input);
        }

        static void ViewIncompleteTasks(TaskManager taskManager)
        {
            taskManager.GetIncompleteTasks();
        }

        static void ViewCompletedTasks(TaskManager taskManager)
        {
            taskManager.GetCompletedTasks();
        }

        static int GetValidIndexWithRetry(TaskManager taskManager)
        {
            while (true)
            { 
                string input = Menu.UserInput();
                bool success = int.TryParse(input, out int index);

                if (!success)
                {
                    Menu.PrintPrompt("Input is not a number. Please enter a valid task number.");
                    continue;
                }

                index -= 1;

                if (index < 0 || index >= taskManager.GetAllTasksCount())
                {
                    int maxTasks = taskManager.GetAllTasksCount();
                    Menu.ShowOutOfRangeMessage(maxTasks);
                    continue;
                }

                return index;    
            }
        }
    }
}