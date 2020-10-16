using UnityEngine;

public class PacmanController : MonoBehaviour
{
    private AudioSource SFXSource;

    private float angle = 0;
    private float delta = 0;

    private KeyCode[] movement = new KeyCode[] {
        KeyCode.W, KeyCode.A,
        KeyCode.S, KeyCode.D
    };
    private KeyCode[] movement2 = new KeyCode[] {
        KeyCode.UpArrow, KeyCode.LeftArrow,
        KeyCode.DownArrow, KeyCode.RightArrow
    };

    // Start is called before the first frame update
    void Start()
    {
        SFXSource = gameObject.GetComponent<AudioSource>();
    }

    private void updateAngle(float newAngle)
    {
        if (newAngle != angle)
        {
            angle = newAngle;
            gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        changePosition();
        delta = Time.deltaTime;
    }

    private void getInput()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(movement[i]) || Input.GetKeyDown(movement2[i]))
            {
                updateAngle(90f * i);
            }
        }
    }

    private void changePosition()
    {
        gameObject.transform.Translate(0f, 4f * delta, 0f);
        if (gameObject.transform.position.x < 0)
        {
            gameObject.transform.position = new Vector3(26f, 0f, -1);
        } else if (gameObject.transform.position.x > 27) {
            gameObject.transform.position = new Vector3(1f, 0f, -1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Points")
        {
            Debug.Log("Name: " + collision.gameObject.name);
        }
    }
}
