using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    
    // NOUVEAU : L'image de la barre de temps
    [SerializeField] private Image timerBarImage; 

    private RecipeOrder recipeOrder; // On garde en mémoire la commande

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeOrder(RecipeOrder order) {
        this.recipeOrder = order;
        recipeNameText.text = order.recipeSO.recipeName;

        foreach (Transform child in iconContainer) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (SO_KitchenObject kitchenObjectSO in order.recipeSO.kitchenObjectSOList) {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }

    private void Update() {
        // NOUVEAU : À chaque frame, on diminue la jauge (Fill Amount)
        if (recipeOrder != null && timerBarImage != null) {
            timerBarImage.fillAmount = recipeOrder.timer / recipeOrder.recipeSO.recipeTimerMax;
        }
    }
}