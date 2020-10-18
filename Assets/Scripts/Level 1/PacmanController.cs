using UnityEngine;

public class PacmanController : MonoBehaviour
{
    private Vector3 prevPos;
    private string lastInput = "";
    private string currentInput = "";
    private Tween tween;

    private PacStudentSensor fishSenses;
    private Animator pacAnim;
    private AudioSource pacAudioSource;
    public AudioClip pop;
    public AudioClip wallBump;

    private TextDisplay score;

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

        fishSenses = GameObject.FindGameObjectWithTag("Sensor").GetComponent<PacStudentSensor>();
        pacAnim = gameObject.GetComponent<Animator>();
        pacAnim.speed = 0;

        pacAudioSource = gameObject.GetComponent<AudioSource>();

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
        if (tween.hasTween || lastInput.Equals(currentInput) || fishSenses.IsThereAWall(lastInput))
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
    private void updatePosition()
    {
        if (tween.hasTween)
        {
            gameObject.transform.position = tween.calculatePosition(Time.deltaTime);
        } else
        {
            if (gameObject.transform.position.x < 1)
            {
                gameObject.transform.position = new Vector3(26f, 0f, -1);
            }
            else if (gameObject.transform.position.x > 26)
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
                    pacAudioSource.Pause();
                    pacAnim.speed = 0;
                    return;
            }
            if (!pacAudioSource.isPlaying)
            {
                pacAudioSource.Play();
                pacAnim.speed = 1;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collider)
    {
        if (score == null)
        {
            return;
        }

        switch(collider.gameObject.tag)
        {
            case "Wall":
                pacAudioSource.PlayOneShot(wallBump);
                gameObject.transform.position = prevPos;
                currentInput = "";
                lastInput = "";
                tween.stopTween();
                break;

            case "Points":
                pacAudioSource.PlayOneShot(pop);
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
