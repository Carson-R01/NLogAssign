using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string path = Directory.GetCurrentDirectory() + "//nlog.config";
        var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
        logger.Info("Program started");

        string file = "mario.csv";
        List<Character> characters = new List<Character>();

        // Load characters from file
        if (File.Exists(file))
        {
            using StreamReader sr = new(file);
            sr.ReadLine(); // Skip header

            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                if (line is not null)
                {
                    string[] details = line.Split(',');

                    characters.Add(new Character(
                        UInt64.Parse(details[0]),
                        details[1],
                        details[2],
                        details[3],
                        details[4],
                        UInt64.Parse(details[5])
                    ));
                }
            }
        }
        else
        {
            logger.Error("File does not exist: {File}", file);
        }

        string? choice;
        do
        {
            Console.WriteLine("\n1) Add Character");
            Console.WriteLine("2) Display All Characters");
            Console.WriteLine("Enter to quit");

            choice = Console.ReadLine();
            logger.Info("User choice: {Choice}", choice);

            if (choice == "1")
            {
                Console.Write("Enter new character name: ");
                string? name = Console.ReadLine();

                if (!string.IsNullOrEmpty(name))
                {
                    if (characters.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    {
                        logger.Info($"Duplicate name {name}");
                    }
                    else
                    {
                        UInt64 id = characters.Count > 0 ? characters.Max(c => c.Id) + 1 : 1;

                        Console.Write("Enter description: ");
                        string? description = Console.ReadLine();
                        Console.Write("Enter species: ");
                        string? species = Console.ReadLine();
                        Console.Write("Enter first appearance: ");
                        string? firstAppearance = Console.ReadLine();

                        Console.Write("Enter the year the character was created: ");
                        string? yearCreatedInput = Console.ReadLine();
                        if (UInt64.TryParse(yearCreatedInput, out UInt64 yearCreated))
                        {
                            var character = new Character(id, name, description, species, firstAppearance, yearCreated);
                            characters.Add(character);

                            using StreamWriter sw = File.AppendText(file);
                            sw.WriteLine($"{id},{name},{description},{species},{firstAppearance},{yearCreated}");

                            Console.WriteLine("Character added successfully!");
                            logger.Info($"Added character: {id}, {name}");
                        }
                        else
                        {
                            logger.Error("Invalid year input. Must be a number.");
                        }
                    }
                }
                else
                {
                    logger.Error("You must enter a name.");
                }
            }
            else if (choice == "2")
            {
                characters.ForEach(c => Console.WriteLine(c));
            }
        } while (choice == "1" || choice == "2");

        logger.Info("Program ended.");
    }
}