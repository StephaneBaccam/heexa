using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Vector3 NorthDirection;
    public Transform Player;

    public RectTransform NorthLayer;
    // Update is called once per frame
    void Update()
    {
        ChangeNorthDirection(); 
    }

    public void ChangeNorthDirection()
    {
        NorthDirection.z = -Player.eulerAngles.y;
        NorthLayer.localEulerAngles = NorthDirection;
    }
}
