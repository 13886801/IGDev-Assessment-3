using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tiles = new GameObject[7];
    public GameObject[] pacStudent = new GameObject[2];
    public GameObject jellyFish;

    // Start is called before the first frame update
    void Start()
    {
        loadLevel();
        loadPacStudent();
        loadJellyfish();
    }

    private void loadLevel()
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
                    Quaternion.Euler(new Vector3(0, 0, angle))
                    ).transform.SetParent(topLeft.transform);
            }
        }

        string[] names = { "topRight", "bottomLeft", "bottomRight" };
        Vector3[] positions = { new Vector3(27, 0, 0), new Vector3(0, 0, 0), new Vector3(27, 0, 0) };
        Vector3[] scales = { new Vector3(-1, 1, 1), new Vector3(1, -1, 1), new Vector3(-1, -1, 1) };
        GameObject[] levelParts = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            levelParts[i] = Instantiate(topLeft, positions[i], Quaternion.identity);
            levelParts[i].transform.SetParent(gameObject.transform);
            levelParts[i].transform.localScale = scales[i];
            levelParts[i].name = names[i];
            if (i != 0)
            {
                for (int j = 0; j < 2; j++)
                {
                    Transform destruct = levelParts[i].transform.GetChild(1 - j);
                    destruct.gameObject.AddComponent<SelfDestruct>();
                }
            }

            foreach (Transform childTile in levelParts[i].transform)
            {
                if (childTile.gameObject.tag != "Wall")
                {
                    childTile.localScale = childTile.parent.localScale;
                }
            }
        }
    }

    private void loadPacStudent()
    {
        Instantiate(pacStudent[0], new Vector3(1, 13, -1), quaternion.identity)
            .transform.eulerAngles = new Vector3(0f, 0f, 270f);
        Instantiate(pacStudent[1], new Vector3(1, 13, -1), quaternion.identity);
    }

    private void loadJellyfish()
    {
        GameObject jellyfishes = new GameObject();
        jellyfishes.name = "Jellyfishes";

        Vector3[] positions = {
            new Vector3(11.5f, 0, 0), new Vector3(12.5f, 0, 0),
            new Vector3(14.5f, 0, 0), new Vector3(15.5f, 0, 0),
        };

        Color[] colours = {
            new Color(1, 12f / 255, 0), new Color(1, 81f / 255, 1),
            new Color(1, 1, 0), new Color(0, 1, 35f / 255)
        };

        GameObject colouredJellyfish;
        string[] names = { "Red", "Pink", "Yellow", "Green" };
        Transform canvas;
        for (int i = 0; i < 4; i++)
        {
            colouredJellyfish = Instantiate(jellyFish, positions[i], quaternion.identity);
            colouredJellyfish.transform.SetParent(jellyfishes.transform);
            colouredJellyfish.GetComponent<SpriteRenderer>().color = colours[i];
            colouredJellyfish.name = names[i];

            canvas = colouredJellyfish.transform.GetChild(1);
            canvas.name = "Ghost" + (i + 1) + "Canvas";
            canvas.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1);
        }
    }
}
