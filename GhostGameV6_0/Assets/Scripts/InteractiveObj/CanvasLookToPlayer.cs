using UnityEngine;

public class CanvasLookToPlayer : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
       if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Canvas zur Kamera ausrichten
            transform.LookAt(mainCamera.transform.position);
            
            // Canvas umdrehen, damit es nicht rückwärts schaut
            transform.Rotate(0, 180, 0);
        }
    }
}
