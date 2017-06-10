using Newtonsoft.Json;
using System.Collections.Generic;

namespace AMA_Card_Reader.Models
{
    public class Vehicle
    {
        [JsonProperty("make")]
        public string Name { get; set; }

        [JsonProperty("models")]
        public List<string> Models { get; set; }
    }

    public class VehicleList
    {
        [JsonProperty("vehicles")]
        public List<Vehicle> Vehicles { get; set; }
    }
}
