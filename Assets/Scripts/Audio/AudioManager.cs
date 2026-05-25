using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestionnaire audio central du jeu.
///
/// MISE EN PLACE :
///   1. Créez un GameObject vide "AudioManager" dans chaque scène.
///   2. Ajoutez ce script dessus.
///   3. Assignez vos clips audio dans l'Inspector (voir régions ci-dessous).
///   4. C'est tout ! Les sons se déclenchent automatiquement via les événements.
/// </summary>
public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    // ═══════════════════════════════════════════════════════════════════
    #region Inspector - Musique de fond

    [Header("── Musique de fond ─────────────────────────────────")]
    [Tooltip("Musique jouée dans la scène Menu")]
    [SerializeField] private AudioClip menuMusic;

    [Tooltip("Musique jouée pendant les niveaux de jeu")]
    [SerializeField] private AudioClip gameMusic;

    [Tooltip("Musique du tutoriel (optionnel, utilise gameMusic si absent)")]
    [SerializeField] private AudioClip tutorialMusic;

    [Range(0f, 1f)]
    [Tooltip("Volume de la musique de fond")]
    [SerializeField] private float musicVolume = 0.35f;

    #endregion
    // ═══════════════════════════════════════════════════════════════════
    #region Inspector - Sons des recettes

    [Header("── Sons des recettes ───────────────────────────────")]
    [Tooltip("Son joué quand une recette est livrée correctement ✅")]
    [SerializeField] private AudioClip recipeSuccessSound;

    [Tooltip("Son joué quand on livre une mauvaise assiette ❌")]
    [SerializeField] private AudioClip recipeWrongSound;

    [Tooltip("Son joué quand le temps d'une recette expire ⏰")]
    [SerializeField] private AudioClip recipeExpiredSound;

    #endregion
    // ═══════════════════════════════════════════════════════════════════
    #region Inspector - Sons de fin de partie

    [Header("── Sons de fin de partie ──────────────────────────")]
    [Tooltip("Son joué à la fin de la partie")]
    [SerializeField] private AudioClip gameOverSound;

    [Tooltip("Son d'alerte quand le temps restant est bas 🔔")]
    [SerializeField] private AudioClip timerWarningSound;

    [Tooltip("Nombre de secondes restantes pour déclencher l'alerte (défaut : 20)")]
    [SerializeField] private float timerWarningThreshold = 20f;

    #endregion
    // ═══════════════════════════════════════════════════════════════════
    #region Inspector - Sons de gameplay

    [Header("── Sons de gameplay ────────────────────────────────")]
    [Tooltip("Son joué à chaque coup de couteau")]
    [SerializeField] private AudioClip cuttingSound;

    [Tooltip("Son joué quand un ingrédient est posé sur l'assiette")]
    [SerializeField] private AudioClip ingredientAddedSound;

    [Tooltip("Son joué quand le joueur ramasse un objet")]
    [SerializeField] private AudioClip pickUpSound;

    [Range(0f, 1f)]
    [Tooltip("Volume des effets sonores")]
    [SerializeField] private float sfxVolume = 0.8f;

    #endregion
    // ═══════════════════════════════════════════════════════════════════

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private bool warningPlayed       = false;
    private bool gameOverSoundPlayed = false;

    // ─── Cycle de vie ──────────────────────────────────────────────────

    private void Awake() {
        // Singleton simple (pas de DontDestroyOnLoad : chaque scène a son propre AudioManager)
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Créer les deux AudioSources dynamiquement sur ce GameObject
        musicSource             = gameObject.AddComponent<AudioSource>();
        musicSource.loop        = true;
        musicSource.playOnAwake = false;
        musicSource.volume      = musicVolume;

        sfxSource             = gameObject.AddComponent<AudioSource>();
        sfxSource.loop        = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume      = sfxVolume;
    }

    private void Start() {
        // Lancer la musique adaptée à la scène courante
        PlayMusicForCurrentScene();

        // Attendre 1 frame pour que GameManager et DeliveryManager aient fini leur Awake/Start
        StartCoroutine(SubscribeNextFrame());
    }

    private void OnDestroy() {
        UnsubscribeFromGameEvents();
        Instance = null;
    }

    private void Update() {
        // Surveiller le timer pour l'alerte sonore quand le temps est bas
        if (!warningPlayed
            && GameManager.Instance != null
            && GameManager.Instance.GameIsRunning
            && GameManager.Instance.TimeRemaining <= timerWarningThreshold
            && GameManager.Instance.TimeRemaining > 0f)
        {
            warningPlayed = true;
            PlaySFX(timerWarningSound);
        }
    }

    // ─── Musique selon la scène ────────────────────────────────────────

    private void PlayMusicForCurrentScene() {
        string sceneName = SceneManager.GetActiveScene().name;

        AudioClip clip;
        if (sceneName == "Menu" || sceneName == "MainMenu")
            clip = menuMusic;
        else if (sceneName == "TutorialScene")
            clip = (tutorialMusic != null) ? tutorialMusic : gameMusic;
        else
            clip = gameMusic; // GameScene, GameScene1, etc.

        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    // ─── Abonnements aux événements ───────────────────────────────────

    private IEnumerator SubscribeNextFrame() {
        yield return null; // 1 frame d'attente
        SubscribeToGameEvents();
    }

    private void SubscribeToGameEvents() {
        // GameManager
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameOver += HandleGameOver;
        }

        // DeliveryManager (pas de static Instance dans ce script → FindObjectOfType)
        DeliveryManager dm = FindObjectOfType<DeliveryManager>();
        if (dm != null) {
            dm.OnRecipeCompleted += HandleRecipeCompleted;
            dm.OnRecipeFailed    += HandleRecipeExpired;
            dm.OnRecipeWrong     += HandleRecipeWrong;
        }
    }

    private void UnsubscribeFromGameEvents() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameOver -= HandleGameOver;
        }

        DeliveryManager dm = FindObjectOfType<DeliveryManager>();
        if (dm != null) {
            dm.OnRecipeCompleted -= HandleRecipeCompleted;
            dm.OnRecipeFailed    -= HandleRecipeExpired;
            dm.OnRecipeWrong     -= HandleRecipeWrong;
        }
    }

    // ─── Handlers d'événements ────────────────────────────────────────

    private void HandleRecipeCompleted() => PlaySFX(recipeSuccessSound);
    private void HandleRecipeExpired()   => PlaySFX(recipeExpiredSound);
    private void HandleRecipeWrong()     => PlaySFX(recipeWrongSound);

    private void HandleGameOver() {
        if (gameOverSoundPlayed) return;
        gameOverSoundPlayed = true;
        musicSource.Stop();             // Couper la musique à la fin de la partie
        PlaySFX(gameOverSound);
    }

    // ─── API publique (appelée depuis CuttingCounter, PlateKitchenObject…) ─

    /// <summary>Joue le son de découpe. Appeler depuis CuttingCounter.Cut()</summary>
    public void PlayCuttingSound()    => PlaySFX(cuttingSound);

    /// <summary>Joue le son d'ajout d'ingrédient. Appeler depuis PlateKitchenObject.TryAddIngredient()</summary>
    public void PlayIngredientAdded() => PlaySFX(ingredientAddedSound);

    /// <summary>Joue le son de ramassage. Appeler depuis PlayerController si souhaité.</summary>
    public void PlayPickUpSound()     => PlaySFX(pickUpSound);

    // ─── Contrôle du volume (utile pour un futur menu Options) ────────

    public void SetMusicVolume(float v) {
        musicVolume        = Mathf.Clamp01(v);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float v) {
        sfxVolume = Mathf.Clamp01(v);
    }

    // ─── Utilitaire interne ────────────────────────────────────────────

    private void PlaySFX(AudioClip clip) {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
