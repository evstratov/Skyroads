using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static event GameOverHandler OnGameOver;
    public delegate void GameOverHandler();

    [SerializeField] private GameObject pressAnyKeyText;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text asteroidCountText;
    [SerializeField] private Text timeText;

    [SerializeField] private SmoothFollow cameraScript;

    public int moveSpeed;
    private int _addScore = 1;
    private int _complScoreInc;

    private int _score = 0;
    private int _time = 0;
    private int _asteroids = 0;

    private bool isGameStarted;

    private float startDistance;
    private float startHeight;

    [Range(0f, 1f)] 
    [SerializeField] private float maxComplexity;
    [NonSerialized]public float gameComplexity = 1;
    // через какое количество очков увеличиваем сложность
    private const int IncComplexityScore = 20;

    void Start()
    {
        PrintInGameTexts();
        Time.timeScale = 0;
    }

    void Update()
    {
        StartGame();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            _addScore = 2;
            moveSpeed *= 2;

            startDistance = cameraScript.distance;
            startHeight = cameraScript.height;

            StartCoroutine(SpeedCameraOn());
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _addScore = 1;
            moveSpeed /= 2;
            
            StartCoroutine(SpeedCameraOff(startDistance, startHeight));
        }

        if (gameComplexity > maxComplexity && _score - _complScoreInc >= IncComplexityScore)
        {
            gameComplexity -= 0.1f;
            _complScoreInc = _score;
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
        OnGameOver?.Invoke();

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

    private void AddScore(int value)
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
            AddScore(_addScore);
        }
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SpeedCameraOn()
    {
        float toDistance = cameraScript.distance * 1.2f;
        float toHeight = cameraScript.height / 1.2f;

        while (cameraScript.distance < toDistance || cameraScript.height > toHeight)
        {
            cameraScript.distance += 1;
            cameraScript.height -= 0.5f;
            yield return null;
        }
    }

    private IEnumerator SpeedCameraOff(float toDistance, float toHeight)
    {
        while (cameraScript.distance > toDistance || cameraScript.height < toHeight)
        {
            cameraScript.distance -= 1;
            cameraScript.height += 0.5f;
            yield return null;
        }

        cameraScript.distance = toDistance;
        cameraScript.height = toHeight;
    }
}
