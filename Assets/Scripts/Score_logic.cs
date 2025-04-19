using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Score_logic : MonoBehaviour
{
    public TextMeshProUGUI scoretext;
    public int score = 0;
    public int JackFrostID = 0;
    public int JackoLantern = 0;
    private void Start()
    {
        PlayerPrefs.GetInt("PlayerScore", score);
        PlayerPrefs.GetInt("FrostScore", JackFrostID);
        PlayerPrefs.GetInt("LanternScore", JackoLantern);
    }
    void Update()
    {
        scoretext.text = "Score: " + score.ToString() + "!";
    }
}
