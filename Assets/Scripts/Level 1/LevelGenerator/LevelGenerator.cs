using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    public TextDisplay textDisplay;

    public GameObject[] tiles = new GameObject[7];
    public GameObject[] pacStudent = new GameObject[2];
    public GameObject jellyFish;
    public GameObject Gem;
    public Canvas HUD;

    public System.Random RNG = new System.Random();
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        bool isLevel2 = SceneManager.GetActiveScene().name.Equals("InnovationScene");
        loadLevel(isLevel2);
        loadPacStudent(isLevel2);
        loadJellyfish();
    }

    private void loadLevel(bool isLevel2)
    {
        GameObject topLeft = new GameObject();
        topLeft.transform.SetParent(gameObject.transform);
        topLeft.name = "topLeft";

        LevelMap levelMap = new LevelMap();
        int tileID;
        float angle;

        for (int y = 14; y >= 0; y--)
        {
            for (int x = 0; x < 14; x++)
            {
                if (levelMap.isNothing(tileID = levelMap.getValue(y, x)))
                {
                    continue;
                }

                angle = levelMap.getAngle(y, x);
                Instantiate(
                    tiles[tileID - 1],
                    new Vector3(x, 14 - y, 0),
                    Quaternion.Euler(new Vector3(0, 0, angle)),
                    topLeft.transform);
            }
        }

        string[] names = { "topRight", "bottomLeft", "bottomRight"};
        Vector3[] positions = { new Vector3(27, 0, 0), new Vector3(0, 0, 0), new Vector3(27, 0, 0) };
        Vector3[] rots = { new Vector3(0, 180, 0), new Vector3(180, 0, 0), new Vector3(180, 180, 0) };
        GameObject[] levelParts = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            levelParts[i] = Instantiate(topLeft, positions[i], Quaternion.Euler(rots[i]), gameObject.transform);
            levelParts[i].name = names[i];
            if (i != 0)
            {
                for (int j = 0; j < 2; j++)
                {
                    Destroy(levelParts[i].transform.GetChild(1 - j).gameObject);
                }
            }

            foreach (Transform childTile in levelParts[i].transform)
            {
                if (!childTile.gameObject.CompareTag("Wall"))
                {
                    childTile.localRotation = childTile.parent.localRotation;
                }
            }
        }

        GameObject points = new GameObject();
        points.name = "points";
        points.transform.SetParent(gameObject.transform);


        if (isLevel2)
        {
            topLeft.transform.position = new Vector3(14, 0, 0);
            levelParts[0].transform.position = new Vector3(14, 0, 0);
            levelParts[1].transform.position = new Vector3(14, 0, 0);
            levelParts[2].transform.position = new Vector3(14, 0, 0);
        }

        foreach (string tag in new string[] { "Points", "PowerUp" })
        {
            foreach (GameObject tile in GameObject.FindGameObjectsWithTag(tag))
            {
                tile.transform.SetParent(points.transform);
            }
        }
        textDisplay.BubbleCount(points);
    }

    private void loadPacStudent(bool isLevel2)
    {
        GameObject pacStudentContainer= new GameObject();
        pacStudentContainer.name = "pacStudent";

        float x = (isLevel2) ? 2f : 1f;
        Instantiate(pacStudent[0], new Vector3(x, 13, -1),
            Quaternion.Euler(new Vector3(0f, 0f, 270f)),
            pacStudentContainer.transform).name = "pacBody";

        Instantiate(pacStudent[1], new Vector3(1, 13, -1), quaternion.identity,
            pacStudentContainer.transform).name = "pacSensor";
    }

    private void loadJellyfish()
    {
        GameObject jellyfishes = new GameObject();
        jellyfishes.name = "Jellyfishes";

        float[] xPos = { 11f, 12f, 15f, 16f };

        Color[] colours = {
            new Color(1, 12f / 255, 0), new Color(1, 81f / 255, 1),
            new Color(1, 1, 0), new Color(0, 1, 35f / 255)
        };

        GameObject colouredJellyfish;
        string[] names = { "Red", "Pink", "Yellow", "Green" };
        Transform canvas;
        for (int i = 0; i < 4; i++)
        {
            colouredJellyfish = Instantiate(jellyFish, new Vector2(xPos[i], 0),
                quaternion.identity, jellyfishes.transform);

            colouredJellyfish.GetComponent<SpriteRenderer>().color = colours[i];
            colouredJellyfish.name = names[i];

            canvas = colouredJellyfish.transform.GetChild(1);
            canvas.name = "Ghost" + (i + 1) + "Canvas";
            canvas.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1);
        }
    }

    void Update()
    {
        if (textDisplay.GetPowerTime() != -1)
        { 
            SpawnGem();
            timer += Time.deltaTime;
        }
    }

    private void SpawnGem()
    {
        if ((int)timer % 30 == 0 && (int)timer != 0)
        {
            Instantiate(Gem, HUD.transform).name = "Gem";
            timer = 0;
        }
    }
}
