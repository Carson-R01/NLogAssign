public class Character
{
    public UInt64 Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Species { get; set; }
    public string FirstAppearance { get; set; }
    public UInt64 YearCreated { get; set; }

    public Character(UInt64 id, string name, string? description, string species, string firstAppearance, UInt64 yearCreated)
    {
        Id = id;
        Name = name;
        Description = description;
        Species = species;
        FirstAppearance = firstAppearance;
        YearCreated = yearCreated;
    }

    public override string ToString()
    {
        return $"Id: {Id}\nName: {Name}\nDescription: {Description}\nSpecies: {Species}\nFirst Appearance: {FirstAppearance}\nYear Created: {YearCreated}\n";
    }
}