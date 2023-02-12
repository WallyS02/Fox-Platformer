using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_LEVELCOMPLETED,
    GS_GAME_OVER,
    GS_OPTIONS,
};

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_GAME;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text playerScoreText;
    private int score = 0;
    public Image[] keysTab;
    public Image[] heartTab;
    public int keysFound = 0;
    public int currentHearts = 3;
    public TMP_Text enemiesText;
    private int enemyScore = 0;
    private float timer = 0.0f;
    public TMP_Text timerText;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
	public Canvas optionsCanvas;
    public TMP_Text qualityText;
    public const string keyHighscore = "HighScoreLevel1";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            else InGame();
        }
    }

    void Awake()
    {
        instance = this;
        scoreText.text = "00";
        enemiesText.text = "00";
        timerText.text = "00:00";
        for (int i = 0; i < 3; i++)
        {
            keysTab[i].color = Color.grey;
        }
        for (int i = 0; i < 3; i++)
        {
            heartTab[i].enabled = true;
        }
        for (int i = 3; i < 6; i++)
        {
            heartTab[i].enabled = false;
        }

        if(!PlayerPrefs.HasKey(keyHighscore))
        {
            PlayerPrefs.SetInt(keyHighscore, 0);
        }

        InGame();
        UpdateQualityLabel();
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (currentGameState != GameState.GS_GAME)
        {
            inGameCanvas.enabled = false;
        }
        else
        {
            inGameCanvas.enabled = true;
        }

        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
		optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);

        if(newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "Level1")
            {
                int highScore = PlayerPrefs.GetInt(keyHighscore);
                if(highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighscore, highScore);
                }

                playerScoreText.text = "Twoj wynik = " + score;
                highScoreText.text = "Najlepszy wynik = " + highScore;
            }
        }
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void AddPoints(int points)
    {
        score += points;
        if (score < 10)
        {
            scoreText.text = ('0' + score.ToString());
        }
        else scoreText.text = score.ToString();
    }

    public void AddEnemies()
    {
        enemyScore++;
        if (enemyScore < 10)
        {
            enemiesText.text = ('0' + enemyScore.ToString());
        }
        else enemiesText.text = enemyScore.ToString();
    }

    public void AddKeys(int nr)
    {
        keysFound++;
        switch (nr)
        {
            case 0:
                keysTab[nr].color = Color.red;
                break;
            case 1:
                keysTab[nr].color = Color.green;
                break;
            case 2:
                keysTab[nr].color = Color.blue;
                break;
        }
    }

    public void AddHeart()
    {
        currentHearts++;
        heartTab[currentHearts - 1].enabled = true;
    }

    public void RemoveHeart()
    {
        heartTab[currentHearts - 1].enabled = false;
        currentHearts--;
    }

    public void OnResumeButtonClicked()
    {
		Time.timeScale = 1;
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

	public void OnOptionsButtonClicked() {
		Options();
	}

	public void Options() {
		Time.timeScale = 0;
		SetGameState(GameState.GS_OPTIONS);
	}

    private void UpdateQualityLabel()
    {
        qualityText.text = "Jakosc: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void OnQualityIncreaseClicked()
    {
        QualitySettings.IncreaseLevel();
        UpdateQualityLabel();
    }

    public void OnQualityDecreaseClicked()
    {
        QualitySettings.DecreaseLevel();
        UpdateQualityLabel();
    }

    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }
}
