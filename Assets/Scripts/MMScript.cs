using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MMScript : MonoBehaviour
{
    private int Level = 11;
    private bool[] completedLevels;
    private int Volume = 0;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;
    private AudioSource AS;
    public Transform[] Levels;

    void Start()
    {
        AS = this.GetComponent<AudioSource>();

        completedLevels = new bool[10];
        for (int i = 0; i < completedLevels.Length; i++)
        {
            completedLevels[i] = false;
        }

        gameData data = saveSystem.loadProgress();
        if (data != null)
        {
            Level = data.Level;
            volumeSlider.value = data.Volume;
            Volume = data.Volume;

            for (int i = 0; i < completedLevels.Length; i++)
            {
                completedLevels[i] = data.completedLevels[i];
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (completedLevels[i])
            {
                Levels[i].GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            }
            else
            {
                Levels[i].GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            }
        }
    }

    void Update()
    {
        Volume = (int)volumeSlider.value;
        volumeText.text = "VOLUME: " + Volume + "%";
        AS.volume = (float)Volume / 100.0f;
    }

    public void closeGame()
    {
        Application.Quit();
    }

    public void startGame(int l)
    {
        AS.Play();
        Level = l;
        saveSystem.saveProgress(Level, completedLevels, Volume);
        SceneManager.LoadScene("Game");
    }

    public void howToPlay()
    {
        AS.Play();
        saveSystem.saveProgress(Level, completedLevels, Volume);
        SceneManager.LoadScene("howToPlay");
    }

    private void OnApplicationQuit()
    {
        saveSystem.saveProgress(Level, completedLevels, Volume);
    }

    public void levelSelection()
    {
        AS.Play();
    }
}
