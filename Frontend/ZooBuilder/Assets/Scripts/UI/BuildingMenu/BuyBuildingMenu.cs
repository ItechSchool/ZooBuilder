using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using SharedNetwork.Dtos;
using UI.BuildingMenu;
using UnityEngine;

namespace UI.BuildingMenu
{
    public class BuyBuildingMenu : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField]
        private BuildingData[] buildings;
        
        private readonly List<BuildingDto> _buildingDTOs = new List<BuildingDto>();

        [SerializeField] private string[] buildingTypes;
        private int _tabIndex = 0;

        [SerializeField] private Transform contentContainer;
        [SerializeField] private GameObject contentPrefab;
        private void Start()
        {
            CloseShop();
            ConnectionHandler.Instance.BuildingAdded += building =>
            {
                _buildingDTOs.Add(building);
            };
            
        }

        public void OpenShop()
        {
            container.SetActive(true);
            ActionButtons.Hide();
            LoadShop();
        }

        public void CloseShop()
        {
            ActionButtons.Show();
            container.SetActive(false);
        }

        public void ChangeTab(int index)
        {
            _tabIndex = index;
            LoadShop();
        }

        private void ClearContentList()
        {
            for (var i = 0; i < contentContainer.childCount; i++)
            {
                Destroy(contentContainer.GetChild(i).gameObject);
            }
        }
        
        private void LoadShop()
        {
            ClearContentList();
            var buildingsInCategory = _buildingDTOs.Where(building => building.Type.Equals(buildingTypes[_tabIndex])).ToArray();
            var matchingBuildings = buildings.Where(building =>
                buildingsInCategory.Select(buildingDto => buildingDto.Id).Contains(building.id)).ToArray();

            foreach (var matchingBuilding in matchingBuildings)
            {
                var buildingDto = _buildingDTOs.First(buildingDto => buildingDto.Id == matchingBuilding.id);
                var button = Instantiate(contentPrefab, contentContainer).GetComponent<BuyBuildingButton>();
                button.SetPreviewImage(matchingBuilding.image);
                button.SetName(buildingDto.Name);
                button.SetCosts(buildingDto.Costs);
            }
        }
    }
}