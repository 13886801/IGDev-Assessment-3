using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip[] OST = new AudioClip[3];
    public AudioSource BGM;

    public void PlayOST(int index)
    {
        if (BGM.clip != OST[index] || !BGM.isPlaying)
        {
            BGM.clip = OST[index];
            BGM.Play();
        }
    }
}
