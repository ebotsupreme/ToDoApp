using ToDoApp.Config;

class Program
{
    static void Main(string[] args)
    {
        bool running = true;
        TaskManager taskManager = new(AppConfig.FilePath);

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("What woud you like to do?");
            Console.WriteLine("1. View tasks");
            Console.WriteLine("2. Add a task");
            Console.WriteLine("3. Mark task as done");
            Console.WriteLine("4. Delete a task");
            Console.WriteLine("5. Edit a task");
            Console.WriteLine("6. Exit");
            Console.Write("> ");

            string input = Console.ReadLine() ?? "";
            HandleMenuSelection(input, ref running, taskManager);
        }

        static void HandleMenuSelection(string input, ref bool running, TaskManager taskManager)
        {
            switch (input)
            {
                case "1":
                    // view tasks
                    ViewTasks(taskManager);
                    break;
                case "2":
                    // add tasks
                    AddTask(taskManager);
                    break;
                case "3":
                    // Mark task as done
                    CompleteTask(taskManager);
                    break;
                case "4":
                    // Delete a task
                    RemoveTask(taskManager);
                    break;
                case "5":
                    // Edit a task
                    EditTask(taskManager);
                    break;
                case "6":
                    // Exit
                    Console.WriteLine();
                    Console.Write("Exiting To-Do App...");
                    running = false;
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        static void ViewTasks(TaskManager taskManager)
        {
            taskManager.GetAllTasks();
        }

        static void AddTask(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.WriteLine("Enter a new task");

            string input = Console.ReadLine() ?? "";
            taskManager.CreateTask(input);
        }

        static void CompleteTask(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the number of the task to mark as done:");

            string input = Console.ReadLine() ?? "";
            bool success = int.TryParse(input, out int index);

            if (!CheckForValidNumber(success)) return;
            taskManager.MarkTaskAsDone(index - 1);
            
        }

        static void RemoveTask(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the number of the task to delete:");

            string input = Console.ReadLine() ?? "";
            bool success = int.TryParse(input, out int index);

            if (!CheckForValidNumber(success)) return;
            taskManager.DeleteTask(index - 1);
        }

        static void EditTask(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the number of the task you want to edit:");

            string input = Console.ReadLine() ?? "";
            bool success = int.TryParse(input, out int index);

            if (!CheckForValidNumber(success)) return;
            taskManager.UpdateTask(index - 1);
        }
        
        static bool CheckForValidNumber(bool success)
        {
            if (!success)
            {
                Console.WriteLine("Invalid number.");
                return false;
            }

            return true;
        }
    }
}