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
            this = StringSerializer.Deserialize<BuildingDto>(dataString);
        }

        public override string ToString()
        {
            return StringSerializer.Serialize(this);
        }
    }
}