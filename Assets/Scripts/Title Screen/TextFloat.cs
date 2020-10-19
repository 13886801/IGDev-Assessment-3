using UnityEngine;
using UnityEngine.UI;

public class TextFloat : MonoBehaviour
{
    public GameObject text;
    private Vector3 originalPos;
    private float angle;

    void Start()
    {
        originalPos = text.transform.position;

        if (!PlayerPrefs.HasKey("totalScore.score"))
        {
            return;
        }

        string[] savedStuff = {
            "" + PlayerPrefs.GetInt("totalScore.score"),

            "" + PlayerPrefs.GetInt("mins") +
            ":" + PlayerPrefs.GetInt("secs") +
            ":" + (int)(PlayerPrefs.GetFloat("ms") * 100)
        };

        int i = 0;
        foreach (GameObject text in GameObject.FindGameObjectsWithTag("UIText"))
        {
            text.GetComponent<Text>().text = savedStuff[i];
            i++;
        }
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        text.transform.position = new Vector3(originalPos.x, originalPos.y + (Mathf.Sin(angle) * 0.25f), originalPos.z);
        angle += Time.deltaTime;
    }
}
