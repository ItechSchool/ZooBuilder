using System;

namespace SharedNetwork.Dtos
{
    [Serializable]
    public struct StartUpDataDto
    {
        public BuildingDto[] Buildings { get; set; }
        public AnimalDto[] Animals { get; set; }
        public GridPlacementDto[] GridPlacements { get; set; }
        public ZooDto Zoo { get; set; }

    }
}
