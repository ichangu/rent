using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PropertyManagerApp.Models
{

    // This converter handles both numbers and strings in JSON
    public class FlexibleStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Handle Number types (like 101)
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64().ToString();
            }

            // Handle Nulls (common in empty spreadsheet cells)
            if (reader.TokenType == JsonTokenType.Null)
            {
                return string.Empty;
            }

            // Handle Strings
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString() ?? string.Empty;
            }

            // Catch-all: If it's something weird (like an object or array),
            // this parses it into a string rather than crashing.
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                return doc.RootElement.ToString();
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
    
    public class SheetsDatabasePayload
    {
        [JsonPropertyName("properties")]
        public List<PropertyRecord> Properties { get; set; } = new();

        [JsonPropertyName("rooms")]
        public List<RoomRecord> Rooms { get; set; } = new();

        [JsonPropertyName("renters")]
        public List<RenterRecord> Renters { get; set; } = new();

        [JsonPropertyName("leases")]
        public List<LeaseRecord> Leases { get; set; } = new();

        [JsonPropertyName("utilities")]
        public List<UtilityRecord> Utilities { get; set; } = new();

        [JsonPropertyName("invoices")]
        public List<InvoiceRecord> Invoices { get; set; } = new();

        [JsonPropertyName("payments")]
        public List<PaymentRecord> Payments { get; set; } = new();

        [JsonPropertyName("expenses")]
        public List<ExpenseRecord> Expenses { get; set; } = new();

        [JsonPropertyName("maintenance_requests")]
        public List<MaintenanceRecord> MaintenanceRequests { get; set; } = new();

        [JsonPropertyName("global_lookups")]
        public List<LookupRecord> GlobalLookups { get; set; } = new();
    }

    public class RenterRecord
    {
        [JsonPropertyName("RenterID")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string RenterId { get; set; } = string.Empty;

        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("LastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("Phone")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("Email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("EmergencyContact")]
        public string EmergencyContact { get; set; } = string.Empty;
    }

    public class PropertyRecord
    {
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Id { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public class RoomRecord { 
        [JsonPropertyName("RoomID")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string RoomID { get; set; } = ""; 
        
        }
    public class LeaseRecord { 
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string LeaseID { get; set; } = ""; 
        
        }
    public class UtilityRecord { 
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string UtilityID { get; set; } = ""; 
        
        }
    public class InvoiceRecord { 
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string InvoiceID { get; set; } = ""; 
        
        }
    public class PaymentRecord { 
        
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string PaymentID { get; set; } = ""; 
        
        }
    public class ExpenseRecord { 
        
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string ExpenseID { get; set; } = ""; 
        }
    public class MaintenanceRecord { 
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string MaintenanceID { get; set; } = ""; }
    public class LookupRecord { 
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string LookupID { get; set; } = ""; }
}