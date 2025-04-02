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
            string cleanedString = dataString.Replace("{", string.Empty).Replace("}", string.Empty);
            string[] values = cleanedString.Split(';');

            Id = int.Parse(values[0]);
            BuildingId = int.Parse(values[1]);
            ZooId = int.Parse(values[2]);
            XCoordinate = int.Parse(values[3]);
            YCoordinate = int.Parse(values[4]);
            AnimalCount = int.Parse(values[5]);
            Connected = bool.Parse(values[6]);
        }

        public override string ToString()
        {
            return "{" + Id + ";" + BuildingId + ";" + ZooId + ";" + XCoordinate + ";" + YCoordinate + ";" + AnimalCount + ";" + Connected + "}";
        }
    }

}