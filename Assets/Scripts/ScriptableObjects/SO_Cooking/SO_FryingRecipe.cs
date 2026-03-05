using UnityEngine;

[CreateAssetMenu()]
public class SO_FryingRecipe : ScriptableObject {
    public SO_KitchenObject input;
    public SO_KitchenObject output;
    public float fryingTimerMax; // Assure-toi que c'est en minuscule ici
}