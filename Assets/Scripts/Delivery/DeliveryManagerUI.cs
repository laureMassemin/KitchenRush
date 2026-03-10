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
        deliveryManager.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        deliveryManager.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        
        // NOUVEAU : On s'abonne à l'échec
        deliveryManager.OnRecipeFailed += DeliveryManager_OnRecipeFailed; 
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeFailed() {
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
        foreach (Transform child in container) {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        // On utilise RecipeOrder ici
        foreach (RecipeOrder recipeOrder in deliveryManager.GetWaitingRecipeList()) {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            // On donne la commande entière au script UI
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeOrder(recipeOrder);
        }
    }
}