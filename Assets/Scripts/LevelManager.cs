using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private List<Button> buttons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        FindButtons();
        AddButtonAction("Level 1");
    }

    private void AddButtonAction(string buttonName)
    {
        buttons.Find(foundButton => foundButton.name.Equals(buttonName)).
            onClick.AddListener(delegate { ButtonActions(buttonName); });
    }

    public void ButtonActions(string buttonNames)
    {
        switch(buttonNames)
        {
            case "Level 1":
                StartCoroutine("LoadingScreen", "PacStudent - Level 1");
                break;

            case "QuitButton":
                StartCoroutine("LoadingScreen", "StartScene");
                break;

            default:
                Debug.Log("This button does nothing.");
                break;
        }
    }

    private void FindButtons()
    {
        buttons.Clear();
        foreach (GameObject foundButton in GameObject.FindGameObjectsWithTag("Button"))
        {
            buttons.Add(foundButton.GetComponent<Button>());
        }
    }

    private IEnumerator LoadingScreen(string newSceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(newSceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        FindButtons();
        AfterLoading(newSceneName);
    }

    private void AfterLoading(string newSceneName)
    {
        switch(newSceneName)
        {
            case "StartScene":
                Destroy(gameObject);
                break;

            case "PacStudent - Level 1":
                AddButtonAction("QuitButton");
                break;

            default:
                break;
        }
    }
}

