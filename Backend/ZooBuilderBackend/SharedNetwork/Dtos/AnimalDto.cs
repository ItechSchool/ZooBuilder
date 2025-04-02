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
            string cleanedString = dataString.Replace("{", string.Empty).Replace("}", string.Empty);
            string[] values = cleanedString.Split(';');

            Id = int.Parse(values[0]);
            Species = values[1];
            Costs = int.Parse(values[2]);
            Diet = values[3];
            Attraction = int.Parse(values[4]);
            Hunger = int.Parse(values[5]);
        }

        public override string ToString()
        {
            return "{" + Id + ";" + Species + ";" + Costs + ";" + Diet + ";" + Attraction + ";" + Hunger + "}";
        }
    }
}