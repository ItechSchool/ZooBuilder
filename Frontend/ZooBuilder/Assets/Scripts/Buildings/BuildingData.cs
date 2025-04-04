using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "Data/Building", fileName = "Building")]
    public class BuildingData : ScriptableObject
    {
        public int id;
        public Sprite image;
        public GameObject prefab;
    }
}