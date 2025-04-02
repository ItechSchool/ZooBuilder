using System;

namespace SharedNetwork.Dtos
{
    [Serializable]
    public struct BuildingDto : IStringSerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int SizeWidth { get; set; }
        public int SizeHeight { get; set; }
        public int Costs { get; set; }
        public int Capacity { get; set; }
        public int MaxRevenue { get; set; }
        public int? AnimalId { get; set; }

        public void FromString(string dataString)
        {
            string cleanedString = dataString.Replace("{", string.Empty).Replace("}", string.Empty);
            string[] values = cleanedString.Split(';');

            Id = int.Parse(values[0]);
            Name = values[1];
            Type = values[2];
            SizeWidth = int.Parse(values[3]);
            SizeHeight = int.Parse(values[4]);
            Costs = int.Parse(values[5]);
            Capacity = int.Parse(values[6]);
            MaxRevenue = int.Parse(values[7]);
            AnimalId = values[8] != "null" ? int.Parse(values[8]) : (int?)null;
        }

        public override string ToString()
        {
            return "{" + Id + ";" + Name + ";" + Type + ";" +
                   SizeWidth + ";" + SizeHeight + ";" + Costs + ";" +
                   Capacity + ";" + MaxRevenue + ";" +
                   (AnimalId.HasValue ? AnimalId.ToString() : "null") + "}";
        }
    }
}