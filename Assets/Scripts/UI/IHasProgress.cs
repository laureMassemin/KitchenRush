using System;

public interface IHasProgress {
    // Événement qui envoie le pourcentage (entre 0 et 1)
    public event Action<float> OnProgressChanged;
}