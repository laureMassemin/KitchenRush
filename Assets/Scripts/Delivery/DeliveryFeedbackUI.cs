using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryFeedbackUI : MonoBehaviour {

    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private TextMeshProUGUI popupText; // Le texte (+10, -5...)
    [SerializeField] private Image redFlashImage; // L'image rouge en plein écran

    private float showTimer;

    private void Start() {
        deliveryManager.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        deliveryManager.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        deliveryManager.OnRecipeWrong += DeliveryManager_OnRecipeWrong;

        Hide();
    }

    private void DeliveryManager_OnRecipeCompleted() {
        ShowText("+10", Color.green);
    }

    private void DeliveryManager_OnRecipeFailed() {
        ShowText("-5\nTemps écoulé !", Color.red);
    }

    private void DeliveryManager_OnRecipeWrong() {
        ShowText("-2\nMauvais plat !", Color.red);
        redFlashImage.gameObject.SetActive(true); // Active le flash rouge
    }

    private void ShowText(string text, Color color) {
        popupText.text = text;
        popupText.color = color;
        popupText.gameObject.SetActive(true);
        showTimer = 1f; // Le texte reste visible 2 secondes
    }

    private void Hide() {
        popupText.gameObject.SetActive(false);
        redFlashImage.gameObject.SetActive(false);
    }

    private void Update() {
        if (showTimer > 0) {
            showTimer -= Time.deltaTime;
            if (showTimer <= 0) {
                Hide();
            }
        }
    }
}