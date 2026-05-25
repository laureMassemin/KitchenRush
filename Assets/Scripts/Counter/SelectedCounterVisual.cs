using UnityEngine;

/// <summary>
/// Surligne visuellement le comptoir quand le joueur le regarde et peut interagir.
///
/// MISE EN PLACE (à faire sur chaque prefab de comptoir) :
///
///   Option A — Objets visuels enfants (recommandé, aspect personnalisable) :
///     1. Dans le prefab du comptoir, créez un enfant : clic droit → Create Empty,
///        nommez-le "SelectedVisual".
///     2. Ajoutez-lui un Quad (3D Object → Quad) en enfant, orienté à plat (rot X=90),
///        scale ~(1, 1, 1), positionné juste sous le comptoir (Y légèrement négatif).
///     3. Créez un matériau jaune semi-transparent et assignez-le à ce Quad.
///     4. Désactivez "SelectedVisual" par défaut (décochez dans l'Inspector).
///     5. Sur ce script, glissez "SelectedVisual" dans le tableau Visual Game Objects.
///
///   Option B — Surlignage par émission automatique (sans rien ajouter au prefab) :
///     Laissez Visual Game Objects vide.
///     Le script détecte automatiquement les MeshRenderers et applique une teinte
///     d'émission. Nécessite que les matériaux du comptoir aient l'émission activée
///     (Material → Emission → cocher la case dans l'Inspector).
/// </summary>
public class SelectedCounterVisual : MonoBehaviour {

    // ─── Inspector ─────────────────────────────────────────────────
    [Header("Comptoir ciblé (auto-détecté si vide)")]
    [SerializeField] private BaseCounter baseCounter;

    [Header("Option A — Objets visuels à activer/désactiver")]
    [Tooltip("Glissez ici les GameObjects enfants qui représentent le surlignage (ex: un quad jaune).")]
    [SerializeField] private GameObject[] visualGameObjects;

    [Header("Option B — Surlignage par émission (si Visual Game Objects est vide)")]
    [Tooltip("Couleur de l'émission quand le comptoir est sélectionné.")]
    [SerializeField] private Color highlightColor = new Color(1f, 0.85f, 0.15f); // Jaune chaud
    [Range(0f, 2f)]
    [Tooltip("Intensité de l'émission. 0 = invisible, 2 = très lumineux.")]
    [SerializeField] private float emissionIntensity = 0.45f;

    // ─── Privé ─────────────────────────────────────────────────────
    private MeshRenderer[] renderers;
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    // ─── Cycle de vie ──────────────────────────────────────────────

    private void Awake() {
        // Auto-détection du BaseCounter si non assigné
        if (baseCounter == null)
            baseCounter = GetComponent<BaseCounter>();

        // Récupérer tous les MeshRenderers du comptoir (pour Option B)
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void Start() {
        // S'abonner à l'événement du joueur
        if (PlayerController.Instance != null)
            PlayerController.Instance.OnSelectedCounterChanged += HandleSelectedCounterChanged;

        // Éteint le visuel au démarrage
        SetHighlight(false);
    }

    private void OnDestroy() {
        if (PlayerController.Instance != null)
            PlayerController.Instance.OnSelectedCounterChanged -= HandleSelectedCounterChanged;
    }

    // ─── Réaction à l'événement ────────────────────────────────────

    private void HandleSelectedCounterChanged(BaseCounter selected) {
        // Allume si C'EST ce comptoir qui est sélectionné, éteint sinon
        SetHighlight(selected == baseCounter);
    }

    // ─── Logique d'affichage ───────────────────────────────────────

    private void SetHighlight(bool active) {

        // ── Option A : objets visuels dédiés ──────────────────────
        if (visualGameObjects != null && visualGameObjects.Length > 0) {
            foreach (var go in visualGameObjects)
                if (go != null) go.SetActive(active);
            return;
        }

        // ── Option B : émission sur les MeshRenderers (fallback) ──
        Color emissionTarget = active
            ? highlightColor * emissionIntensity
            : Color.black;

        foreach (var r in renderers) {
            if (r == null) continue;

            // MaterialPropertyBlock = pas de nouvelle instance de matériau (performant)
            var mpb = new MaterialPropertyBlock();
            r.GetPropertyBlock(mpb);
            mpb.SetColor(EmissionColorID, emissionTarget);
            r.SetPropertyBlock(mpb);
        }
    }
}
