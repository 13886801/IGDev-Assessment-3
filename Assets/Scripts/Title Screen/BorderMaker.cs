using Unity.Mathematics;
using UnityEngine;

public class BorderMaker : MonoBehaviour
{
    public GameObject cornerPiece;
    public GameObject horizontalPiece;
    public RectTransform canvasDimensions;

    // Start is called before the first frame update
    void Start()
    {
        GameObject border = GameObject.FindGameObjectWithTag("Border");
        GameObject borderPiece;
        float w = canvasDimensions.rect.width;
        float h = canvasDimensions.rect.height;

        Vector3[] positions = { new Vector3(0, h, 0), new Vector3(w, h, 0) , new Vector3(0, 0, 0) , new Vector3(w, 0, 0) };
        float[] angles = { 0f, 270f, 90f, 180f };

        for (int i = 0; i < 4; i++)
        {
            borderPiece = Instantiate(cornerPiece, positions[i], quaternion.identity);
            borderPiece.transform.SetParent(border.transform);
            borderPiece.transform.eulerAngles = new Vector3(0, 0, angles[i]);
        }

        for (int xPos = 1; xPos < (int) w / 100 + 1; xPos++)
        {
            foreach (float yPos in new float[] { h, 0 })
            {
                borderPiece = Instantiate(horizontalPiece, new Vector3(xPos * 100, yPos, 0), quaternion.identity);
                borderPiece.transform.SetParent(border.transform);
            }
        }

        for (int yPos = 1; yPos < (int) h / 100 + 1; yPos++)
        {
            foreach (float xPos in new float[] { 0, w })
            {
                borderPiece = Instantiate(horizontalPiece, new Vector3(xPos, yPos * 100, 0), quaternion.identity);
                borderPiece.transform.SetParent(border.transform);
                borderPiece.transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }
    }
}
