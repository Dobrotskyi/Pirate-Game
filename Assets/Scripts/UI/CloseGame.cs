using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGame : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
