using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private GameObject[] gameObjectsOnLevel;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObjectsOnLevel = new GameObject[] { GameObject.FindGameObjectWithTag("Lv1Btn"),
                                                GameObject.FindGameObjectWithTag("Lv2Btn")};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
