using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    public float xRotation;
    public float yRotation;
    
    private float fov;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        xRotation = transform.parent.rotation.eulerAngles.x;
        yRotation = transform.parent.rotation.eulerAngles.y;
        float sens = PlayerPrefs.GetFloat("sensitivity", 1f);
        sensX = sens;
        sensY = sens;
        fov = GetComponent<Camera>().fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && Player.Instance.health > 0)
        {
            float xLook = Input.GetAxisRaw("Mouse X") * sensX;
            float yLook = Input.GetAxisRaw("Mouse Y") * sensY;

            yRotation += xLook;
            xRotation -= yLook;
        
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        if (Player.Instance.player.GetComponent<PlayerMove>().isSprinting)
        {
            GetComponent<Camera>().fieldOfView =
                Mathf.Lerp(GetComponent<Camera>().fieldOfView, fov + 10f, Time.deltaTime * 10f);
        }
        else
        {
            GetComponent<Camera>().fieldOfView =
                Mathf.Lerp(GetComponent<Camera>().fieldOfView, fov, Time.deltaTime * 10f);
        }
            
    }
}
