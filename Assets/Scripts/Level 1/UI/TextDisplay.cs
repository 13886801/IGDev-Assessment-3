using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    public GameObject announcer;
    private GameObject bubbleCount;

    private ScoreKeeper totalScore;
    private Transform lives;
    private BackgroundMusic music;

    private int mins;
    private int secs;
    private float ms;

    private float powerTime = -1;
    private int jellyfishCount = 4;

    private List<Text> UITexts = new List<Text>();
    private PacStudentController pacStudent;

    void Start()
    {
        totalScore = new ScoreKeeper();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UIText"))
        {
            UITexts.Add(obj.GetComponent<Text>());
        }

        lives = GameObject.FindGameObjectWithTag("CanvasHUD").transform.GetChild(0);
        music = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BackgroundMusic>();
        StartCoroutine("ReadySetGo");
    }

    public void IncreaseScore(int num)
    {
        totalScore.IncreaseScore(num);
    }

    public void DecrementLife()
    {
        if (lives.childCount > 1)
        {
            Destroy(lives.GetChild(0).gameObject);
        }
    }

    public int GetLivesRemaining()
    {
        return lives.transform.childCount - 1;
    }

    public void PowerTimer()
    {
        powerTime = 10f;
        StartCoroutine("PoweredUpMusic");
    }

    public float GetPowerTime()
    {
        return powerTime;
    }

    public void GetPacStudentController(PacStudentController script)
    {
        pacStudent = script;
    }

    public void BubbleCount(GameObject bubbleCount)
    {
        this.bubbleCount = bubbleCount;
    }

    public void ChangeJellyfishCount(int num)
    {
        jellyfishCount += num;
        if (jellyfishCount == 4)
        {
            music.PlayOST((powerTime > 0f) ? 1 : 0);
            return;
        }
        music.PlayOST(2);
    }

    void Update()
    {
        if (powerTime == -2)
        {
            return;
        }

        if (GetBubbleCount() == 0 || GetLivesRemaining() == 0)
        {
            powerTime = -2;
            StartCoroutine("GameOver");
        }

        foreach (Text UIText in UITexts)
        {
            switch (UIText.name)
            {
                case "Score":
                    UIText.text = "Score:\n" + totalScore.Score;
                    break;

                case "Timer":
                    if (powerTime == -1)
                    {
                        continue;
                    }

                    ms += Time.deltaTime;
                    secs += (int)ms;
                    ms -= (int)ms;

                    mins += (secs == 60) ? 1 : 0;
                    secs = (secs == 60) ? 0 : secs;

                    UIText.text =
                        "Time:\n" +
                        ((mins < 10) ? "0" : "") + mins +
                        ":" + ((secs < 10) ? "0" : "") + secs +
                        ":" + ((ms * 100 < 10) ? "0" : "") + (int)(ms * 100);
                    break;

                case "GhostTimer":
                    if (powerTime > 0f)
                    {
                        powerTime -= Time.deltaTime;
                        UIText.text = "Power Time:\n" + ((int)powerTime + 1);
                    } else
                    {
                        UIText.text = "";
                    }
                    
                    break;
            }
        }
    }
    private int GetBubbleCount()
    {
        if (bubbleCount != null)
        {
            return bubbleCount.transform.childCount;
        }
        return 1;
    }

    private IEnumerator PoweredUpMusic()
    {
        music.PlayOST(1);
        while (powerTime > 0f)
        {
            yield return null;
        }
        if (!(music.BGM.clip == music.OST[2]))
        {
            music.PlayOST(0);
        }
    }

    private IEnumerator ReadySetGo()
    {
        float time = 4f;
        Text show = Instantiate(announcer,
            GameObject.FindGameObjectWithTag("CanvasHUD")
            .transform).GetComponent<Text>();

        string[] text = { "Go!", "1", "2", "3" };

        while (time > 0f)
        {
            time -= Time.deltaTime;
            show.text = "" + text[(int)time];
            yield return null;
        }
        powerTime = 0;
        pacStudent.SetHitBox(true);
        music.PlayOST(0);
        Destroy(show.gameObject);
    }

    private IEnumerator GameOver()
    {
        pacStudent.SetHitBox(false);
        float time = 4f;
        Text show = Instantiate(announcer,
            GameObject.FindGameObjectWithTag("CanvasHUD")
            .transform).GetComponent<Text>();

        show.text = "Game Over";
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        SaveScore();

        GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<LevelManager>().ButtonActions("QuitButton");
    }

    private void SaveScore()
    {
        if (!PlayerPrefs.HasKey("totalScore.score") ||
            PlayerPrefs.GetInt("totalScore.score") < totalScore.Score)
        {
            PlayerPrefs.SetInt("totalScore.score", totalScore.Score);
            PlayerPrefs.SetInt("mins", mins);
            PlayerPrefs.SetInt("secs", secs);
            PlayerPrefs.SetFloat("ms", ms);
        }
        else if (PlayerPrefs.GetInt("totalScore.score") == totalScore.Score)
        {
            if (PlayerPrefs.GetInt("mins") > mins)
            {
                PlayerPrefs.SetInt("mins", mins);
                PlayerPrefs.SetInt("secs", secs);
                PlayerPrefs.SetFloat("ms", ms);
            }
            else if (PlayerPrefs.GetInt("secs") > secs)
            {
                PlayerPrefs.SetInt("secs", secs);
                PlayerPrefs.SetFloat("ms", ms);
            }
            else if (PlayerPrefs.GetInt("ms") > ms)
            {
                PlayerPrefs.SetFloat("ms", ms);
            }
        }
    }
}
