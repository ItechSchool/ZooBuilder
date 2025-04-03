using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ZooName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    private string _currentText;

    private void Start()
    {
        ConnectionHandler.Instance.ZooInfoUpdated += zooInfo => _currentText = zooInfo.Name;
    }

    private void Update()
    {
        if (ConnectionHandler.Instance.Connected == false)
        {
            _currentText = "Connecting ...";
        }

        if (nameText.text != _currentText)
        {
            SetZooNameText(_currentText);
        }
    }

    private void SetZooNameText(string text)
    {
        nameText.text = text;
        var width = nameText.GetPreferredValues(text).x;
        nameText.rectTransform.sizeDelta = new Vector2(width, nameText.rectTransform.sizeDelta.y);
    }
}
