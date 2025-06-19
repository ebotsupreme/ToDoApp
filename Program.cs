using System;

class Program
{
    static void Main(string[] args)
    {
        List<string> tasks = new List<string>();
        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("What woud you like to do?");
            Console.WriteLine("1. View tasks");
            Console.WriteLine("2. Add a task");
            Console.WriteLine("3. Mark task as done");
            Console.WriteLine("4. Delete a task");
            Console.WriteLine("5. Exit");
            Console.Write("> ");

            string input = Console.ReadLine();
            HandleMenuSelection(input, tasks, ref running);
        }

        static void HandleMenuSelection(string input, List<string> tasks, ref bool running)
        {
            switch (input)
            {
                case "1": // view tasks
                    ViewTasks(tasks);
                    break;
                // case "2": // add tasks
                //     break;
                // case "3": // Mark task as done
                //     break;
                // case "4": // Delete a task
                //     break;
                case "5": // Exit
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
        
        static void ViewTasks(List<string> tasks)
        {
            if (tasks.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Your tasks:");
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("No tasks found.");
            }
        }
    }
}