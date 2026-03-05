using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_Recipe : ScriptableObject {
    public List<SO_KitchenObject> kitchenObjectSOList; // Liste des ingrédients (Pain, Steak, etc.)
    public string recipeName;
}