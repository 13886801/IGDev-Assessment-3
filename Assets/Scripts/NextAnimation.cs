using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextAnimation : MonoBehaviour
{
    public Animator trigger;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if ((int)time != 0 && (int)time % 3 == 0)
        {
            trigger.SetTrigger("Next Set");
            time = 0;
        }
    }
}
