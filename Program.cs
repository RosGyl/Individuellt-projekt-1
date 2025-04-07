using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Task
{
    public string Title { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsDone { get; set; }
    public string Project { get; set; }

    public Task(string title, DateTime dueDate, string project)
    {
        Title = title;
        DueDate = dueDate;
        Project = project;
        IsDone = false;
    }

    public override string ToString()
    {
        return $"{Title} - {DueDate.ToString("yyyy-MM-dd")} - {Project} - {(IsDone ? "Done" : "Not Done")}";
    }
}

public class TaskManager
{
    private List<Task> tasks = new List<Task>();
    private string filePath = "tasks.txt";

    public List<Task> Tasks => tasks; 

   
    // Lägga till en uppgift
    public void AddTask(string title, DateTime dueDate, string project)
    {
        var newTask = new Task(title, dueDate, project);
        tasks.Add(newTask);
    }

    // Redigera en uppgift
    public void EditTask(int taskIndex, string title, DateTime dueDate, string project)
    {
        if (taskIndex >= 0 && taskIndex < tasks.Count)
        {
            tasks[taskIndex].Title = title;
            tasks[taskIndex].DueDate = dueDate;
            tasks[taskIndex].Project = project;
        }
        else
        {
            Console.WriteLine("Error: Task index is out of range.");
        }
    }

    // Markera en uppgift som klar
    public void MarkTaskAsDone(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < tasks.Count)
        {
            tasks[taskIndex].IsDone = true;
        }
        else
        {
            Console.WriteLine("Error: Task index is out of range.");
        }
    }

    // Ta bort en uppgift
    public void RemoveTask(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < tasks.Count)
        {
            tasks.RemoveAt(taskIndex);
            Console.WriteLine("Task removed.");
        }
        else
        {
            Console.WriteLine("Error: Task index is out of range.");
        }
    }

    // Visa alla uppgifter
    public void DisplayTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
        }
        else
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }
    }

    public void DisplayTasksSortedByDate()
    {
        var sortedTasks = tasks.OrderBy(t => t.DueDate).ToList();
        for (int i = 0; i < sortedTasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {sortedTasks[i]}");
        }
    }

    public void DisplayTasksSortedByProject()
    {
        var sortedTasks = tasks.OrderBy(t => t.Project).ToList();
        for (int i = 0; i < sortedTasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {sortedTasks[i]}");
        }
    }



    // Spara uppgifter till fil
    public void SaveTasksToFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Title},{task.DueDate},{task.IsDone},{task.Project}");
                }
            }
            Console.WriteLine("Tasks saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving tasks: {ex.Message}");
        }
    }

    // Ladda uppgifter från fil
    public void LoadTasksFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                tasks.Clear();
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        string title = parts[0];
                        DateTime dueDate = DateTime.Parse(parts[1]);
                        bool isDone = bool.Parse(parts[2]);
                        string project = parts[3];
                        tasks.Add(new Task(title, dueDate, project) { IsDone = isDone });
                    }
                }
            }
            else
            {
                Console.WriteLine("No task file found. Starting with an empty task list.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading tasks: {ex.Message}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var taskManager = new TaskManager();
        taskManager.LoadTasksFromFile();

        bool running = true;
        while (running)
        {
            ShowMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask(taskManager);
                    break;
                case "2":
                    EditTask(taskManager);
                    break;
                case "3":
                    MarkTaskAsDone(taskManager);
                    break;
                case "4":
                    RemoveTask(taskManager);
                    break;
                case "5":
                    taskManager.DisplayTasks();
                    break;
                case "6":
                    taskManager.DisplayTasksSortedByDate();
                    break;
                case "7":
                    taskManager.DisplayTasksSortedByProject();
                    break;
                case "8":
                    taskManager.SaveTasksToFile();
                    break;
                case "9":
                    Console.WriteLine("Goodbye!");
                    taskManager.SaveTasksToFile();
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\nTodo List Application");
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. Edit Task");
        Console.WriteLine("3. Mark Task as Done");
        Console.WriteLine("4. Remove Task");
        Console.WriteLine("5. Display Tasks");
        Console.WriteLine("6. Display Tasks Sorted by Date");
        Console.WriteLine("7. Display Tasks Sorted by Project");
        Console.WriteLine("8. Save Tasks");
        Console.WriteLine("9. Quit");
    }

    static void AddTask(TaskManager taskManager)
    {
        Console.Write("Enter task title: ");
        string title = Console.ReadLine();
        DateTime dueDate = GetValidDate();
        Console.Write("Enter project name: ");
        string project = Console.ReadLine();

        taskManager.AddTask(title, dueDate, project);
        Console.WriteLine("Task added.");
    }

    static void EditTask(TaskManager taskManager)
    {
        Console.Write("Enter task index to edit: ");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            Console.Write("Enter new title: ");
            string title = Console.ReadLine();
            DateTime dueDate = GetValidDate();
            Console.Write("Enter new project: ");
            string project = Console.ReadLine();

            taskManager.EditTask(index, title, dueDate, project);
        }
        else
        {
            Console.WriteLine("Invalid index.");
        }
    }

    static void MarkTaskAsDone(TaskManager taskManager)
    {
        Console.Write("Enter task index to mark as done: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= taskManager.Tasks.Count)
        {
            taskManager.MarkTaskAsDone(index - 1);
            Console.WriteLine("Task marked as done.");
        }
        else
        {
            Console.WriteLine("Invalid task index.");
        }
    }

    static void RemoveTask(TaskManager taskManager)
    {
        Console.Write("Enter task index to remove: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= taskManager.Tasks.Count)
        {
            taskManager.RemoveTask(index - 1);
        }
        else
        {
            Console.WriteLine("Invalid task index.");
        }
    }


    // Funktion för att få ett giltigt datum
    static DateTime GetValidDate()
    {
        DateTime dueDate;
        while (true)
        {
            Console.Write("Enter due date (yyyy-MM-dd): ");
            string input = Console.ReadLine();

            if (DateTime.TryParse(input, out dueDate))
            {
                return dueDate;
            }
            else
            {
                Console.WriteLine("Invalid date format, please try again.");
            }
        }
    }
}
