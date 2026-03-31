using UnityEngine;
using UnityEngine.UI;

public class PlateIconsUI : MonoBehaviour {

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        // On cache le modèle d'icône au démarrage
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        // On écoute l'événement : "Un ingrédient a été ajouté à l'assiette"
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(SO_KitchenObject kitchenObjectSO) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        // 1. On nettoie les anciennes icônes (sauf le modèle de base)
        foreach (Transform child in transform) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // 2. On crée une icône pour chaque ingrédient présent sur l'assiette
        foreach (SO_KitchenObject kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            
            // 3. On applique l'image (Sprite) de ton SO à cette nouvelle icône
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}