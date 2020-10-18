using System.IO.Pipes;
using UnityEngine;

public class GemRun : MonoBehaviour
{
    private Tween Run = new Tween();

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        Vector2 pos1 = gameObject.transform.position;
        float h = Camera.main.orthographicSize * 2f;
        float w = h * Screen.width / Screen.height;
        Run.setTweenValues(pos1, new Vector2(pos1.x + 2 + w, pos1.y), 10f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Run.calculatePosition(Time.deltaTime);
        if (gameObject.transform.position == Run.endPos)
        {
            gameObject.AddComponent<SelfDestruct>();
        }
    }
}
