using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public float RotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= +Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(Target);
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0); //Si on appuie sur LeftAlt, seul la caméra rotate
        }
        else
        {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            Player.rotation = Quaternion.Euler(0, mouseX, 0); //Sinon le joueur tourne aussi
        }
    }
}
