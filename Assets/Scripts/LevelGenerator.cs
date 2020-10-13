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
        GameObject topRight;
        GameObject bottomLeft;
        GameObject bottomRight;

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
        topRight = Instantiate(topLeft, new Vector3(27, 0, 0), Quaternion.identity);
        topRight.name = "topRight";
        topRight.transform.parent = gameObject.transform;
        topRight.transform.localScale = new Vector3(-1, 1, 1);

        bottomLeft = Instantiate(topLeft, new Vector3(0, 0, 0), Quaternion.identity);
        bottomLeft.name = "bottomLeft";
        bottomLeft.transform.parent = gameObject.transform;
        bottomLeft.transform.localScale = new Vector3(1, -1, 1);

        bottomRight = Instantiate(topLeft, new Vector3(27, 0, 0), Quaternion.identity);
        bottomRight.name = "bottomRight";
        bottomRight.transform.parent = gameObject.transform;
        bottomRight.transform.localScale = new Vector3(-1, -1, 1);

        Instantiate(pacman[0], new Vector3(1, 9, -1), quaternion.identity);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(pacman[1], new Vector3(i, -15, -1), quaternion.identity);
        }
    }
}
