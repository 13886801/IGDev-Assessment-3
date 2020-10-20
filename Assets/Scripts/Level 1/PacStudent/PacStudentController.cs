using System.Collections;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Vector3 prevPos;
    private string lastInput = "";
    private string currentInput = "";
    private Tween tween;
    private TextDisplay GUIText;

    private PacStudentSensor fishSenses;
    private BoxCollider2D hitbox;
    private SpriteRenderer frameRenderer;

    private ParticleSystem.EmissionModule[] particles = new ParticleSystem.EmissionModule[3];

    private Animator pacAnim;
    private AudioSource pacAudioSource;
    public AudioClip pop;
    public AudioClip wallBump;


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
        prevPos = gameObject.transform.position;
        tween = new Tween();

        GUIText = GameObject.FindGameObjectWithTag("TextManager").GetComponent<TextDisplay>();
        GUIText.GetPacStudentController(this);

        fishSenses = GameObject.FindGameObjectWithTag("Sensor").GetComponent<PacStudentSensor>();
        hitbox = gameObject.GetComponent<BoxCollider2D>();
        SetHitBox(false);

        frameRenderer = gameObject.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            particles[i] = gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().emission;
            particles[i].enabled = false;
        }

        pacAnim = gameObject.GetComponent<Animator>();
        pacAnim.speed = 0;

        pacAudioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!hitbox.enabled)
        {
            return;
        }

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
                    tween.setTweenValues(prevPos, new Vector2(prevPos.x, prevPos.y + 1), 0.25f);
                    break;

                case "Left":
                    tween.setTweenValues(prevPos, new Vector2(prevPos.x - 1, prevPos.y), 0.25f);
                    break;

                case "Down":
                    tween.setTweenValues(prevPos, new Vector2(prevPos.x, prevPos.y - 1), 0.25f);
                    break;

                case "Right":
                    tween.setTweenValues(prevPos, new Vector2(prevPos.x + 1, prevPos.y), 0.25f);
                    break;

                default:
                    if (currentInput.Equals("WallBump"))
                    {
                        pacAudioSource.PlayOneShot(wallBump);
                        currentInput = "";
                        lastInput = "";
                    }
                    pacAnim.speed = 0;
                    return;
            }
            if (!pacAudioSource.isPlaying)
            {
                particles[2].enabled = true;
                pacAudioSource.loop = true;
                pacAudioSource.Play();
                pacAnim.speed = 1;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collider)
    {
        if (GUIText == null)
        {
            return;
        }

        switch(collider.gameObject.tag)
        {
            case "Wall":
                if (!particles[0].enabled)
                {
                    StartCoroutine("Bleed", 2);
                }
                Stop("WallBump");
                break;

            case "Points":
                switch(collider.gameObject.name)
                {
                    case "Gem":
                        GUIText.IncreaseScore(100);
                        break;

                    default:
                        GUIText.IncreaseScore(10);
                        pacAudioSource.PlayOneShot(pop);
                        break;
                }
                Destroy(collider.gameObject);
                break;

            case "PowerUp":
                GUIText.IncreaseScore(50);
                GUIText.PowerTimer();
                Destroy(collider.gameObject);
                break;

            case "Predator":
                Stop("");
                StartCoroutine("Death");
                break;

            case "Prey":
                GUIText.IncreaseScore(300);
                //Add more things here.
                break;
        }
    }

    public void SetHitBox(bool boolean)
    {
        hitbox.enabled = boolean;
    }

    private void Stop(string soundEffect)
    {
        particles[2].enabled = false;
        gameObject.transform.position = prevPos;
        currentInput = soundEffect;
        lastInput = soundEffect;
        tween.stopTween();
        pacAudioSource.loop = false;
    }

    private IEnumerator Bleed(float seconds)
    {
        float sec = seconds;
        particles[0].enabled = true;
        while (sec > 0f)
        {
            sec -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        particles[0].enabled = false;
    }

    private IEnumerator Death()
    {
        float time = 2f;
        SetHitBox(false);
        particles[1].enabled = true;
        particles[2].enabled = false;

        pacAnim.SetBool("Dead", true);

        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        frameRenderer.enabled = false;
        particles[1].enabled = false;
        yield return null;

        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        GUIText.DecrementLife();
        pacAnim.SetBool("Dead", false);
        gameObject.transform.position = new Vector2(1f, 13f);
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, -90f);

        SetHitBox(true);
        frameRenderer.enabled = true;
    }
}
