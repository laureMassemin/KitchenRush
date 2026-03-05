using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        // Les abonnements resteront les mêmes
        deliveryManager.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        deliveryManager.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        UpdateVisual();
    }

    // Retire (object sender, System.EventArgs e)
    private void DeliveryManager_OnRecipeSpawned() {
        UpdateVisual();
    }

    // Retire (object sender, System.EventArgs e)
    private void DeliveryManager_OnRecipeCompleted() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        // On nettoie l'ancien visuel
        foreach (Transform child in container) {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        // On crée un nouveau visuel pour chaque recette en attente
        foreach (SO_Recipe recipeSO in deliveryManager.GetWaitingRecipeSOList()) {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}