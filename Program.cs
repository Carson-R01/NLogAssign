﻿using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";
// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
logger.Info("Program started");
string file = "mario.csv";
// make sure movie file exists
if (!File.Exists(file))
{
    logger.Error("File does not exist: {File}", file);
}
else
{
    List<UInt64> Ids = [];
    List<string> Names = [];
    List<string> Descriptions = [];
    List<string> Species = [];
    List<string> FirstAppearance = [];
    List<UInt64> YearCreated = [];
    // to populate the lists with data, read from the data file
    try
    {
        StreamReader sr = new(file);
        // first line contains column headers
        sr.ReadLine();
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line is not null)
            {
                // character details are separated with comma(,)
                string[] characterDetails = line.Split(',');
                // 1st array element contains id
                Ids.Add(UInt64.Parse(characterDetails[0]));
                // 2nd array element contains character name
                Names.Add(characterDetails[1]);
                // 3rd array element contains character description
                Descriptions.Add(characterDetails[2]);
                // 4th array element contains character species
                Species.Add(characterDetails[3]);
                // 5th array element contains characters First Appearance
                FirstAppearance.Add(characterDetails[4]);
                // 6th array element contains the year the character was created
                YearCreated.Add(UInt64.Parse(characterDetails[5]));
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
        // display choices to user
        Console.WriteLine("1) Add Character");
        Console.WriteLine("2) Display All Characters");
        Console.WriteLine("Enter to quit");
        // input selection
        choice = Console.ReadLine();
        logger.Info("User choice: {Choice}", choice);
        if (choice == "1")
        {
            // Add Character
            Console.WriteLine("Enter new character name: ");
            string? Name = Console.ReadLine();
            if (!string.IsNullOrEmpty(Name)){
                // generate id - use max value in Ids + 1
                UInt64 Id = Ids.Max() + 1;
                Console.WriteLine($"{Id}, {Name}");
            } else {
                logger.Error("You must enter a name");
            }
        }
        else if (choice == "2")
        {
            // Display All Characters
            // loop thru Lists
            for (int i = 0; i < Ids.Count; i++)
            {
                // display character details
                Console.WriteLine($"Id: {Ids[i]}");
                Console.WriteLine($"Name: {Names[i]}");
                Console.WriteLine($"Description: {Descriptions[i]}");
                Console.WriteLine($"Species:  {Species[i]}");
                Console.WriteLine($"First Appearance:  {FirstAppearance[i]}");
                Console.WriteLine($"Year Created:  {YearCreated[i]}");
                Console.WriteLine("");
            }
        }
    } while (choice == "1" || choice == "2");
}
logger.Info("Program ended");