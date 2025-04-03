using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorMessageBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    public void SetText(string message)
    {
        messageText.text = message;
    }

    public void Confirm()
    {
        GameObject.Destroy(gameObject);
    }
}
