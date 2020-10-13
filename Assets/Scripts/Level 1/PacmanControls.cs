using UnityEngine;

public class PacmanControls : MonoBehaviour
{
    public GameObject pacman;
    public AudioSource swim;

    private float angle = 0;
    private float preAngle = 0;
    private string facing = "Up";

    private float time = 0;
    private float delta = 0;

    /* Not included for assessment 3
    private string[] direction = new string[] { "Up", "Left", "Down", "Right" };
    private KeyCode[] movement = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    private KeyCode[] movement2 = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
    */

    // Start is called before the first frame update
    void Start()
    {
        swim = pacman.GetComponent<AudioSource>();
    }

    private void updateAngle()
    {
        if (preAngle != angle)
        {
            pacman.transform.eulerAngles = new Vector3(0, 0, angle);
            preAngle = angle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* Not included for assessment 3
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(movement[i]) || Input.GetKeyDown(movement2[i]))
            {
                angle = 90f * i;
                facing = direction[i];
            }
        }
        */

        if (facing == "Up" && pacman.transform.position.y >= 13)
        {
            facing = "Right";
            angle = 270;
        } else if (facing == "Right" && pacman.transform.position.x >= 6)
        {
            facing = "Down";
            angle = 180;
        } else if (facing == "Down" && pacman.transform.position.y <= 9)
        {
            facing = "Left";
            angle = 90;
        } else if (facing == "Left" && pacman.transform.position.x <= 1)
        {
            facing = "Up";
            angle = 0;
        }

        pacman.transform.Translate(0f, (1f / 0.3f) * delta, 0f);

        updateAngle();

        time += delta;
        if (time > 0.5)
        {
            time = 0;
            swim.Play();
        }

        delta = Time.deltaTime;
    }
}
