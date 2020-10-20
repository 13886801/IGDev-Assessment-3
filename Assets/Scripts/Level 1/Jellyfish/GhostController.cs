using UnityEngine;

public class GhostController : MonoBehaviour
{
    private JellyFishSensor[] sensor;
    private Animator anim;
    private Tween tween = new Tween();
    private TextDisplay textDisplay;

    private int previousSpot = -1;
    private int currentMovementIndex = -1;
    private Vector2 prevPos;
    private Vector2 Destination;

    private bool[] validMovement = new bool[4];
    private int[] sensorDistance = new int[4];

    void Start()
    {
        sensor = gameObject.GetComponentsInChildren<JellyFishSensor>();
        anim = gameObject.GetComponent<Animator>();
        textDisplay = GameObject.FindGameObjectWithTag("TextManager").GetComponent<TextDisplay>();

        int[] randX = { 13, 14 };
        int[] randY = { -3, 3 };
        Destination = new Vector2(randX[Random.Range(0, 2)], randY[Random.Range(0, 2)]);
    }

    void Update()
    {
        if ((int)textDisplay.GetPowerTime() < 0)
        {
            return;
        }

        ChooseDirection();
        UpdatePosition();
    }

    private void ChooseDirection()
    {
        if (tween.hasTween)
        {
            return;
        }

        if (Destination != Vector2.zero)
        {
            Vector2 pos = gameObject.transform.position;
            if (pos.x != Destination.x)
            {
                currentMovementIndex = (pos.x < Destination.x) ? 3 : 1;
                previousSpot = (currentMovementIndex + 2) % 4;
                return;
            } else if (gameObject.transform.position.y != Destination.y)
            {
                currentMovementIndex = (pos.y < Destination.y) ? 0 : 2;
                previousSpot = (currentMovementIndex + 2) % 4;
                return;
            }
            Destination = Vector2.zero;
        }

        for (int i = 0; i < 4; i++)
        {
            validMovement[i] = !sensor[i].IsColliding();
            sensorDistance[i] = sensor[i].CalculateDistance();
        }

        if (OppositeDirectionCheck())
        {
            return;
        }

        switch (gameObject.name)
        {
            case "Red":
                RedDirection();
                break;

            case "Pink":
                PinkDirection();
                break;

            case "Yellow":
                YellowDirection();
                break;

            case "Green":
                GreenDirection();
                break;
        }
        previousSpot = (currentMovementIndex + 2) % 4;
    }

    private void RedDirection()
    {
        int[] validSpots = { -1, -1, -1, -1 };
        int longestDistance = int.MinValue;
        for (int i = 0; i < 4; i++)
        {
            if (!validMovement[i])
            {
                continue;
            }
            
            if (sensorDistance[i] > longestDistance)
            {
                longestDistance = sensorDistance[i];
                validSpots = new int[] { -1, -1, -1, -1 };
                validSpots[i] = i;
            } else if (sensorDistance[i] == longestDistance)
            {
                validSpots[i] = i;
            }
        }
        RandomDecision(validSpots);
    }

    private void PinkDirection()
    {
        int[] validSpots = { -1, -1, -1, -1 };
        int shortestDistance = int.MaxValue;
        for (int i = 0; i < 4; i++)
        {
            if (!validMovement[i])
            {
                continue;
            }

            if (sensorDistance[i] < shortestDistance)
            {
                shortestDistance = sensorDistance[i];
                validSpots = new int[] { -1, -1, -1, -1 };
                validSpots[i] = i;
            }
            else if (sensorDistance[i] == shortestDistance)
            {
                validSpots[i] = i;
            }
        }
        RandomDecision(validSpots);

        string s = "";
        foreach (int i in validSpots)
        {
            s += i + " ";
        }
        Debug.Log("validSpots: " + s);
        Debug.Break();
    }

    private void YellowDirection()
    {
        int[] validSpots = { -1, -1, -1, -1 };
        for (int i = 0; i < 4; i++)
        {
            if (validMovement[i])
            {
                validSpots[i] = i;
            }
        }
        RandomDecision(validSpots);
    }

    private void GreenDirection()
    {
        YellowDirection();
    }

    private void RandomDecision(int[] validSpots)
    {
        int check = 0;
        foreach (int i in validSpots)
        {
            check += (i != -1) ? 1 : 0;
        }
        if (check == 0)
        {
            return;
        }

        currentMovementIndex = validSpots[Random.Range(0, 4)];
        while (currentMovementIndex == -1)
        {
            currentMovementIndex = validSpots[Random.Range(0, 4)];
        }
    }

    private bool OppositeDirectionCheck()
    {
        if (previousSpot == -1)
        {
            return false;
        }

        validMovement[previousSpot] = false;
        for (int i = 0; i < 4; i++)
        {
            if (validMovement[i])
            {
                return false;
            }
        }
        validMovement[previousSpot] = true;
        OppositeDirection();
        return true;
    }

    private void OppositeDirection()
    {
        for (int i = 0; i < 4; i++)
        {
            if (validMovement[i])
            {
                currentMovementIndex = i;
                previousSpot = (currentMovementIndex + 2) % 4;
                return;
            }
        }
    }

    private void UpdatePosition()
    {
        if (tween.hasTween)
        {
            gameObject.transform.position = tween.calculatePosition(Time.deltaTime);
            return;
        }

        prevPos = gameObject.transform.position;
        switch (currentMovementIndex)
        {
            case 0:
                tween.setTweenValues(prevPos, new Vector2(prevPos.x, prevPos.y + 1), 0.25f);
                break;

            case 1:
                tween.setTweenValues(prevPos, new Vector2(prevPos.x - 1, prevPos.y), 0.25f);
                break;

            case 2:
                tween.setTweenValues(prevPos, new Vector2(prevPos.x, prevPos.y - 1), 0.25f);
                break;

            case 3:
                tween.setTweenValues(prevPos, new Vector2(prevPos.x + 1, prevPos.y), 0.25f);
                break;
        }
        anim.SetInteger("ULDR", currentMovementIndex);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            gameObject.transform.position = prevPos;
            tween.stopTween();
        }
    }
}
