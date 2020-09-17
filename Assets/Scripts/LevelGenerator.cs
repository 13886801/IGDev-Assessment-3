using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] Tiles = new GameObject[7];
    public GameObject[] pacman = new GameObject[2];

    public void Generate(int tileID, float x, float y, float angle, int area) {
        GameObject tile = Instantiate(Tiles[tileID - 1], new Vector3(x, y, 0), Quaternion.Euler(new Vector3(0, 0, angle)));
        if (!(tileID % 2 == 1 && tileID < 5))
        {
            if (tileID == 7 && area > 1)
            {
                tile.GetComponent<SpriteRenderer>().flipY = true;
            }
            return;
        }

        switch (area)
        {
            case 1:
                tile.GetComponent<SpriteRenderer>().flipX = true;
                if (angle % 90f == 0)
                {
                    tile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
                }
                break;

            case 2:
                tile.GetComponent<SpriteRenderer>().flipY = true;
                if (angle % 90f == 0)
                {
                    tile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
                }
                break;

            case 3:
                tile.GetComponent<SpriteRenderer>().flipX = true;
                tile.GetComponent<SpriteRenderer>().flipY = true;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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

                Generate(tileID, x, 14 - y, angle, 0); //Normal, top left
                Generate(tileID, 27 - x, 14 - y, angle, 1); //top right

                if (y != 14)
                {
                    Generate(tileID, x, y - 14, angle, 2); //bottom left
                    Generate(tileID, 27 - x, y - 14, angle, 3); //bottom right
                }
            }
        }

        Instantiate(pacman[0], new Vector3(1, 9, -1), quaternion.identity);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(pacman[1], new Vector3(i, -15, -1), quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
