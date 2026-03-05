using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress {
    [SerializeField] private SO_CuttingRecipe[] cuttingRecipeSOArray;
    private int cuttingProgress;
    public event Action<float> OnProgressChanged; // Événement de l'interface

    public override void Interact() {
        // Logique de pose/prise identique au BaseCounter ou personnalisée
        base.Interact();
    }

    // Nouvelle méthode pour l'action de couper
    public void Cut() {
        if (HasKitchenObject()) {
            SO_CuttingRecipe recipe = GetRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (recipe != null) {
                cuttingProgress++;
                // On envoie le pourcentage à la barre de progression
                float progressNormalized = (float)cuttingProgress / recipe.cuttingProgressMax;
                OnProgressChanged?.Invoke(progressNormalized);

                if (cuttingProgress >= recipe.cuttingProgressMax) {
                    // Transformation !
                    KitchenObject item = GetKitchenObject();
                    Destroy(item.gameObject); // On détruit la tomate entière
                    
                    GameObject sliced = Instantiate(recipe.output.prefab);
                    sliced.GetComponent<KitchenObject>().SetKitchenObjectParent(GetSpawnPoint());
                    SetKitchenObject(sliced.GetComponent<KitchenObject>());
                    OnProgressChanged?.Invoke(0f);
                }
            }
        }
    }

    private SO_CuttingRecipe GetRecipeWithInput(SO_KitchenObject input) {
        foreach (SO_CuttingRecipe recipe in cuttingRecipeSOArray) {
            if (recipe.input == input) return recipe;
        }
        return null;
    }
}