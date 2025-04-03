using System;

namespace SharedNetwork.Dtos
{
    [Serializable]
    public struct AnimalDto : IStringSerializable
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public int Costs { get; set; }
        public string Diet { get; set; }
        public int Attraction { get; set; }
        public int Hunger { get; set; }

        public void FromString(string dataString)
        {
            this = StringSerializer.Deserialize<AnimalDto>(dataString);
        }

        public override string ToString()
        {
            return StringSerializer.Serialize(this);
        }
    }
}