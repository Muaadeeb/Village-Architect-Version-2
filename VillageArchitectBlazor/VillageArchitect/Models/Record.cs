namespace VillageArchitect.Models;

public record Record(string Name, string Race, string Role, string Trait, string Sex, string Alignment, string? Secret, string Motivation);

public record MarketItem(string Name, string Price, string Availability, string Description); 
public record CombatStats(int Hp, int Ac, string Atk, string Dmg); 
public record Relationship(string TargetName, int Score, string Feeling, string Reason); 
public record DetailedNPC(string Name, string Race, string Role, string Trait, string Sex, string Alignment, string? Secret, string Motivation,
    string Personality, Relationship[] Relationships, string? PortraitUrl, string? AudioGreeting, CombatStats Stats) : Record(Name, Race, Role, Trait, Sex, Alignment, Secret, Motivation); 
public record Business(string Name, string Type, string Description, Record Owner, string[] NotableItems, MarketItem[] MarketItems, string Rumor, string EncounterHook, string GmNotes); 
public record Landmark(string Name, string Description, string EncounterHook); 
public record Quest(string Title, string Description, string Reward); 
public record Room(int Number, string Name, string Description, string Threats, string Treasure); 
public record PointOfInterest(string Title, string Type, string Location, string Background, Room[] Rooms); 
public record SettlementRelation(string SettlementName, string Type, string Status, string Description); 
public record Festival(string Name, string Season, string Timing, string Lore, string ModernPractice); 
public record DemographicEntry(string Race, int Count); 
public record VillageData(string Name, int Population, string Description, string Morale, DemographicEntry[] Demographics, string Geography,
    string Atmosphere, string Weather, string DarkSecret, Business[] Businesses, Landmark[] Landmarks, DetailedNPC[] Residents,
    SettlementRelation[] SettlementRelations, Festival[] Festivals, Quest[] MainQuests, Quest[] SideTreks, string[] CurrentEvents,
    string GmNotes, string? MapUrl, PointOfInterest? Poi);




