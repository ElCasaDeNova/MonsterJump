MonsterJump is the third and final project I completed as part of the Unity Developer - Level 2 training at MakeYourGame over the course of 2 months. This 3D platformer highlights advanced mechanics involving jumps, gravity, AI, and controls.

The initial goal was to create a smooth and precise platformer, including:

✔ Optimized jump and gravity management

✔ A basic AI for enemies

✔ An ergonomic control system, suitable for both keyboard and controller

In addition to the requested objectives, I decided to explore creating 3D assets using Blender to better understand the work of 3D artists within a production pipeline.

There was therefore a lot of effort put into 3D modeling, organic animations, and gameplay. However, doing all of this in 2 months required some sacrifices. The level design and other aspects of the game were not pushed as far as I would have liked. However, you can check out my second project, Hang Knight, if you want to see a project more focused on level design.

Technologies used
🎮 Game Engine: Unity

💻 Programming Language: C#

🏗️ Game Mechanics: Rigidbody, gravity management, and applied forces

🎨 3D Modeling: Blender (Main character and enemies created by hand)

🎭 Animations: Imported from Mixamo

🎮 Basic AI: Used NavMeshAgent

Gameplay mechanics
🎮 Use of Unity's New Input System: Allows dynamic switching between keyboard and controller without additional configuration

🦘 Coyote Jump: Adds extra time after leaving a platform to jump, making the gameplay more forgiving

🎢 Dynamic Jump: The ascent is slower than the descent, simulating realistic gravity with increased force when falling

🤖 Simple AI: Player detection through Raycast and basic enemy behavior (Patrol and Attack)

🎭 Optimized Animations: Managed transitions using Animator Controller and BlendTree

🎨 Creation of custom 3D assets (on my own initiative) to better understand the work of 3D artists

Technical challenges
💡 Advanced jump and gravity management: Added Coyote Jump and Dynamic Jump to enhance gameplay. Introduced gameplay elements like unstable platforms that tilt according to the weight of the character.

💡 Improved controls: Instead of the old Input Manager, I set up Unity's New Input System for an instant transition between keyboard and controller, improving game feel and accessibility.

💡 Optimization: The area is divided into different parts with corridors separating them. Each zone generates and de-generates as the game progresses to ensure smooth gameplay running at a minimum of 60 fps according to the profiler. (The videos do not represent the current smoothness of the game)

💡 Creation of custom 3D models: Although the course didn't require 3D asset creation, I wanted to explore Blender to better understand the role of 3D artists and the technical constraints involved in integrating assets into Unity. This allowed me to learn how to optimize a model to integrate well into a game engine.

What I've learned
✅ Implemented Unity's New Input System for smoother and more accessible control

✅ Created my own 3D models using Blender (Basic understanding of modeling, retopology, texture painting, etc.) and animated them via Mixamo

✅ Managed interactions between the player and AI in a 3D environment

✅ Optimized controls with advanced jump mechanics

✅ Used advanced physics and gravity techniques for a natural gameplay experience

________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________

MonsterJump est le troisième et dernier projet que j’ai réalisé dans le cadre de la formation Développeur Unity - Niveau 2 de MakeYourGame en 2 mois. Ce jeu de plateforme 3D met en avant des mécaniques avancées de sauts, gravité, IA et maniabilité.

L’objectif initial était de créer un jeu de plateforme fluide et précis, intégrant :

✔ Une gestion optimisée des sauts et de la gravité

✔ Une IA basique pour les ennemis

✔ Un système de contrôles ergonomique, adapté au clavier et à la manette

En plus des objectifs demandés, j’ai décidé d’explorer la création d’assets 3D sous Blender afin de mieux comprendre le travail des 3D Artistes dans un pipeline de production.

Il y a donc eu de gros efforts sur la modélisation 3D, les animations organiques et le gameplay. Cependant faire tout cela en 2 mois requiert des sacrifices. Le Level Design ainsi que d'autres aspects du jeu n'ont pas été poussés aussi loin que je ne l'aurais souhaité. Vous pouvez cependant regarder mon projet n°2 Hang Knight si vous souhaitez voir un projet plus accès sur le Level Design.

Technologies utilisées
🎮 Moteur de jeu : Unity

💻 Langage de programmation : C#

🏗️ Mécanique du jeu : Rigidbody, gestion de la gravité et forces appliquées

🎨 Modélisation 3D : Blender (Personnage principal et ennemis créés à la main)

🎭 Animations : Import de Mixamo

🎮 IA de base : Utilisation du NavMeshAgent

Mécaniques de jeu
🎮 Utilisation du New Input System de Unity : Permet de passer dynamiquement du clavier à la manette sans configuration supplémentaire

🦘 Coyote Jump : Laisse un temps supplémentaire après avoir quitté une plateforme pour sauter, rendant le gameplay plus permissif

🎢 Dynamic Jump : La montée est plus lente que la descente, simulant une gravité réaliste avec force accrue lors de la chute

🤖 IA simpliste : Détection du joueur par Raycast et comportement basique des ennemis (Patrouille et Attaque)

🎭 Animations optimisées : Gestion des transitions via Animator Controller et BlendTree

🎨 Création d’assets 3D personnalisés (en initiative personnelle) pour mieux comprendre le travail des 3D Artistes

Défis techniques
💡 Gestion avancée du saut et de la gravité : Ajout du Coyote Jump et du Dynamic Jump pour dynamiser le Gameplay. Ajout d'élément de Gameplay, tel que les plateformes instables qui s'inclinent selon le poids du personnage joué.

💡 Amélioration de la maniabilité : Au lieu de l'ancien Input Manager, j’ai configuré Unity New Input System pour une transition instantanée entre clavier et manette, facilitant le game feel et l’accessibilité.

💡 Optimisation : La zone est séparée en différente partie avec des couloirs les séparents. Chaque zone se génère et dégénère au fil du jeu pour avoir un jeu fluide tournant au minimum à 60 fps d'après le profiler. (Les vidéos ne représentant pas la fluidité actuelle du jeu)

💡 Création de modèles 3D personnalisés : Même si la formation ne demandait pas la création d’assets 3D, j’ai voulu explorer Blender afin de mieux comprendre le rôle des 3D Artistes et les contraintes techniques liées à l’intégration dans Unity. Cela m’a permis de voir comment optimiser un modèle pour qu’il s’intègre bien dans un moteur de jeu.

Ce que j'ai appris
✅ Implémenter Unity New Input System pour un contrôle plus fluide et accessible

✅ Créer mes propres modèles 3D sous Blender (Compréhension basique de la modélisation, la retopologie, le texture painting, etc.) et les animers via Mixamo

✅ Gérer les interactions entre le joueur et l’IA dans un environnement 3D

✅ Optimiser la maniabilité avec des mécaniques avancées de saut

✅ Utiliser des techniques avancées de physique et gravité pour un gameplay naturel
