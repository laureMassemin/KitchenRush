using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    // On utilise LateUpdate plutôt que Update. 
    // Ainsi, on s'assure que la caméra a fini de bouger avant d'orienter l'UI.
    private void LateUpdate() {
        // Aligne parfaitement la direction de cet objet avec la direction de la caméra.
        // C'est parfait pour l'UI car cela évite que les images soient inversées (effet miroir).
        transform.forward = Camera.main.transform.forward;
    }
}