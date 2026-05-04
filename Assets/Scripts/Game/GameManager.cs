using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    // ─── Événements ───────────────────────────────────────────────
    public event Action OnGameStarted;
    public event Action OnGameOver;

    // ─── Paramètres Inspector ─────────────────────────────────────
    [Header("Temps")]
    [SerializeField] private float gameDuration = 120f; // secondes

    [Header("Score pour passer au niveau suivant")]
    [SerializeField] private int scoreToPassLevel = 30;

    [Header("Scènes")]
    [SerializeField] private string nextLevelSceneName = "Level2";
    [SerializeField] private string mainMenuSceneName  = "MainMenu";

    // ─── État interne ─────────────────────────────────────────────
    private float timeRemaining;
    private bool  gameIsRunning = false;

    // ─── Propriétés publiques ─────────────────────────────────────
    public float TimeRemaining   => timeRemaining;
    public float GameDuration    => gameDuration;
    public int   ScoreToPass     => scoreToPassLevel;
    public bool  GameIsRunning   => gameIsRunning;

    // ─────────────────────────────────────────────────────────────
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        timeRemaining = gameDuration;
        gameIsRunning = true;
        OnGameStarted?.Invoke();
    }

    private void Update() {
        if (!gameIsRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f) {
            timeRemaining = 0f;
            TriggerGameOver();
        }
    }

    // ─── Déclenche la fin de partie ───────────────────────────────
    private void TriggerGameOver() {
        gameIsRunning = false;
        Time.timeScale = 0f;        // Pause le jeu
        OnGameOver?.Invoke();
    }

    // ─── Appelé par les boutons de l'écran de fin ─────────────────
    public void LoadNextLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}