using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class CityGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;

        [SerializeField] private Vector2 _cellSize;

        [SerializeField] private LineRenderer _gridRenderer;

        private void Start()
        {
            GenerateGridGraphic();
        }

        private void GenerateGridGraphic()
        {
            //  Vertical lines
            for (var x = 0; x < _gridSize.x + 1; x++)
            {
                var line = Instantiate(_gridRenderer, transform);
                line.positionCount = 2;
                var startPos = new Vector3(x * _cellSize.x, 0, 0);
                var endPos = new Vector3(x * _cellSize.x, 0, _gridSize.y * _cellSize.y);
                line.SetPosition(0, startPos);
                line.SetPosition(1, endPos);
            }
            //  Horizontal lines
            for (var y = 0; y < _gridSize.y + 1; y++)
            {
                var line = Instantiate(_gridRenderer, transform);
                line.positionCount = 2;
                var startPos = new Vector3(0, 0, y * _cellSize.y);
                var endPos = new Vector3(_gridSize.x * _cellSize.x, 0, y * _cellSize.y);
                line.SetPosition(0, startPos);
                line.SetPosition(1, endPos);
            }
        }
    }
}