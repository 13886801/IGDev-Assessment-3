using Unity.Mathematics;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] Tiles = new GameObject[7];
    public GameObject[] pacman = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        GameObject topLeft = new GameObject();

        topLeft.transform.parent = gameObject.transform;
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
                    Tiles[tileID - 1],
                    new Vector3(x, 14 - y, 0),
                    Quaternion.Euler(new Vector3(0, 0, angle))
                    ).transform.parent = topLeft.transform;
            }
        }

        string[] names = { "topRight", "bottomLeft", "bottomRight"};
        Vector3[] positions = { new Vector3(27, 0, 0), new Vector3(0, 0, 0), new Vector3(27, 0, 0) };
        Vector3[] scales = { new Vector3(-1, 1, 1), new Vector3(1, -1, 1), new Vector3(-1, -1, 1) };
        GameObject[] levelParts = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            levelParts[i] = Instantiate(topLeft, positions[i], Quaternion.identity);
            levelParts[i].transform.parent = gameObject.transform;
            levelParts[i].transform.localScale = scales[i];
            levelParts[i].name = names[i];
        }

        Instantiate(pacman[0], new Vector3(1, 9, -1), quaternion.identity);

        GameObject lives = new GameObject();
        lives.name = "Lives";
        for (int i = 0; i < 3; i++)
        {
            Instantiate(pacman[1], new Vector3(i, -15, -1), quaternion.identity).transform.parent = lives.transform;
        }
    }
}
