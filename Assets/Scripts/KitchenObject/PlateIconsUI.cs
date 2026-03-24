using UnityEngine;
using UnityEngine.UI;

public class PlateIconsUI : MonoBehaviour {
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Start() {
        // On écoute l'ajout d'ingrédient pour mettre à jour l'UI
        plateKitchenObject.OnIngredientAdded += (SO_KitchenObject so) => UpdateVisual();
        iconTemplate.gameObject.SetActive(false);
    }

    private void UpdateVisual() {
        foreach (Transform child in transform) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (SO_KitchenObject kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            // ICI : On utilise l'icône que tu as déjà configurée dans le SO !
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite; 
        }
    }
}