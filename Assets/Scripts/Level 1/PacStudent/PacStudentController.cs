using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool isLevel2;

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

        isLevel2 = SceneManager.GetActiveScene().name.Equals("InnovationScene");
        pacAudioSource = gameObject.GetComponent<AudioSource>();

        pacAnim = gameObject.GetComponent<Animator>();
        pacAnim.speed = 0;
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
                if (!isLevel2)
                {
                    lastInput = directions[i];
                } else {
                    NoBacktracking(i);
                }
                return;
            }
        }
    }
    private void NoBacktracking(int direction)
    {
        int index = 0;

        int zRot = (int)gameObject.transform.rotation.eulerAngles.z;
        switch(zRot)
        {
            case 90:
                index = 1;
                break;

            case 180:
                index = 2;
                break;

            case 270:
                index = 3;
                break;
        }

        if (!directions[(index + 2) % 4].Equals(directions[direction]))
        {
            lastInput = directions[direction];
        }
    }

    private void changeDirection()
    {
        if (tween.hasTween || lastInput.Equals(currentInput) || fishSenses.IsThereAWall(lastInput))
        {
            return;
        }

        if (!OutsideBoundary().Equals(""))
        {
            lastInput = currentInput;
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
            prevPos = gameObject.transform.position;
            string boundary = OutsideBoundary();
            if (!boundary.Equals(""))
            {
                if (boundary.Equals("Left"))
                {
                    gameObject.transform.position = new Vector3(27f, prevPos.y, -1);
                } else
                {
                    gameObject.transform.position = new Vector3(1f, prevPos.y, -1);
                }
            }

            prevPos = gameObject.transform.position;
            switch (currentInput)
            {
                case "Up":
                    tween.setTweenValues(prevPos, new Vector3(prevPos.x, prevPos.y + 1, -1f), 0.25f);
                    break;

                case "Left":
                    tween.setTweenValues(prevPos, new Vector3(prevPos.x - 1, prevPos.y, -1f), 0.25f);
                    break;

                case "Down":
                    tween.setTweenValues(prevPos, new Vector3(prevPos.x, prevPos.y - 1, -1f), 0.25f);
                    break;

                case "Right":
                    tween.setTweenValues(prevPos, new Vector3(prevPos.x + 1, prevPos.y, -1f), 0.25f);
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

    private string OutsideBoundary()
    {
        if (gameObject.transform.position.x == 0)
        {
            return "Left";
        }
        else if (gameObject.transform.position.x == 27)
        {
            return "Right";
        }
        return "";
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
        float x = isLevel2 ? 2f : 1f;
        gameObject.transform.position = new Vector2(x, 13f);
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, -90f);

        SetHitBox(true);
        frameRenderer.enabled = true;
    }
}
