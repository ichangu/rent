using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text; // This fixes your Encoding error
using System.Text.Json; // This fixes your JsonSerializer error
using System.Threading.Tasks;
using System.Collections.Generic;
using PropertyManagerApp.Models; // Ensure this matches your namespace

namespace PropertyManagerApp.Services
{
    public class DatabaseService
    {
        private readonly HttpClient _http;
        // PASTE YOUR SCRIPT URL HERE
        private const string ApiUrl = "https://script.google.com/macros/s/AKfycby9ASO0DJTig0mANePXIrlic65W4q2DHSTSEijDtP2gZquun99HNWSgeoGLMf3wQM1xyA/exec"; 

        public DatabaseService(HttpClient http)
        {
            _http = http;
        }

        public async Task<SheetsDatabasePayload?> FetchEverythingAsync()
{
    try
    {
        var response = await _http.GetAsync(ApiUrl);
        if (response.IsSuccessStatusCode)
        {
            var rawJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RAW DATA FROM SHEETS: " + rawJson); // CHECK YOUR BROWSER CONSOLE!
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await response.Content.ReadFromJsonAsync<SheetsDatabasePayload>(options);
        }
        else
        {
            Console.WriteLine("Failed to fetch. Status: " + response.StatusCode);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching data: {ex.Message}");
    }
    return null;
}

        public async Task<bool> SaveRecordAsync(string action, string tableName, object dataObject)
        {
            Console.WriteLine("DEBUG: SaveRecordAsync was called!");
            try
            {
                var payload = new
                {
                    action = action,
                    table = tableName,
                    data = dataObject
                };

                // Serialize to JSON string
                var json = JsonSerializer.Serialize(payload);

                // Use "text/plain" to bypass the CORS preflight check
                // Encoding.UTF8 is now available because of the 'using System.Text;' at the top
                var content = new StringContent(json, Encoding.UTF8, "text/plain");

                // Post using PostAsync
                var response = await _http.PostAsync(ApiUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonString);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("success", out var successProp))
                    {
                        return successProp.GetBoolean();
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving record to Google Sheets: {ex.Message}");
                return false;
            }
        }
    }
}