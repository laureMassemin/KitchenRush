using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Affiche le timer en HH:SS et colore la barre en rouge quand le temps est critique.
/// Attache ce script sur un objet UI qui contient un texte et (optionnel) une barre de progression.
/// </summary>
public class GameTimerUI : MonoBehaviour {

    [Header("Références UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    /// <summary>Barre de remplissage (Image en mode "Filled"). Optionnel.</summary>
    [SerializeField] private Image timerBarImage;

    [Header("Alerte temps critique")]
    [Tooltip("En dessous de ce nombre de secondes, le texte passe en rouge et clignote.")]
    [SerializeField] private float criticalTimeThreshold = 20f;
    [SerializeField] private Color normalColor   = Color.white;
    [SerializeField] private Color criticalColor = Color.red;

    private GameManager gameManager;
    private bool isCritical = false;

    // ─────────────────────────────────────────────────────────────
    private void Start() {
        gameManager = GameManager.Instance;

        if (gameManager == null) {
            Debug.LogWarning("GameTimerUI : GameManager.Instance introuvable !");
            return;
        }

        gameManager.OnGameOver += () => gameObject.SetActive(false);
    }

    private void Update() {
        if (gameManager == null || !gameManager.GameIsRunning) return;

        float t = gameManager.TimeRemaining;

        // ── Texte MM:SS ──────────────────────────────────────────
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // ── Barre de progression ──────────────────────────────────
        if (timerBarImage != null) {
            timerBarImage.fillAmount = t / gameManager.GameDuration;
        }

        // ── Couleur critique ──────────────────────────────────────
        bool nowCritical = t <= criticalTimeThreshold;
        if (nowCritical != isCritical) {
            isCritical = nowCritical;
            timerText.color = isCritical ? criticalColor : normalColor;
        }

        // ── Clignotement dans la phase critique ───────────────────
        if (isCritical) {
            float alpha = Mathf.PingPong(Time.time * 3f, 1f);
            Color c = timerText.color;
            c.a = Mathf.Clamp(alpha, 0.3f, 1f);
            timerText.color = c;
        }
    }
}