using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public Camera cameraVC;

    public Plane planeFloor;

    void Start()
    {
       
        planeFloor = new Plane(Vector3.up, 0f);
    }

    void Update()
    {
        Ray ray = cameraVC.ScreenPointToRay(Input.mousePosition);;
       

        float hitDist;
            if (planeFloor.Raycast(ray, out hitDist))
        {
            Vector3 lookPoint = ray.GetPoint(hitDist);
            Vector3 lookDir = lookPoint - Player.transform.position;
            lookDir.y = 0f; // Nur auf XZ-Ebene rotieren

            if (lookDir.sqrMagnitude > 0.001f)
                Player.transform.rotation = Quaternion.LookRotation(lookDir);
        }



    }
    
    
}