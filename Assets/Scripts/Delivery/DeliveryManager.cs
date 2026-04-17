using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeOrder {
    public SO_Recipe recipeSO;
    public float timer;
}

public class DeliveryManager : MonoBehaviour {

    public event Action OnRecipeSpawned;
    public event Action OnRecipeCompleted;
    public event Action OnRecipeFailed;
    public event Action OnRecipeWrong;
    public event Action OnAnyPlateDelivered; // Se déclenche quelle que soit l'assiette
    
    // NOUVEAU : Événement et variable pour le score
    public event Action OnScoreChanged; 
    private int score = 0;

    [SerializeField] private List<SO_Recipe> recipeListSO;
    private List<RecipeOrder> waitingRecipeList = new List<RecipeOrder>(); 

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeList.Count < waitingRecipesMax) {
                SO_Recipe waitingRecipeSO = recipeListSO[UnityEngine.Random.Range(0, recipeListSO.Count)];
                
                RecipeOrder newOrder = new RecipeOrder {
                    recipeSO = waitingRecipeSO,
                    timer = waitingRecipeSO.recipeTimerMax
                };
                waitingRecipeList.Add(newOrder);

                OnRecipeSpawned?.Invoke();
            }
        }

        for (int i = 0; i < waitingRecipeList.Count; i++) {
            waitingRecipeList[i].timer -= Time.deltaTime;
            if (waitingRecipeList[i].timer <= 0f) {
                
                // NOUVEAU : Malus de temps écoulé (-5 points)
                score -= 5;
                //if (score < 0) score = 0; // (Optionnel) Empêche le score de descendre sous 0
                OnScoreChanged?.Invoke(); // Met à jour l'UI

                waitingRecipeList.RemoveAt(i);
                OnRecipeFailed?.Invoke(); 
                i--; 
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeList.Count; i++) {
            RecipeOrder waitingRecipeOrder = waitingRecipeList[i];

            if (waitingRecipeOrder.recipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                bool plateContentsMatchesRecipe = true;
                
                foreach (SO_KitchenObject recipeIngredientSO in waitingRecipeOrder.recipeSO.kitchenObjectSOList) {
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
                    // NOUVEAU : Bonus de réussite (+10 points)
                    score += 10;
                    OnScoreChanged?.Invoke(); // Met à jour l'UI

                    waitingRecipeList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke();
                    return;
                }
            }
        }
        
        // NOUVEAU : Malus si on livre une mauvaise assiette (-2 points)
        score -= 2;
        if (score < 0) score = 0;
        OnScoreChanged?.Invoke();
        OnRecipeWrong?.Invoke();
        OnAnyPlateDelivered?.Invoke(); // AJOUTE ICI

        Debug.Log("Mauvaise recette !");

    }

    public List<RecipeOrder> GetWaitingRecipeList() => waitingRecipeList; 
    
    // NOUVEAU : Permet à l'UI de lire le score
    public int GetScore() => score; 
}