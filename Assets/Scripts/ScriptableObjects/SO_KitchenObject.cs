using UnityEngine;

[CreateAssetMenu()]
public class SO_KitchenObject : ScriptableObject {
    public GameObject prefab; // Le modèle 3D de la tomate
    public Sprite icon;       // L'icône pour l'interface
    public string objectName; // Le nom "Tomate"
}