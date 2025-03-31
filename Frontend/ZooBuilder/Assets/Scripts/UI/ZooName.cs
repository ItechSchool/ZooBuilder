using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZooName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;

    private string _currentText;

    private void Start()
    {
        ConnectionHandler.Instance.ZooNameUpdated += zooName => _currentText = zooName;
    }

    private void Update()
    {
        if (ConnectionHandler.Instance.Connected == false)
        {
            _currentText = "Connecting ...";
        }

        if (_nameText.text != _currentText)
        {
            SetZooNameText(_currentText);
        }
    }

    private void SetZooNameText(string text)
    {
        _nameText.text = text;
        var width = _nameText.GetPreferredValues(text).x;
        _nameText.rectTransform.sizeDelta = new Vector2(width, _nameText.rectTransform.sizeDelta.y);
    }
}
