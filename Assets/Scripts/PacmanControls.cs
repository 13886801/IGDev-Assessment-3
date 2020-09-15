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
    private KeyCode[] movement2 = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(movement[i]) || Input.GetKeyDown(movement2[i]))
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
