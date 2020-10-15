using UnityEngine;
using UnityEngine.UI;

public class BorderMovement : MonoBehaviour
{
    public Image Border;
    public Sprite[] frames = new Sprite[3];
    
    private float tick;

    // Update is called once per frame
    void Update()
    {
        Border.sprite = frames[(int)tick % frames.Length];
        tick += Time.deltaTime * 1.5f;
    }
}
