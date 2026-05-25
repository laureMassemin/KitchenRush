# 🎵 Guide de mise en place de l'Audio

## Étape 1 — Trouver des fichiers audio gratuits

Téléchargez des sons gratuits sur ces sites (format .mp3 ou .wav) :

| Type de son | Mots-clés à chercher | Sites recommandés |
|---|---|---|
| Musique menu | "upbeat menu music loop" | freemusicarchive.org, pixabay.com |
| Musique jeu | "cooking game music loop" / "kitchen background music" | opengameart.org |
| Recette réussie ✅ | "success jingle", "level complete" | freesound.org |
| Mauvaise livraison ❌ | "wrong buzz", "error sound" | freesound.org |
| Recette expirée ⏰ | "time up bell", "alarm clock" | freesound.org |
| Alerte temps bas 🔔 | "tick tock", "clock ticking fast" | freesound.org |
| Fin de partie | "game over jingle" | freesound.org |
| Découpe couteau | "knife chopping", "cutting vegetables" | freesound.org |
| Ingrédient sur assiette | "plate clink", "food drop" | freesound.org |
| Ramasser objet | "item pickup", "whoosh" | freesound.org |

Placez tous les fichiers audio dans ce dossier : `Assets/Audio/`

---

## Étape 2 — Créer le GameObject AudioManager dans chaque scène

Dans **chaque scène** (GameScene, GameScene1, Menu, TutorialScene) :

1. Ouvrez la scène dans Unity
2. Dans la Hiérarchie : **clic droit → Create Empty**
3. Renommez le GameObject : `AudioManager`
4. Dans l'Inspector, cliquez **Add Component**
5. Cherchez et ajoutez : `AudioManager`

---

## Étape 3 — Assigner les clips dans l'Inspector

Sélectionnez le GameObject `AudioManager` dans chaque scène.
Dans l'Inspector, glissez vos fichiers audio depuis le dossier `Assets/Audio/` :

### Dans la scène `Menu` :
| Champ | Clip à assigner |
|---|---|
| Menu Music | votre musique de menu |
| (les autres champs peuvent rester vides) | |

### Dans les scènes `GameScene` et `GameScene1` :
| Champ | Clip à assigner |
|---|---|
| Game Music | musique de jeu |
| Recipe Success Sound | son succès |
| Recipe Wrong Sound | son erreur |
| Recipe Expired Sound | son expiration |
| Game Over Sound | son fin de partie |
| Timer Warning Sound | son alerte temps |
| Cutting Sound | son découpe |
| Ingredient Added Sound | son ingrédient ajouté |
| Pick Up Sound | son ramassage (optionnel) |

### Dans la scène `TutorialScene` :
| Champ | Clip à assigner |
|---|---|
| Game Music | musique de jeu (utilisée si Tutorial Music est vide) |
| Tutorial Music | musique tutoriel (optionnel) |
| Cutting Sound | son découpe |
| Ingredient Added Sound | son ingrédient ajouté |

---

## Étape 4 — Régler les volumes

Dans l'Inspector du `AudioManager` :
- **Music Volume** : 0.35 recommandé (musique pas trop forte)
- **SFX Volume** : 0.8 recommandé
- **Timer Warning Threshold** : 20 (alerte quand il reste 20 secondes)

---

## 🔊 Sons déclenchés automatiquement

| Événement | Son joué |
|---|---|
| Recette livrée correctement | `recipeSuccessSound` |
| Mauvaise assiette livrée | `recipeWrongSound` |
| Recette expirée (temps écoulé) | `recipeExpiredSound` |
| Fin de partie (timer à 0) | `gameOverSound` + musique stoppée |
| ≤ 20 secondes restantes | `timerWarningSound` (une seule fois) |
| Coup de couteau | `cuttingSound` |
| Ingrédient posé sur l'assiette | `ingredientAddedSound` |

---

## 💡 Conseils

- Préférez des sons **courts** pour les SFX (< 2 secondes)
- La musique de fond doit être en **boucle** (loop seamless)
- Si un champ est laissé vide, aucun son n'est joué (pas d'erreur)
- Vous pouvez régler les volumes différemment par scène
