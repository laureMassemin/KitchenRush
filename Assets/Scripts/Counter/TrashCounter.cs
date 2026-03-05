using UnityEngine;

public class TrashCounter : BaseCounter {

    public override void Interact() {
        // On vérifie si le joueur porte quelque chose (via ton PlayerController)
        // Mais dans notre architecture simplifiée, on peut aussi laisser le 
        // PlayerController gérer la destruction.
        Debug.Log("Interaction avec la poubelle !");
    }
}