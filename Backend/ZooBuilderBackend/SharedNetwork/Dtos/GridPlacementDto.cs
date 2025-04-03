using System;

namespace SharedNetwork.Dtos
{
    [Serializable]
    public struct GridPlacementDto : IStringSerializable
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int ZooId { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int AnimalCount { get; set; }
        public bool Connected { get; set; }

        public void FromString(string dataString)
        {
            this = StringSerializer.Deserialize<GridPlacementDto>(dataString);
        }

        public override string ToString()
        {
            return StringSerializer.Serialize(this);
        }
    }

}