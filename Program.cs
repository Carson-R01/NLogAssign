using NLog;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

string path = Directory.GetCurrentDirectory() + "//nlog.config";
// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
logger.Info("Program started");

string file = "mario.csv";

// Make sure file exists
if (!File.Exists(file))
{
    logger.Error("File does not exist: {File}", file);
}
else
{
    List<UInt64> Ids = [];
    List<string> Names = [];
    List<string?> Descriptions = [];
    List<string> SpeciesList = [];  // Renamed from "Species" to avoid conflict
    List<string> FirstAppearanceList = [];  // Renamed from "FirstAppearance" to avoid conflict
    List<UInt64> YearCreatedList = [];  // Renamed from "YearCreated" to avoid conflict

    // Read file and populate lists
    try
    {
        StreamReader sr = new(file);
        sr.ReadLine(); // Skip header

        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line is not null)
            {
                string[] characterDetails = line.Split(',');

                Ids.Add(UInt64.Parse(characterDetails[0]));
                Names.Add(characterDetails[1]);
                Descriptions.Add(characterDetails[2]);
                SpeciesList.Add(characterDetails[3]);  // Use renamed variable
                FirstAppearanceList.Add(characterDetails[4]);  // Use renamed variable
                YearCreatedList.Add(UInt64.Parse(characterDetails[5]));  // Use renamed variable
            }
        }
        sr.Close();
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
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
                List<string> lowerCaseNames = Names.ConvertAll(n => n.ToLower());
                if (lowerCaseNames.Contains(name.ToLower()))
                {
                    logger.Info($"Duplicate name {name}");
                }
                else
                {
                    UInt64 id = Ids.Max() + 1;

                    Console.Write("Enter description: ");
                    string? description = Console.ReadLine();
                    Console.Write("Enter species: ");
                    string? species = Console.ReadLine();
                    Console.Write("Enter first appearance: ");
                    string? firstAppearance = Console.ReadLine();
                    
                    Console.Write("Enter the year the character was created: ");
                    string? yearCreatedInput = Console.ReadLine();
                    UInt64 yearCreated;
                    
                    if (UInt64.TryParse(yearCreatedInput, out yearCreated))
                    {
                        Ids.Add(id);
                        Names.Add(name);
                        Descriptions.Add(description);
                        SpeciesList.Add(species);
                        FirstAppearanceList.Add(firstAppearance);
                        YearCreatedList.Add(yearCreated);

                        using (StreamWriter sw = File.AppendText(file))
                        {
                            sw.WriteLine($"{id},{name},{description},{species},{firstAppearance},{yearCreated}");
                        }

                        Console.WriteLine("Character added successfully!");
                        logger.Info($"Added character: {id}, {name}, {description}, {species}, {firstAppearance}, {yearCreated}");
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
            // Display all characters
            for (int i = 0; i < Ids.Count; i++)
            {
                Console.WriteLine($"Id: {Ids[i]}");
                Console.WriteLine($"Name: {Names[i]}");
                Console.WriteLine($"Description: {Descriptions[i]}");
                Console.WriteLine($"Species: {SpeciesList[i]}");  // Use renamed variable
                Console.WriteLine($"First Appearance: {FirstAppearanceList[i]}");  // Use renamed variable
                Console.WriteLine($"Year Created: {YearCreatedList[i]}");  // Use renamed variable
                Console.WriteLine("");
            }
        }
    } while (choice == "1" || choice == "2");
}

logger.Info("Program ended.");