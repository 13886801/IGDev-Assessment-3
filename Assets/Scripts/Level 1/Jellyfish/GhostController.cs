using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GhostController : MonoBehaviour
{
    private JellyfishSensor[] sensor;
    private Animator anim;
    private Tween tween = new Tween();
    private TextDisplay textDisplay;

    private int previousSpot = -1;
    private int currentMovementIndex = -1;

    private Vector2 prevPos;
    private Vector2 spawnPoint;
    private Vector2 destination;
    private Vector2[] corners;

    private bool[] validMovement = new bool[4];
    private int[] sensorDistance = new int[4];

    private int borderPos;

    void Start()
    {
        sensor = gameObject.GetComponentsInChildren<JellyfishSensor>();
        anim = gameObject.GetComponent<Animator>();
        textDisplay = GameObject.FindGameObjectWithTag("TextManager").GetComponent<TextDisplay>();

        spawnPoint = gameObject.transform.position;
        RandomisedSpawn();

        corners = new Vector2[] {
            new Vector2(1, 13), //Top Left
            new Vector2(12, 13), //Top Middle Left
            new Vector2(15, 13), //Top Middle Right
            new Vector2(26, 13), //Top Right

            new Vector2(26, 6), //Top Right Bottom
            new Vector2(25, 0), //Middle Right
            new Vector2(26, -6), //Bottom Right Top

            new Vector2(26, -13), //Bottom Right
            new Vector2(15, -13), //Bottom Middle Right
            new Vector2(12, -13), //Bottom Middle Left
            new Vector2(1, -13), //Bottom Left

            new Vector2(1, -6), //Bottom Left Top
            new Vector2(2, 0), //Middle Left
            new Vector2(1, 6) //Middle Left
        };
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
        if (tween.hasTween || anim.GetBool("Dead?"))
        {
            return;
        }

        if (destination != Vector2.zero)
        {
            Vector2 pos = gameObject.transform.position;
            if (pos.x != destination.x)
            {
                currentMovementIndex = (pos.x < destination.x) ? 3 : 1;
                previousSpot = (currentMovementIndex + 2) % 4;
                return;
            }
            else if (gameObject.transform.position.y != destination.y)
            {
                currentMovementIndex = (pos.y < destination.y) ? 0 : 2;
                previousSpot = (currentMovementIndex + 2) % 4;
                return;
            }
            destination = Vector2.zero;
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

        if (anim.GetBool("Scared?"))
        {
            RedDirection();
            previousSpot = (currentMovementIndex + 2) % 4;
            borderPos = -1;
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
            }
            else if (sensorDistance[i] == longestDistance)
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
        int[] validSpots = { -1, -1, -1, -1 };
        int shortestDistance = int.MaxValue;
        ChooseCornerPos(gameObject.transform.position);
        for (int i = 0; i < 4; i++)
        {
            if (!validMovement[i])
            {
                continue;
            }

            sensorDistance[i] = sensor[i].CalculateDistance(corners[borderPos]);
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
    }

    private void ChooseCornerPos(Vector2 pos)
    {
        if (borderPos == -1)
        {
            borderPos = (pos.y > 0) ? 2 : 9;
            return;
        }

        if (corners[borderPos] == pos)
        {
            borderPos = (borderPos + 1) % corners.Length;
        }
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
        if (anim.GetBool("Dead?"))
        {
            if (prevPos == spawnPoint)
            {
                anim.SetBool("Dead?", false);
                RandomisedSpawn();
            }
            else
            {
                tween.setTweenValues(prevPos, spawnPoint, Mathf.Sqrt(Vector2.Distance(prevPos, spawnPoint)) * 0.25f);
            }
            return;
        }

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

    private void RandomisedSpawn()
    {
        int[] randX = { 13, 14 };
        int[] randY = { -3, 3 };
        destination = new Vector2(randX[Random.Range(0, 2)], randY[Random.Range(0, 2)]);
        borderPos = -1;
    }
}