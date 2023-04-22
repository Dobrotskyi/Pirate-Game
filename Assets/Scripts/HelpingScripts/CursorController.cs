using UnityEngine;

public class CursorController : MonoBehaviour
{
    private void Start()
    {
        SetCursorOff();
    }

    public static void SetCursorOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void SetCursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
