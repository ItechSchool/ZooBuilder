using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MessagesDisplay : MonoBehaviour
{
    private int mainThread;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private Transform messageContainer;
    [SerializeField] private GameObject backgroundImage;
    
    private void Start()
    {
        mainThread = Thread.CurrentThread.ManagedThreadId;
        ConnectionHandler.Instance.ThrowError += ShowErrorMessage;
    }

    private void Update()
    {
        backgroundImage.SetActive(messageContainer.childCount > 0);
    }

    private void ShowErrorMessage(string message)
    {
        var messageBox = Instantiate(errorMessage, messageContainer).GetComponent<ErrorMessageBox>();
        messageBox.SetText(message);
    }
}
