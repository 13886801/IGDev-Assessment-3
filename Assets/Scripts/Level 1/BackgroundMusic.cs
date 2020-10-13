using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip[] OST = new AudioClip[4];
    public AudioClip Current;
    public AudioSource BGM;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        BGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 5 && Current != OST[1])
        {
            BGM.clip = OST[1];
            Current = OST[1];
            BGM.Play();
        }
        time += Time.deltaTime;   
    }
}
