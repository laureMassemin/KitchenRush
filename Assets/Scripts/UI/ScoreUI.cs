using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        // On s'abonne à l'événement de changement de score
        deliveryManager.OnScoreChanged += DeliveryManager_OnScoreChanged;
        
        // On affiche le score initial (0) au lancement
        UpdateVisual();
    }

    private void DeliveryManager_OnScoreChanged() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        scoreText.text = "SCORE: " + deliveryManager.GetScore().ToString();
    }
}