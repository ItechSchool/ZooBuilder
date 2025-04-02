using System;

namespace SharedNetwork.Dtos
{
    [Serializable]
    public struct ZooDto : IStringSerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public int Vegetables { get; set; }
        public int Meat { get; set; }
        public int PlayerId { get; set; }

        public void FromString(string dataString)
        {
            string cleanedString = dataString.Replace("{", string.Empty).Replace("}", string.Empty);
            string[] values = cleanedString.Split(';');

            Id = int.Parse(values[0]);
            Name = values[1];
            Money = int.Parse(values[2]);
            Vegetables = int.Parse(values[3]);
            Meat = int.Parse(values[4]);
            PlayerId = int.Parse(values[5]);
        }

        public override string ToString()
        {
            return "{" + Id + ";" + Name + ";" + Money + ";" +
                   Vegetables + ";" + Meat + ";" + PlayerId + "}";
        }
    }
}