using UnityEngine;

/// <summary>
/// Teinte le comptoir en jaune et fait pulser sa couleur quand le joueur peut interagir.
/// Aucune configuration requise : ajouter ce script sur chaque comptoir.
/// </summary>
public class SelectedCounterVisual : MonoBehaviour {

    [Header("Couleur de surlignage")]
    [SerializeField] private Color highlightColor = new Color(0.3f, 1f, 0.6f);   // vert menthe

    [Header("Pulsation")]
    [Range(0.3f, 1f)]
    [SerializeField] private float pulseMin    = 0.55f; // luminosité minimale du pulse
    [Range(0.5f, 2f)]
    [SerializeField] private float pulseSpeed  = 3.5f;  // vitesse de pulsation

    // ─── Privé ─────────────────────────────────────────────────────
    private BaseCounter  baseCounter;
    private Renderer[]   renderers;
    private Color[][]    originalColors; // couleurs d'origine sauvegardées
    private bool         isHighlighted;

    // ─── Initialisation ────────────────────────────────────────────

    private void Awake() {
        baseCounter = GetComponent<BaseCounter>();
        renderers   = GetComponentsInChildren<Renderer>();

        // Mémorise les couleurs originales de chaque matériau
        originalColors = new Color[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++) {
            var mats = renderers[i].materials; // .materials = instances (n'affecte pas les autres)
            originalColors[i] = new Color[mats.Length];
            for (int j = 0; j < mats.Length; j++)
                originalColors[i][j] = mats[j].color;
        }
    }

    private void Start() {
        SetHighlight(false);

        if (PlayerController.Instance != null)
            PlayerController.Instance.OnSelectedCounterChanged += HandleSelectedCounterChanged;
    }

    private void OnDestroy() {
        // Restaure les couleurs originales avant destruction
        SetHighlight(false);

        if (PlayerController.Instance != null)
            PlayerController.Instance.OnSelectedCounterChanged -= HandleSelectedCounterChanged;
    }

    // ─── Update : animation de pulsation ──────────────────────────

    private void Update() {
        if (!isHighlighted) return;

        // Fait osciller la couleur entre la teinte normale et la teinte surlignée
        float t = pulseMin + (1f - pulseMin) * (0.5f + 0.5f * Mathf.Sin(Time.time * pulseSpeed));
        Color pulsed = Color.Lerp(Color.white, highlightColor, t);

        for (int i = 0; i < renderers.Length; i++) {
            if (renderers[i] == null) continue;
            var mats = renderers[i].materials;
            foreach (var mat in mats)
                mat.color = pulsed * originalColors[i][0];
        }
    }

    // ─── Réaction à l'événement ────────────────────────────────────

    private void HandleSelectedCounterChanged(BaseCounter selected) {
        SetHighlight(selected == baseCounter);
    }

    private void SetHighlight(bool active) {
        isHighlighted = active;

        if (!active) {
            // Restaure les couleurs d'origine
            for (int i = 0; i < renderers.Length; i++) {
                if (renderers[i] == null) continue;
                var mats = renderers[i].materials;
                for (int j = 0; j < mats.Length; j++)
                    mats[j].color = originalColors[i][j];
            }
        }
    }
}
