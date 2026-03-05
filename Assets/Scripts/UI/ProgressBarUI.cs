using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage; // L'image avec le "Fill Amount"

    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += (float progress) => {
            barImage.fillAmount = progress;

            // Cache la barre si c'est à 0 ou 1
            if (progress == 0f || progress == 1f) {
                gameObject.SetActive(false);
            } else {
                gameObject.SetActive(true);
            }
        };
        gameObject.SetActive(false); // Caché par défaut
    }
}