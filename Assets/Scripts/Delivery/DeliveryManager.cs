using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public event Action OnRecipeSpawned;
    public event Action OnRecipeCompleted;

    [SerializeField] private List<SO_Recipe> recipeListSO; // Toutes les recettes possibles du jeu
    private List<SO_Recipe> waitingRecipeSOList = new List<SO_Recipe>(); // Recettes en cours d'attente

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax) {
                // On choisit une recette au hasard
                SO_Recipe waitingRecipeSO = recipeListSO[UnityEngine.Random.Range(0, recipeListSO.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke();
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        // On parcourt les recettes en attente
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            SO_Recipe waitingRecipeSO = waitingRecipeSOList[i];

            // On vérifie si le nombre d'ingrédients correspond
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                bool plateContentsMatchesRecipe = true;

                // On vérifie chaque ingrédient de la recette
                foreach (SO_KitchenObject recipeIngredientSO in waitingRecipeSO.kitchenObjectSOList) {
                    bool ingredientFound = false;
                    foreach (SO_KitchenObject plateIngredientSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        if (plateIngredientSO == recipeIngredientSO) {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe) {
                    // La recette est correcte !
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke();
                    Debug.Log("Commande livrée avec succès !");
                    return;
                }
            }
        }
        // Aucune correspondance trouvée
        Debug.Log("Mauvaise recette !");
    }

    public List<SO_Recipe> GetWaitingRecipeSOList() => waitingRecipeSOList;
}