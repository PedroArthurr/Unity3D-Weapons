using System;
using UnityEngine;

public class LookWithMouse : MonoBehaviour
{
    [SerializeField] private bool lockAndHideMouse;
    [SerializeField] private PlayerSettings settings;

    private Vector2 rotation = new Vector2(0, 0);
    private void Update()
    {
        MouseLook();

        if (lockAndHideMouse)
        {
            Cursor.visible = !lockAndHideMouse;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void MouseLook()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x -= Input.GetAxis("Mouse Y");

        transform.eulerAngles = rotation * settings.mouseSensitivty;
    }
}

[Serializable]
internal struct PlayerSettings
{
    public float mouseSensitivty;
}