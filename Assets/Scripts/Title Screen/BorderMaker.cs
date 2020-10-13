using Unity.Mathematics;
using UnityEngine;

public class BorderMaker : MonoBehaviour
{
    public GameObject cornerPiece;
    public GameObject horizontalPiece;

    // Start is called before the first frame update
    void Start()
    {
        GameObject border = GameObject.FindGameObjectWithTag("Border");
        GameObject borderPiece;
        Vector3[] positions = { new Vector3(0, 313, 0), new Vector3(556, 313, 0) , new Vector3(0, 0, 0) , new Vector3(556, 0, 0) };
        float[] angles = { 0f, 270f, 90f, 180f };

        for (int i = 0; i < 4; i++)
        {
            borderPiece = Instantiate(cornerPiece, positions[i], quaternion.identity);
            borderPiece.transform.SetParent(border.transform);
            borderPiece.transform.eulerAngles = new Vector3(0, 0, angles[i]);
        }

        for (int xPos = 1; xPos < 6; xPos++)
        {
            foreach (float yPos in new float[] { 314, 0 })
            {
                borderPiece = Instantiate(horizontalPiece, new Vector3(xPos * 100, yPos, 0), quaternion.identity);
                borderPiece.transform.SetParent(border.transform);
            }
        }

        for (int yPos = 1; yPos < 4; yPos++)
        {
            foreach (float xPos in new float[] { 0, 556 })
            {
                borderPiece = Instantiate(horizontalPiece, new Vector3(xPos, yPos * 100, 0), quaternion.identity);
                borderPiece.transform.SetParent(border.transform);
                borderPiece.transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }
    }
}
