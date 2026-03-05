using UnityEngine;

[CreateAssetMenu()]
public class SO_BurningRecipe : ScriptableObject {
    public SO_KitchenObject input;
    public SO_KitchenObject output;
    public float burningTimerMax; // Assure-toi que c'est en minuscule ici
}