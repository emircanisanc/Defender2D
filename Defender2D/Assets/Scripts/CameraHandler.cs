using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 5f;

    private float orthographicSize;
    private float targetOrthographicSize;

    void Start() {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }

    void Update() {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom() {
        targetOrthographicSize += Input.mouseScrollDelta.y * -zoomAmount;
        
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, 10, 30);

        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}