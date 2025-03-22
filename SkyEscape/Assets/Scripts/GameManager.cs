using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public float baseSurvivalTime = 30f;
    private float survivalTime;
    private float timer;
    private int level = 1;
    private bool isGameRunning = false;

    [Header("UI Elements")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;

    public Button startButton;
    public Button continueButton;
    public Button restartButton;
    public Button quitButton;
    public Button nextLevelButton;
    public Button reviveButton;

    [Header("Game Rule Modifiers")]
    public float difficultyMultiplier = 1.2f;
    public int maxLevel = 5;
    public float reviveBonusTime = 5f;  // 复活时加的时间(可选)

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 初始暂停游戏
        Time.timeScale = 0f;
        isGameRunning = false;

        LoadGame();
        UpdateUI();

        // 只显示开始面板，其它隐藏
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        timerText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);

        // 绑定按钮
        startButton.onClick.AddListener(StartGame);
        continueButton.onClick.AddListener(ContinueGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
        // nextLevelButton.onClick.AddListener(NextLevel);
        reviveButton.onClick.AddListener(ReviveGame);

        // 如果有存档则显示“Continue”
        continueButton.gameObject.SetActive(PlayerPrefs.HasKey("Level"));
    }

    private void Update()
    {
        if (isGameRunning)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Ceil(timer) + "s";
            if (timer <= 0)
            {
                WinGame();
            }
        }
    }

    // 存活时间计算示例：指数增长
    private float CalculateSurvivalTime(int currentLevel)
    {
        return baseSurvivalTime * Mathf.Pow(difficultyMultiplier, currentLevel - 1);
    }

    public void StartGame()
    {
        level = 1;
        survivalTime = CalculateSurvivalTime(level);
        timer = survivalTime;
        isGameRunning = true;

        // 隐藏开始界面，显示游戏内UI
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        timerText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);

        Time.timeScale = 1f;
        UpdateUI();
    }

    public void ContinueGame()
    {
        // “继续游戏”只做读档后恢复
        isGameRunning = true;
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        timerText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        level = 1;
        survivalTime = CalculateSurvivalTime(level);
        timer = survivalTime;
        isGameRunning = true;

        SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        isGameRunning = false;
        Time.timeScale = 0f;

        timerText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        startPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Revive 只管复活，不再调用 ContinueGame()
    /// </summary>
    public void ReviveGame()
    {
        Debug.Log("Revive Game Clicked!");
        
        // 如果你想给玩家多一些时间：
        timer += reviveBonusTime;

        // 1. 隐藏 GameOverPanel
        gameOverPanel.SetActive(false);

        // 2. 解除玩家死亡状态
        if (PlayerController.instance != null)
        {
            PlayerController.instance.ResetState();
        }

        // 3. 恢复游戏
        isGameRunning = true;
        Time.timeScale = 1f;

        // 如果你想在UI上提示“你已复活”，可以在这里加文字提示
        // 例如: ShowReviveMessage();
    }

    public void WinGame()
    {
        if (level >= maxLevel)
        {
            isGameRunning = false;
            Time.timeScale = 0f;

            timerText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            startPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            winPanel.SetActive(true);

            nextLevelButton.gameObject.SetActive(false);
        }
        else
        {
            isGameRunning = false;
            Time.timeScale = 0f;

            timerText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            startPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            winPanel.SetActive(true);

            nextLevelButton.gameObject.SetActive(true);

        }
    }

public void NextLevel()
{
    level++;

    survivalTime = CalculateSurvivalTime(level);

    timer = survivalTime;
    isGameRunning = true;

    SaveGame();

    winPanel.SetActive(false);
    nextLevelButton.gameObject.SetActive(false);
    timerText.gameObject.SetActive(true);
    levelText.gameObject.SetActive(true);

    Time.timeScale = 1f;

    UpdateUI();
    // nextLevelButton.interactable = false;
}


    public void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time Left: " + Mathf.Ceil(timer) + "s";
        if (levelText != null)
            levelText.text = "Level: " + level;
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetFloat("Timer", timer);
        PlayerPrefs.Save();
    }

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
            timer = PlayerPrefs.GetFloat("Timer");
            survivalTime = CalculateSurvivalTime(level);
        }
        else
        {
            level = 1;
            timer = baseSurvivalTime;
            survivalTime = baseSurvivalTime;
        }
    }
}
