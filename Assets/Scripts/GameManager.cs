using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int moveSpeed;

    private int _score = 0;
    private int _time = 0;
    private int _asteroids = 0;

    private bool isGameStarted;

    [SerializeField] private GameObject pressAnyKeyText;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text asteroidCountText;
    [SerializeField] private Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        PrintInGameTexts();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        StartGame();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveSpeed *= 2;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            moveSpeed /= 2;
        }
    }

    private void PrintInGameTexts()
    {
        asteroidCountText.text = $"Asteroids count: {_asteroids}";
        scoreText.text = $"Score: {_score}";
        timeText.text = $"Time: {_time}";
    }

    private void HideInGameTexts()
    {
        asteroidCountText.text = "";
        scoreText.text = "";
        timeText.text = "";
    }

    private void StartGame()
    {
        if (!isGameStarted && Input.anyKey)
        {
            isGameStarted = true;

            Time.timeScale = 1;
            pressAnyKeyText.SetActive(false);

            StartCoroutine(TimeCoroutine());
        }
    }

    public void GameOver()
    {
        moveSpeed = 0;
        StopAllCoroutines();
        HideInGameTexts();
        gameOverPanel.SetActive(true);

        if (_score > PlayerPrefs.GetInt("BestScore"))
        {
            gameOverPanel.GetComponentsInChildren<Text>()[4].text = $"New record! {_score}";
            PlayerPrefs.SetInt("BestScore", _score);
        }

        gameOverPanel.GetComponentsInChildren<Text>()[0].text = $"Score: {_score}";
        gameOverPanel.GetComponentsInChildren<Text>()[1].text = $"Best Score: {PlayerPrefs.GetInt("BestScore")}";
        gameOverPanel.GetComponentsInChildren<Text>()[2].text = $"Session Time: {_time}";
        gameOverPanel.GetComponentsInChildren<Text>()[3].text = $"Asteroids count: {_asteroids}";
    }

    public void AddAsteroid()
    {
        _asteroids++;
        AddScore(5);
    }

    private void AddScore(int value = 1)
    {
        _score += value;
        PrintInGameTexts();
    }

    private IEnumerator TimeCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _time++;
            AddScore();
        }
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
