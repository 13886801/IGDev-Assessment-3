using UnityEngine;

public class TextFloat : MonoBehaviour
{
    public GameObject Text;
    private float angle;

    void Update()
    {
        Text.transform.Translate(Vector3.up * (Mathf.Sin(angle) * 0.05f));
        angle += 0.01f;
    }
}
