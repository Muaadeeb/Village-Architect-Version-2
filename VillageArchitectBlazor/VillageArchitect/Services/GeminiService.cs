using Google.GenAI; // Confirm namespace for your package
using Google.GenAI.Types;
using Microsoft.Extensions.Configuration; // For API key
using System.Text.Json;
using VillageArchitect.Models; // Your namespace

namespace VillageArchitect.Services; // Your namespace

public class GeminiService
{
    private readonly Client _client; // Use Client as per SDK

    public GeminiService(IConfiguration config)
    {
        var apiKey = config.GetValue<string>("Gemini:ApiKey") ?? throw new InvalidOperationException("Gemini API key not configured.");
        _client = new Client(apiKey: apiKey); // Create Client with API key (for Google AI)
        // If using Vertex AI: _client = new Client(project: "your-project", location: "your-location", vertexAI: true);
    }

    public async Task<VillageData> GenerateVillageDetails(string villageName, int popCount)
    {
        var goodPart = $"""
                Generate a massive, detailed fantasy village dossier for the Shadowdark RPG. 
                Name: {villageName}
                Population: {popCount}
                Atmosphere: Gritty, dark, low-magic, old-school feel.REQUIRED DATA:
            1. Geography: Moody description of the site by a river.
            2. Description: Elaborate on the general mood and prevalent dangers of the village (Shadowdark style).
            3. Dark Secret: The village's core rot or hidden horror.
            4. Morale: A single metric ('Hopeful', 'Fearful', 'Resentful', 'Apathetic', 'Defiant').
            5. Weather: A single short thematic phrase.
            6. Demographics: Break down the population into 4-6 distinct racial groups totaling {popCount}.
            7. Exactly 4 Nearby Settlement Relations: 
               - Focus on resource scarcity, border skirmishes, and espionage.
               - Each entry: settlementName, type ('Good', 'Neutral', 'Harmful'), status, description.
            8. Exactly 6-8 Festivals:
               - name, season ('Spring', 'Summer', 'Fall', 'Winter', 'Major'), timing, lore (dark origins), and modernPractice.
            9. Exactly 12 Businesses: Gritty names, rumors, encounterHooks, gmNotes, and exactly 5 marketItems each.
            10. Two major landmarks.
            11. Exactly 15 NPCs: 
               - Attributes: name, race, sex, role, alignment ('Lawful', 'Neutral', 'Chaotic').
               - Psychology: personality, motivation, trait (Characteristic), secret (Alignment Shadow Secret).
               - Stats: hp, ac.
               - IMPORTANT: Relationship matrix for ALL other 14 NPCs. Scores 1-10 (Varied mix).
            12. Main Quests (4) and Side Treks (10).
            13. Current Events (3).
            14. GM Notes.

            Output JSON schema:
            """;


        var jsonPart1 = $$$"""
            {{
              "geography": "string",
              "description": "string",
              "atmosphere": "string",
              "morale": "Hopeful|Fearful|Resentful|Apathetic|Defiant",
              "weather": "string",
              "darkSecret": "string",
              "demographics": [
                {{ "race": "string", "count": number }}
              ],
              "settlementRelations": [
                {{ "settlementName": "string", "type": "Good|Neutral|Harmful", "status": "string", "description": "string" }}
              ],
              "festivals": [
                {{ "name": "string", "season": "Spring|Summer|Fall|Winter|Major", "timing": "string", "lore": "string", "modernPractice": "string" }}
              ],
              "landmarks": [ {{ "name": "string", "description": "string", "encounterHook": "string" }} ],
              "gmNotes": "string",
              "currentEvents": ["string", "string", "string"],
              "businesses": [
            """;

        var jsonPart2 = $$$"""
        {{
          "name": "string", "type": "string", "description": "string", "rumor": "string", "encounterHook": "string", "gmNotes": "string",
          "marketItems": [ {{ "name": "string", "price": "string", "availability": "Common|Rare|Scarce", "description": "string" }} ],
          "owner": {{ "name": "string", "race": "string", "sex": "Male|Female", "role": "string", "trait": "string", "alignment": "Lawful|Neutral|Chaotic", "motivation": "string", "secret": "string" }} 
        }}
          ],
          "residents": [
            {{
              "name": "string", "race": "string", "sex": "Male|Female", "role": "string", "personality": "string", "trait": "string", "alignment": "Lawful|Neutral|Chaotic", "motivation": "string", "secret": "string",
              "stats": {{ "hp": number, "ac": number, "atk": "string", "dmg": "string" }},
              "relationships": [ {{ "targetName": "string", "score": number, "feeling": "string", "reason": "string" }} ]
            }}
          ],
          "mainQuests": [ {{ "title": "string", "description": "string", "reward": "string" }} ],
          "sideTreks": [ {{ "title": "string", "description": "string", "reward": "string" }} ]
        }}
        """; 
        
        var prompt = goodPart + jsonPart1 + jsonPart2;


        // Configure for JSON response
        var config = new GenerateContentConfig
        {
            ResponseMimeType = "application/json",
            // Add other configs if needed, e.g., Temperature = 0.5 for creativity
        };

        var response = await _client.Models.GenerateContentAsync(
            model: "gemini-1.5-pro-latest", // Or "gemini-1.5-flash" for faster responses
            contents: prompt,
            config: config
        );

        // Safely extract JSON text, handling possible nulls to avoid CS8602
        string rawJson = response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "{}";

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; // Flexible parsing
        return JsonSerializer.Deserialize<VillageData>(rawJson, options) ?? new VillageData(villageName, popCount, string.Empty, string.Empty, Array.Empty<DemographicEntry>(), string.Empty, string.Empty, string.Empty, string.Empty, Array.Empty<Business>(), Array.Empty<Landmark>(), Array.Empty<DetailedNPC>(), Array.Empty<SettlementRelation>(), Array.Empty<Festival>(), Array.Empty<Quest>(), Array.Empty<Quest>(), Array.Empty<string>(), string.Empty, null, null);
    }

    // Placeholder for other methods (adapt similarly)
    public async Task<PointOfInterest> GeneratePOI(VillageData village)
    {
        // Similar structure: Build prompt, call _client.Models.GenerateContentAsync
        throw new NotImplementedException();
    }

    public async Task<string> GenerateVillageMap(VillageData village)
    {
        var prompt = $"Generate a base64-encoded image of a fantasy village map for {village.Name}. Description: {village.Description}.";

        var config = new GenerateImagesConfig
        {
            NumberOfImages = 1,
            OutputMimeType = "image/png" // Or jpeg
        };

        var response = await _client.Models.GenerateImagesAsync(
            model: "imagen-3.0-generate-002", // Image model
            prompt: prompt,
            config: config
        );

        // Safely handle possible nulls to avoid CS8604 and CS8602
        var imageBytes = response?.GeneratedImages?.FirstOrDefault()?.Image?.ImageBytes;
        if (imageBytes == null)
        {
            throw new InvalidOperationException("No image was generated by the Gemini API.");
        }
        return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}"; // Return base64 URL for display
    }

    // Add more methods as needed
}
