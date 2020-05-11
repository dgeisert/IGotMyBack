using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public void EndGame(bool victory = false)
    {
        Game.Instance.active = false;
        Time.timeScale = 0;
        gameObject.SetActive(true);
        scoreText.text = Game.Score.ToString("#,#");
        Leaderboard_SampleScript.Instance.PostScoreBttn();
    }

    public void Restart()
    {
        SceneChanger.LoadScene(Scenes.Game);
    }
    public void Menu()
    {
        SceneChanger.LoadScene(Scenes.MainMenu);
    }
}