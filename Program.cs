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

    public void AddTask(string title, DateTime dueDate, string project)
    {
        var newTask = new Task(title, dueDate, project);
        tasks.Add(newTask);
        Console.WriteLine("Task added successfully!");
    }

    public void EditTask(int taskIndex, string title, DateTime dueDate, string project)
    {
        if (taskIndex >= 0 && taskIndex < tasks.Count)
        {
            tasks[taskIndex].Title = title;
            tasks[taskIndex].DueDate = dueDate;
            tasks[taskIndex].Project = project;
            Console.WriteLine("Task updated successfully!");
        }
        else
        {
            Console.WriteLine("Invalid task index.");
        }
    }

    public void MarkTaskAsDone(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < tasks.Count)
        {
            tasks[taskIndex].IsDone = true;
            Console.WriteLine("Task marked as done.");
        }
        else
        {
            Console.WriteLine("Invalid task index.");
        }
    }

    public void RemoveTask(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < tasks.Count)
        {
            tasks.RemoveAt(taskIndex);
            Console.WriteLine("Task removed.");
        }
        else
        {
            Console.WriteLine("Invalid task index.");
        }
    }

    public void DisplayTasks()
    {
        Console.WriteLine("All Tasks:");
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    public void DisplayTasksSortedByDate()
    {
        var sortedTasks = tasks.OrderBy(t => t.DueDate).ToList();
        Console.WriteLine("Tasks Sorted by Due Date:");
        foreach (var task in sortedTasks)
        {
            Console.WriteLine(task);
        }
    }

    public void DisplayTasksSortedByProject()
    {
        var sortedTasks = tasks.OrderBy(t => t.Project).ToList();
        Console.WriteLine("Tasks Sorted by Project:");
        foreach (var task in sortedTasks)
        {
            Console.WriteLine(task);
        }
    }

    public void SaveTasksToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var task in tasks)
            {
                writer.WriteLine($"{task.Title},{task.DueDate},{task.IsDone},{task.Project}");
            }
        }
        Console.WriteLine("Tasks saved to file.");
    }

    public void LoadTasksFromFile()
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
            Console.WriteLine("Tasks loaded from file.");
        }
        else
        {
            Console.WriteLine("No previous tasks found. Starting fresh.");
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

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter task title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter task due date (yyyy-mm-dd): ");
                    DateTime dueDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Enter project: ");
                    string project = Console.ReadLine();
                    taskManager.AddTask(title, dueDate, project);
                    break;

                case "2":
                    taskManager.DisplayTasks();
                    Console.Write("Enter task number to edit: ");
                    int editIndex = int.Parse(Console.ReadLine()) - 1;
                    Console.Write("Enter new title: ");
                    string newTitle = Console.ReadLine();
                    Console.Write("Enter new due date (yyyy-mm-dd): ");
                    DateTime newDueDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Enter new project: ");
                    string newProject = Console.ReadLine();
                    taskManager.EditTask(editIndex, newTitle, newDueDate, newProject);
                    break;

                case "3":
                    taskManager.DisplayTasks();
                    Console.Write("Enter task number to mark as done: ");
                    int doneIndex = int.Parse(Console.ReadLine()) - 1;
                    taskManager.MarkTaskAsDone(doneIndex);
                    break;

                case "4":
                    taskManager.DisplayTasks();
                    Console.Write("Enter task number to remove: ");
                    int removeIndex = int.Parse(Console.ReadLine()) - 1;
                    taskManager.RemoveTask(removeIndex);
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
                    taskManager.SaveTasksToFile();
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }
}
