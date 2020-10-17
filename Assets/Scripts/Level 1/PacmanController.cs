using UnityEngine;

public class PacmanController : MonoBehaviour
{
    private Vector3 prevPos;
    private string lastInput = "";
    private string currentInput = "";
    private Tween tween;
    private PacStudentSensor fishSenses;

    private TextDisplay score;
    private AudioSource bubblePop;

    private string[] directions = { "Up", "Left", "Down", "Right" };
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
        score = GameObject.FindGameObjectWithTag("TextManager").GetComponent<TextDisplay>();
        bubblePop = gameObject.GetComponentInChildren<AudioSource>();

        fishSenses = GameObject.FindGameObjectWithTag("Sensor").GetComponent<PacStudentSensor>();

        tween = new Tween();
        prevPos = gameObject.transform.position;
    }

    void Update()
    {
        getInput();
        changeDirection();
        updatePosition();
    }

    private void getInput()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(movement[i]) || Input.GetKeyDown(movement2[i]))
            {
                lastInput = directions[i];
            }
        }
    }
    private void changeDirection()
    {
        if (tween.hasTween || lastInput.Equals(currentInput))
        {
            return;
        }

        if (fishSenses.IsThereAWall(lastInput))
        {
            return;
        }

        currentInput = lastInput;
        for (int i = 0; i < 4; i++)
        {
            if (currentInput.Equals(directions[i]))
            {
                gameObject.transform.eulerAngles = new Vector3(0, 0, 90f * i);
                break;
            }
        }
    }

    private bool OppositeDirection()
    {
        switch(lastInput)
        {
            case "Up":
                return gameObject.transform.rotation.z > 90f;

            case "Left":
                return gameObject.transform.rotation.z < 0f;

            case "Down":
                return gameObject.transform.rotation.z < 90f;

            case "Right":
                return gameObject.transform.rotation.z > 0f;
        }
        return false;
    }

    private void updatePosition()
    {
        if (tween.hasTween)
        {
            gameObject.transform.position = tween.calculatePosition(Time.deltaTime);
        } else
        {
            if (gameObject.transform.position.x < 0)
            {
                gameObject.transform.position = new Vector3(26f, 0f, -1);
            }
            else if (gameObject.transform.position.x > 27)
            {
                gameObject.transform.position = new Vector3(1f, 0f, -1);
            }
            prevPos = gameObject.transform.position;

            switch (currentInput)
            {
                case "Up":
                    tween.setTweenValue(prevPos, new Vector2(prevPos.x, prevPos.y + 1), 0.25f);
                    break;

                case "Left":
                    tween.setTweenValue(prevPos, new Vector2(prevPos.x - 1, prevPos.y), 0.25f);
                    break;

                case "Down":
                    tween.setTweenValue(prevPos, new Vector2(prevPos.x, prevPos.y - 1), 0.25f);
                    break;

                case "Right":
                    tween.setTweenValue(prevPos, new Vector2(prevPos.x + 1, prevPos.y), 0.25f);
                    break;

                default:
                    break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (score == null)
        {
            return;
        }

        switch(collider.gameObject.tag)
        {
            case "Wall":
                gameObject.transform.position = prevPos;
                currentInput = "";
                tween.stopTween();
                break;

            case "Points":
                bubblePop.Play();
                score.IncreaseScore(10);
                collider.gameObject.SetActive(false);
                break;

            case "PowerUp":
                score.IncreaseScore(50);
                collider.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }
}
