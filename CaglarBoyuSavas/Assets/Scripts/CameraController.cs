using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5.0f; // Kamera hýzý
    public float maxLeftPosition ;
    public float maxRightPosition ;
    [HideInInspector] public bool isDragging = false;


    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X");

            Vector3 newPosition = transform.position + new Vector3(-mouseX * cameraSpeed * Time.deltaTime, 0, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, maxLeftPosition, maxRightPosition);
            transform.position = newPosition;
        }
        else return;
    }
}