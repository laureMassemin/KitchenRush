using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Écran de fin de partie.
/// S'affiche automatiquement quand GameManager déclenche OnGameOver.
///
/// Setup Inspector :
///   - Glisse le panneau racine dans "gameOverPanel"
///   - Glisse le DeliveryManager pour lire le score
///   - Assigne les 3 boutons et les 2 textes
/// </summary>
public class GameOverUI : MonoBehaviour {

    [Header("Panneau principal (désactivé au démarrage)")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Références managers")]
    [SerializeField] private DeliveryManager deliveryManager;

    [Header("Textes UI")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI resultMessageText;

    [Header("Boutons")]
    [SerializeField] private Button nextLevelButton;   // visible seulement si score suffisant
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Visuels optionnels")]
    [Tooltip("Étoiles, confettis, ou autre feedback visuel pour succès.")]
    [SerializeField] private GameObject successEffect;
    [Tooltip("Feedback visuel pour l'échec.")]
    [SerializeField] private GameObject failEffect;

    // ─────────────────────────────────────────────────────────────
    private void Start() {
        // Panneau caché au départ
        gameOverPanel.SetActive(false);

        if (successEffect != null) successEffect.SetActive(false);
        if (failEffect    != null) failEffect.SetActive(false);

        // Écoute la fin de partie
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameOver += ShowGameOver;
        } else {
            Debug.LogWarning("GameOverUI : GameManager.Instance introuvable !");
        }

        // Boutons
        nextLevelButton.onClick.AddListener(() => GameManager.Instance.LoadNextLevel());
        restartButton  .onClick.AddListener(() => GameManager.Instance.RestartLevel());
        mainMenuButton .onClick.AddListener(() => GameManager.Instance.GoToMainMenu());
    }

    // ─── Affichage de l'écran de fin ─────────────────────────────
    private void ShowGameOver() {
        gameOverPanel.SetActive(true);

        int score       = deliveryManager.GetScore();
        int scoreToPass = GameManager.Instance.ScoreToPass;
        bool success    = score >= scoreToPass;

        // ── Score final ───────────────────────────────────────────
        finalScoreText.text = $"Score : {score}";

        // ── Message selon résultat ────────────────────────────────
        if (success) {
            resultMessageText.text = $"Bravo ! 🎉\nObjectif atteint ({scoreToPass} pts) !";
            resultMessageText.color = new Color(0.2f, 0.85f, 0.3f);

            nextLevelButton.gameObject.SetActive(true);

            if (successEffect != null) successEffect.SetActive(true);
        } else {
            resultMessageText.text = $"Dommage...\nIl fallait {scoreToPass} pts.";
            resultMessageText.color = new Color(0.9f, 0.25f, 0.2f);

            nextLevelButton.gameObject.SetActive(false);   // Bouton caché si échec

            if (failEffect != null) failEffect.SetActive(true);
        }
    }

    private void OnDestroy() {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameOver -= ShowGameOver;
    }
}