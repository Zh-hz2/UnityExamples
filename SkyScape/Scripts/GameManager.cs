using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // ✅ 引入 TextMeshPro 命名空间
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public float baseSurvivalTime = 30f; // 第一关存活时间
    private float survivalTime;
    private float timer;
    private int level = 1;
    private bool isGameRunning = false;

    [Header("UI Elements")]
    public GameObject startPanel;     // ✅ 开始界面
    public GameObject gameOverPanel;  // ✅ 游戏失败界面
    public GameObject winPanel;       // ✅ 游戏胜利界面
    public TextMeshProUGUI timerText; // ✅ 计时器
    public TextMeshProUGUI levelText; // ✅ 关卡信息
    public Button startButton;        // ✅ "Start Game" 按钮
    public Button continueButton;     // ✅ "Continue Game" 按钮
    public Button restartButton;      // ✅ "Restart" 按钮
    public Button quitButton;         // ✅ "Quit" 按钮
    public Button nextLevelButton;    // ✅ "Next Level" 按钮

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadGame();
        UpdateUI();

        // **确保 UI 变量绑定**
        if (startPanel == null || gameOverPanel == null || winPanel == null)
        {
            Debug.LogError("❌ UI elements are NOT assigned in GameManager! Please check Inspector.");
            return;
        }

        // **游戏开始时，只显示 StartPanel**
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        // **绑定按钮**
        startButton.onClick.AddListener(StartGame);
        continueButton.onClick.AddListener(ContinueGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
        nextLevelButton.onClick.AddListener(NextLevel); // ✅ 绑定 "Next Level" 按钮

        // **如果没有存档，隐藏 Continue 按钮**
        continueButton.gameObject.SetActive(PlayerPrefs.HasKey("Level"));
    }

    void Update()
    {
        if (isGameRunning)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Ceil(timer) + "s";

            if (timer <= 0)
            {
                WinGame(); // **确保时间结束时调用 `WinGame()`**
            }
        }
    }

    public void StartGame()
    {
        Debug.Log("✅ Start Game Clicked!");
        
        level = 1; // **游戏从第一关开始**
        survivalTime = baseSurvivalTime;
        timer = survivalTime;
        isGameRunning = true;

        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        UpdateUI();
    }

    public void ContinueGame()
    {
        Debug.Log("✅ Continue Game Clicked!");
        isGameRunning = true;
        startPanel.SetActive(false);
    }

    public void RestartGame()
    {
        Debug.Log("🔄 Restart Game Clicked!");
        level = 1;
        survivalTime = baseSurvivalTime;
        timer = survivalTime;
        isGameRunning = true;

        SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Debug.Log("💀 Game Over!");
        isGameRunning = false;

        // **立即启用 GameOverPanel**
        startPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void WinGame()
    {
        if (level == 5) // **第五关胜利，游戏结束**
        {
            Debug.Log("🏆 You Win! Game Completed!");
            isGameRunning = false;

            // **隐藏所有 UI，只显示 WinPanel**
            startPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            winPanel.SetActive(true);

            nextLevelButton.gameObject.SetActive(false); // **隐藏 "Next Level" 按钮**
        }
        else
        {
            Debug.Log($"🏆 Level {level} Complete! Moving to Level {level + 1}!");

            isGameRunning = false;
            winPanel.SetActive(true);
            gameOverPanel.SetActive(false);
            startPanel.SetActive(false);

            nextLevelButton.gameObject.SetActive(true); // **显示 "Next Level" 按钮**
        }
    }

    public void NextLevel()
    {
        Debug.Log($"⏭ Moving to Level {level + 1}");
        
        level++; // **升级到下一关**
        survivalTime = baseSurvivalTime + (level - 1) * 30f; // **每关增加 30s**
        timer = survivalTime;
        isGameRunning = true;

        SaveGame(); // **存档下一关的信息**
        
        // **隐藏 WinPanel，继续游戏**
        winPanel.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        
        UpdateUI();
    }

    public void QuitGame()
    {
        Debug.Log("❌ Quit Game!");
        Application.Quit();
    }

    void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time Left: " + Mathf.Ceil(timer) + "s";
        else
            Debug.LogError("❌ TimerText is NOT assigned in GameManager!");

        if (levelText != null)
            levelText.text = "Level: " + level;
        else
            Debug.LogError("❌ LevelText is NOT assigned in GameManager!");
    }

    void SaveGame()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetFloat("Timer", timer);
        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
            timer = PlayerPrefs.GetFloat("Timer");
            survivalTime = baseSurvivalTime + (level - 1) * 30f;
        }
        else
        {
            level = 1;
            timer = baseSurvivalTime;
        }
    }
}
