using UnityEngine;

public class TextFloat : MonoBehaviour
{
    public GameObject text;
    private Vector3 originalPos;
    private float angle;

    void Start()
    {
        originalPos = text.transform.position;
    }

    void Update()
    {
        text.transform.position = new Vector3(originalPos.x, originalPos.y + (Mathf.Sin(angle) * 0.25f), originalPos.z);
        angle += Time.deltaTime;
    }
}
