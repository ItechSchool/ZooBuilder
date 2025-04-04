using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BuildingMenu
{
    public class BuyBuildingButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costsText;
        [SerializeField] private Image previewImage;

        public void SetName(string itemName)
        {
            nameText.text = itemName;
        }

        public void SetCosts(int costs)
        {
            costsText.text = $"${costs}";
        }

        public void SetPreviewImage(Sprite image)
        {
            previewImage.sprite = image;
        }
    }
}
