using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _cameraObject;

    [SerializeField] private Vector2 _boundingBox;
    [SerializeField] private Vector2 _center;

    [SerializeField] private float _sensitivity = 10f;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 25f;
    [SerializeField] private float _zoomSensitivity = 5f;
    [SerializeField] private float _zoomSmoothing = 3f;
    private float _zoom = 10f;
    private Vector3 _mouseStartDragPosition;
    
    private void Update()
    {
        UpdateCameraPosition();
        UpdateZoomPosition();
    }

    private void UpdateCameraPosition()
    {
        var input = FetchMovementInput();
        var moveDirection = transform.right * input.x + transform.forward * input.y;
        moveDirection *= _sensitivity;
        var newPosition = transform.position + moveDirection;
        newPosition.x = Mathf.Clamp(newPosition.x, _center.x - _boundingBox.x, _center.x + _boundingBox.x);
        newPosition.z = Mathf.Clamp(newPosition.z, _center.y - _boundingBox.y, _center.y + _boundingBox.y);
        transform.position = newPosition;
    }

    private Vector2 FetchMovementInput()
    {
        var input = new Vector2();
        if (Input.GetKeyDown(KeyCode.Mouse0))
            _mouseStartDragPosition = Input.mousePosition;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            input = _mouseStartDragPosition - Input.mousePosition;
            _mouseStartDragPosition = Input.mousePosition;
        }
        return input;
    }

    private void FetchZoomInput()
    {
        float difference = -Input.mouseScrollDelta.y * _zoomSensitivity;
        _zoom = Mathf.Clamp(_zoom + difference, _minZoom, _maxZoom);
    }
    private void UpdateZoomPosition()
    {
        FetchZoomInput();
        var targetPosition = new Vector3(0f, _zoom, 0f);
        _cameraObject.localPosition =
            Vector3.Lerp(_cameraObject.localPosition, targetPosition, Time.deltaTime * _zoomSmoothing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(_center.x, _maxZoom / 2f, _center.y),
            new Vector3(_boundingBox.x, _maxZoom, _boundingBox.y) * 2f);
    }
}
