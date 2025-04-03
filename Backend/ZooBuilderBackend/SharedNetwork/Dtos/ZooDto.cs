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
            this = StringSerializer.Deserialize<ZooDto>(dataString);
        }

        public override string ToString()
        {
            return StringSerializer.Serialize(this);
        }
    }
}