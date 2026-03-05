// Dans SO_KitchenObject.cs
using UnityEngine;

[CreateAssetMenu()]
public class SO_KitchenObject : ScriptableObject {
    public GameObject prefab;
    public Sprite sprite;
    public string objectName;

    // Ajoute ces options pour l'assiette
    public GameObject plateVisualLowerPrefab; // Le visuel pour le bas (si applicable)
    public GameObject plateVisualUpperPrefab; // Le visuel pour le haut (si applicable)
}