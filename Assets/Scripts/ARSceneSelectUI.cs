using UnityEngine;
using UnityEngine.SceneManagement;

public class ARSceneSelectUI : MonoBehaviour
{
    public Score_logic score;

    void Awake()
    {
        score = FindObjectOfType<Score_logic>();
    }

    public void LoadScene(string sceneName)
    {
        if(SceneManager.GetActiveScene().buildIndex == 2 && score != null)
        {
            PlayerPrefs.SetInt("PlayerScore", score.score);
            PlayerPrefs.SetInt("FrostScore", score.JackoLantern);
            PlayerPrefs.SetInt("LanternScore", score.JackFrostID);
            PlayerPrefs.SetInt("BlackScore", score.BlackFrostID);
            PlayerPrefs.Save();
        } 
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}