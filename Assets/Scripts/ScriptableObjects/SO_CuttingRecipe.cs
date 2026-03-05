using UnityEngine;

[CreateAssetMenu()]
public class SO_CuttingRecipe : ScriptableObject {
    public SO_KitchenObject input;  // Ce qu'on pose (ex: Tomate)
    public SO_KitchenObject output; // Ce qu'on obtient (ex: Tomate Sliced)
    public int cuttingProgressMax;  // Nombre de coups de couteau nécessaires
}