using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    private ScoreKeeper totalScore;

    private List<Text> UITexts = new List<Text>();


    void Start()
    {
        totalScore = new ScoreKeeper();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UIText"))
        {
            UITexts.Add(obj.GetComponent<Text>());
        }
    }

    public void IncreaseScore(int num)
    {
        totalScore.IncreaseScore(num);
    }

    void Update()
    {
        foreach (Text UIText in UITexts)
        {
            switch (UIText.name)
            {
                case "Score":
                    UIText.text = "Score:\n" + totalScore.Score;
                    break;

                case "Timer":
                    break;

                case "GhostTimer":
                    break;

                default:
                    break;
            }
        }
    }
}
