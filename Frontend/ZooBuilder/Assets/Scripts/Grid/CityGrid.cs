using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class CityGrid : MonoBehaviour
    {
        public static CityGrid Instance { get; private set; }
        
        [SerializeField] private Vector2Int _gridSize;

        [SerializeField] private Vector2 _cellSize;

        [SerializeField] private Transform _gridContainer;
        [SerializeField] private LineRenderer _gridRenderer;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GenerateGridGraphic();
        }

        public Vector3 GetPositionOnGrid(int x, int y)
        {
            float xPos = x * _cellSize.x;
            float yPos = y * _cellSize.y;
            return new Vector3(xPos, 0, yPos) + transform.position;
        }

        public void SetGridVisibility(bool active)
        {
            _gridContainer.gameObject.SetActive(active);
        }

        private void GenerateGridGraphic()
        {
            //  Vertical lines
            for (var x = 0; x < _gridSize.x + 1; x++)
            {
                var line = Instantiate(_gridRenderer, _gridContainer);
                line.positionCount = 2;
                var startPos = new Vector3(x * _cellSize.x, 0, 0);
                var endPos = new Vector3(x * _cellSize.x, 0, _gridSize.y * _cellSize.y);
                line.SetPosition(0, startPos);
                line.SetPosition(1, endPos);
            }
            //  Horizontal lines
            for (var y = 0; y < _gridSize.y + 1; y++)
            {
                var line = Instantiate(_gridRenderer, _gridContainer);
                line.positionCount = 2;
                var startPos = new Vector3(0, 0, y * _cellSize.y);
                var endPos = new Vector3(_gridSize.x * _cellSize.x, 0, y * _cellSize.y);
                line.SetPosition(0, startPos);
                line.SetPosition(1, endPos);
            }
        }
    }
}