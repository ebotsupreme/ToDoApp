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
            Console.WriteLine("1. View all tasks");
            Console.WriteLine("2. View incomplete tasks");
            Console.WriteLine("3. View completed tasks");
            Console.WriteLine("4. Add a task");
            Console.WriteLine("5. Mark task as done");
            Console.WriteLine("6. Delete a task");
            Console.WriteLine("7. Edit a task");
            Console.WriteLine("8. Exit");
            Console.Write("> ");

            string input = Console.ReadLine() ?? "";
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
            bool isStringValidated = ValidateStringInput(input, "Cannot add an empty task.");
            if (isStringValidated) taskManager.CreateTask(input);
        }

        static void CompleteTask(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the number of the task to mark as done:");

            int? index = GetValidIndex(taskManager);

            if (index is int i)
            {
                taskManager.MarkTaskAsDone(i);
            }
        }

        static void RemoveTask(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the number of the task to delete:");

            int? index = GetValidIndex(taskManager);

            if (index is int i)
            {
                taskManager.DeleteTask(i);
            }
        }

        static void EditTask(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the number of the task you want to edit:");

            int? index = GetValidIndex(taskManager);

            if (index is int i)
            {
                UpdateTaskDescription(taskManager, i);
            }
        }

        static void UpdateTaskDescription(TaskManager taskManager, int index)
        {
            Console.WriteLine();
            Console.WriteLine("Current task description: " + taskManager.GetTaskDescription(index));
            Console.WriteLine("Enter an updated description: ");

            string input = Console.ReadLine() ?? "";
            bool isStringValidated = ValidateStringInput(input, "Cannot add an empty description.");
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

        static int? GetValidIndex(TaskManager taskManager)
        {
            string input = Console.ReadLine() ?? "";
            bool success = int.TryParse(input, out int index);

            if (!success)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid task number.");
                return null;
            }

            index -= 1;

            if (index < 0 || index >= taskManager.GetAllTasksCount())
            {
                return null;
            }

            return index;
        }
        
        static bool ValidateStringInput(string input, string message)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(message);
                return false;
            }

            return true;
        }
    }
}