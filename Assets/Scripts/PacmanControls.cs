using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PacmanControls : MonoBehaviour
{
    public GameObject pacman;
    private float angle = 0;
    private float preAngle = 0;

    private KeyCode[] movement = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(movement[i]))
            {
                angle = 90f * i;
            }
        }

        if (preAngle != angle)
        {
            pacman.transform.eulerAngles = new Vector3(0, 0, angle);
            preAngle = angle;
        }
    }
}
