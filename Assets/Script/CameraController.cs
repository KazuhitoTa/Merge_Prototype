using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    public float zoomSpeed = 15f; // カメラのズーム速度
    Camera cam;

    void Start()
    {
        cam=gameObject.GetComponent<Camera>();
    }
    void Update()
    {
        int direction = (Input.GetKey(KeyCode.W))? -1 : (Input.GetKey(KeyCode.S))? 1 : 0;
        transform.position += direction * transform.up * moveSpeed * Time.deltaTime;
        direction = (Input.GetKey(KeyCode.D))? -1 : (Input.GetKey(KeyCode.A))? 1 : 0;
        transform.position += direction * transform.right * moveSpeed * Time.deltaTime;
        CameraZoom();
    }

    void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scroll, zoomSpeed);
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        float z = cam.orthographicSize+ deltaMagnitudeDiff * speed*-1;
        cam.orthographicSize=z;
    }

}
