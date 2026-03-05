using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event Action<float> OnProgressChanged;

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private SO_FryingRecipe[] fryingRecipeSOArray;
    [SerializeField] private SO_BurningRecipe[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private SO_FryingRecipe fryingRecipeSO;
    private float burningTimer;
    private SO_BurningRecipe burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (!HasKitchenObject()) return;

        switch (state) {
            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(fryingTimer / fryingRecipeSO.fryingTimerMax);

                if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                    // ÉTAPE 1 : Transformer en CUIT
                    TransformKitchenObject(fryingRecipeSO.output);
                    
                    // ÉTAPE 2 : Changer l'état et charger la recette suivante
                    state = State.Fried;
                    burningTimer = 0f; // TRÈS IMPORTANT : Remise à zéro ici
                    burningRecipeSO = GetBurningRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                }
                break;

            case State.Fried:
                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(burningTimer / burningRecipeSO.burningTimerMax);

                if (burningTimer > burningRecipeSO.burningTimerMax) {
                    // ÉTAPE 3 : Transformer en BRÛLÉ
                    TransformKitchenObject(burningRecipeSO.output);
                    state = State.Burned;
                    OnProgressChanged?.Invoke(0f); // Cache la barre
                }
                break;
        }
    }

    public override void Interact() {
        if (!HasKitchenObject()) {
            // Le joueur veut poser quelque chose
            // Note: La logique de transfert d'objet doit être gérée ici ou dans ton PlayerController
            // comme nous l'avons fait pour les autres comptoirs.
        } else {
            // Le joueur reprend l'objet
            state = State.Idle;
            OnProgressChanged?.Invoke(0f);
        }
    }

    // Cette méthode remplace l'objet par sa version cuite/brûlée
    private void TransformKitchenObject(SO_KitchenObject outputSO) {
        KitchenObject item = GetKitchenObject();
        Destroy(item.gameObject);

        GameObject newObjectTransform = Instantiate(outputSO.prefab);
        KitchenObject newKitchenObject = newObjectTransform.GetComponent<KitchenObject>();
        
        newKitchenObject.SetKitchenObjectParent(GetSpawnPoint());
        SetKitchenObject(newKitchenObject);
    }

    // Les méthodes manquantes qui causaient tes erreurs :
    private SO_FryingRecipe GetFryingRecipeWithInput(SO_KitchenObject input) {
        foreach (SO_FryingRecipe recipe in fryingRecipeSOArray) {
            if (recipe.input == input) return recipe;
        }
        return null;
    }

    private SO_BurningRecipe GetBurningRecipeWithInput(SO_KitchenObject input) {
        foreach (SO_BurningRecipe recipe in burningRecipeSOArray) {
            if (recipe.input == input) return recipe;
        }
        return null;
    }

    // Utile pour lancer la cuisson quand on pose un objet
    public void StartFrying(SO_KitchenObject inputSO) {
        fryingRecipeSO = GetFryingRecipeWithInput(inputSO);

        if (fryingRecipeSO != null) {
            fryingTimer = 0f;
            state = State.Frying;
            // Optionnel : On force l'affichage de la barre à 0 immédiatement
            OnProgressChanged?.Invoke(0f); 
        }
    }
}