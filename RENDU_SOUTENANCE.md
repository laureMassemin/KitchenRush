# Document de soutenance — Projet Gaming S6
**Auteur·es :** Laure MASSEMIN
**Lien GitHub :** https://github.com/laureMassemin/KitchenRush
**Date :** 29 mai 2026

---

## 1. Résumé du jeu

### Mise en situation
Le joueur incarne un cuisinier dans une cuisine en temps limité, inspiré du jeu *Overcooked*. Avant de jouer, il choisit son personnage parmi plusieurs apparences disponibles depuis l'écran de sélection. Un tutoriel interactif lui apprend les commandes de base avant de lancer la partie.

### But du jeu
Préparer et livrer un maximum de recettes avant que le chronomètre n'atteigne zéro. Chaque recette demande de collecter les bons ingrédients, de les préparer (découper, cuire) et de les assembler sur une assiette avant de la déposer au comptoir de livraison.

### Conditions de gain et de perte

| Condition | Résultat |
| Score ≥ 30 points avant la fin du temps | ✅ Victoire — passage au niveau suivant |
| Temps écoulé avec score < 30 | ❌ Défaite — écran Game Over |

### Système de score
- **+10 points** : recette livrée correctement
- **−2 points** : mauvaise assiette livrée
- **−5 points** : recette expirée sans livraison

### Les deux niveaux
- **Niveau 1** — *Recettes de salade* : Salade simple, Salade avec tomates  
- **Niveau 2** — *Recettes de burgers* : Burger simple, Cheese Burger, Burger Complet

---

## 2. Description du mécanisme

### Déroulement d'une partie
1. Le chronomètre de **120 secondes** démarre dès le chargement de la scène
2. Toutes les **4 secondes**, une nouvelle commande apparaît dans la liste des recettes à préparer (maximum 4 en attente simultanément)
3. Chaque commande a son propre sous-timer : si elle n'est pas livrée à temps, elle expire (−5 points)
4. À la fin du temps, le jeu se met en pause (`Time.timeScale = 0`) et l'écran de fin s'affiche

### Préparation des ingrédients
- **Découpe** : poser l'ingrédient sur la planche à découper, appuyer plusieurs fois sur `F` jusqu'à remplir la barre de progression
- **Cuisson** : poser le steak sur la plaque de cuisson, attendre qu'il soit cuit (attention à ne pas le brûler)
- **Assemblage** : prendre une assiette puis y ajouter les ingrédients un par un avec `E`
- **Livraison** : apporter l'assiette complète au comptoir de livraison

### Feedback visuel
- Une **barre de progression** apparaît au-dessus de la planche à découper et de la plaque de cuisson
- Le **comptoir regardé** par le joueur pulse en vert pour indiquer qu'une interaction est possible
- Le **chronomètre passe en rouge** et clignote quand il reste moins de 20 secondes
- Des **sons** accompagnent chaque événement : réussite, erreur, expiration, alerte temps, découpe, game over

---

## 3. UX — Contrôles

### Touches du joueur

| Touche | Action |
|---|---|
| `Z` `Q` `S` `D` ou flèches directionnelles | Déplacer le personnage |
| `E` | **Interagir** : ramasser un objet, poser un objet, ajouter un ingrédient sur une assiette, livrer une assiette |
| `F` | **Couper** : appuyer plusieurs fois pour découper l'ingrédient sur la planche |

### Logique de la touche E (pas un combo mais une action contextuelle)
La touche `E` fait une action différente selon le contexte :

```
Joueur porte quelque chose :
  → regarde un comptoir vide          = pose l'objet
  → regarde une assiette              = ajoute l'ingrédient sur l'assiette
  → regarde le comptoir de livraison  = livre l'assiette
  → regarde la poubelle               = jette l'objet

Joueur ne porte rien :
  → regarde un comptoir avec un objet = ramasse l'objet
  → regarde un distributeur vide      = prend un ingrédient / une assiette
```

### Feedback visuel de sélection
Quand le joueur regarde un comptoir, celui-ci **pulse en vert** — c'est le signal visuel indiquant que `E` fera quelque chose sur ce comptoir précis.

---

## 4. Technique — Décisions d'implémentation et code clés

### 4.1 Détection du comptoir avec un Raycast

Le joueur envoie un rayon dans la direction de son dernier mouvement. Si le rayon touche un objet sur le layer `Counters`, ce comptoir devient le `selectedCounter`.

### 4.2 Machine à états dans StoveCounter

La plaque de cuisson utilise une machine à états explicite avec un `enum` pour gérer les transitions Cru → Cuit → Brûlé.

### 4.3 Validation d'une recette dans DeliveryManager

Pour valider une livraison, le code compare la liste d'ingrédients de l'assiette avec chaque recette en attente, en vérifiant que chaque ingrédient attendu est présent (sans tenir compte de l'ordre).

---

## 5. Architecture globale

### Structure des scènes

```
Menu
 └─ MainMenuUI.cs          → boutons Jouer / Quitter
 └─ CharacterSelectionMenu → sélection du personnage avant de jouer

TutorialScene
 └─ TutorialManager.cs     → séquence d'instructions pas à pas
 └─ TutorialOverlayUI.cs   → affichage des bulles de texte
 └─ TutorialDeliveryBridge → relie le tutoriel au système de livraison

GameScene / GameScene1  (même structure, recettes différentes)
 └─ GameManager            → timer, état de la partie, chargement de scène
 └─ DeliveryManager        → spawn des commandes, validation, score
 └─ PlayerController       → mouvements, interactions
 └─ AudioManager           → musique + SFX
 └─ Comptoirs (×N)         → BaseCounter et autres
 └─ UI                     → ScoreUI, GameTimerUI, DeliveryManagerUI, GameOverUI
```

### Hiérarchie des comptoirs

```
BaseCounter  (pose/prise d'un KitchenObject, point de spawn)
  ├─ CuttingCounter   (découpe en N coups, barre de progression)
  ├─ StoveCounter     (cuisson avec machine à états Idle/Frying/Fried/Burned)
  ├─ ContainerCounter (distributeur infini d'un ingrédient)
  ├─ PlatesCounter    (distributeur d'assiettes)
  ├─ DeliveryCounter  (valide et livre la recette)
  └─ TrashCounter     (supprime l'objet)
```

### Ce qui change entre le niveau 1 et le niveau 2

| Élément | Niveau 1 | Niveau 2 |
| Recettes disponibles | Salade simple, Salade + Tomates | Burger Simple, Cheese Burger, Burger Complet |
| Ingrédients à préparer | Salade (couper), Tomate (couper) | Steak (cuire), Tomate/Fromage/Salade (couper) |
| Complexité | 1–2 ingrédients | 3–5 ingrédients |
| Difficulté de cuisson | Aucune | Steak qui peut brûler |

La liste de recettes est assignée directement dans l'Inspector du `DeliveryManager` via des **ScriptableObjects** — changer de niveau = changer la liste, sans modifier le code.

### Design Patterns appliqués

**1. Singleton**  
Utilisé pour `GameManager`, `PlayerController` et `AudioManager` afin de les rendre accessibles depuis n'importe quel script sans référence directe dans l'Inspector.


**2. ScriptableObjects (base de données d'objets)**  
Tous les ingrédients (`SO_KitchenObject`), recettes (`SO_Recipe`) et recettes de transformation (`SO_CuttingRecipe`, `SO_FryingRecipe`, `SO_BurningRecipe`) sont des ScriptableObjects. Cela permet de :
- Créer de nouveaux ingrédients ou recettes sans toucher au code
- Partager les données entre scènes (les assets existent indépendamment des scènes)
- Configurer les paramètres (temps de cuisson, nombre de coups de couteau) directement dans l'Inspector

**3. Événements C# (`System.Action`)**  
Les composants communiquent par événements pour rester découplés. Exemple : `DeliveryManager` n'a aucune référence vers l'UI — il publie des événements, et l'UI s'y abonne.

Ce pattern permet d'ajouter de nouveaux abonnés (nouvelles UIs, nouvelles animations) sans modifier `DeliveryManager`.

**4. Interface `IHasProgress`**  
`CuttingCounter` et `StoveCounter` implémentent l'interface `IHasProgress` qui expose un événement `OnProgressChanged(float)`. La barre de progression (`ProgressBarUI`) s'abonne à cet événement sans savoir si elle est au-dessus d'une planche ou d'une plaque.

---

## 6. Diagramme de flux d'une recette

```
[Spawn recette]  ←──── DeliveryManager (toutes les 4s)
       │
       ▼
[Timer de recette démarre]
       │
       ├─── Timer > 0 ──► Joueur prépare les ingrédients
       │                        │
       │                   E = ramasser/poser
       │                   F = couper
       │                   StoveCounter = cuire
       │                        │
       │                   [Assiette complète]
       │                        │
       │                        ▼
       │                  [Livraison au comptoir]
       │                        │
       │           ┌────────────┴────────────┐
       │           │                         │
       │     Recette correcte          Mauvaise assiette
       │     → +10 points              → −2 points
       │     → OnRecipeCompleted       → OnRecipeWrong
       │
       └─── Timer = 0 ──► Recette expirée → −5 points → OnRecipeFailed
```

---

