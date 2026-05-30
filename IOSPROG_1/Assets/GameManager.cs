using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    private bool _gameOver;
    private bool _gamePaused;

    [Header("Score")]
    private int _scoreCounter;
    private int _highScore;

    [Header("UI")]
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _gamePausedUI;



    [SerializeField] private TextMeshProUGUI _scoreText;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        InitializeGame();
    }

    public void CallGameOver()
    {
        if (_gameOver)
        {
            return;
        }

        _gameOver = true;

        PauseTime();

        _gameOverUI.SetActive(true);
    }

    public void IncrementScore(int upScore)
    {
        _scoreCounter += upScore;

        if (_scoreCounter > _highScore)
        {
            _highScore = _scoreCounter;

            PlayerPrefs.SetInt("HighScore", _highScore);

            PlayerPrefs.Save();
        }

        UpdateScoreUI();
    }

    #region button scripts
    public void BTN_PauseGame()
    {
        if (_gameOver || _gamePaused)
        {
            return;
        }

        _gamePaused = true;

        PauseTime();

        _gamePausedUI.SetActive(true);
    }

    public void BTN_ResumeGame()
    {
        if (_gameOver)
        {
            return;
        }

        _gamePaused = false;

        ResumeTime();

        _gamePausedUI.SetActive(false);
    }

    public void BTN_RetryGame()
    {
        ResumeTime();

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void BTN_ReturnToMainMenu()
    {
        ResumeTime();

        SceneManager.LoadScene("MainMenu");
    }

    #endregion
    private void InitializeGame()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);

        _scoreCounter = 0;

        _gameOver = false;
        _gamePaused = false;

        ResumeTime();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (_scoreText == null)
        {
            Debug.LogError("Score Text is NULL");

            return;
        }

        _scoreText.text = $"Score: {_scoreCounter}";
    }

    #region manipulate timescale 
    private void PauseTime()
    {
        Time.timeScale = 0f;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }
    #endregion
}