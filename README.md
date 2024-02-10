Coding Weeks 2023-2024
=

# Introduction
Ce projet Unity en C# est un jeu arcade platformer 2D.

L'objectif du joueur est de passer par la porte du dernier _(2ème)_ niveau. Pour ouvrir les portes de chaque niveau, qui mènent au niveau suivant, le joueur doit tuer un certain nombre d'IA qui se trouvent sur la carte.

# Sommaire :

1. MVPs / Feuille de route avec répartition des rôles
2. MVC 
3. Cas d’usage
4. Description des fonctionnalités (à expliciter)	
5. Instructions d'installation et de lancement du jeu	



# 1.MVPs / Feuille de route 

Voici comment nous avons hiérarchisé la construction de nos MVPs :

➤ MVP1 : générer un terrain (map) en 2D de manière procédurale. Nous souhaitons aussi intégrer des éléments d’ambiance à notre map (arbre, cavernes, creux, dénivelés…) répartis de manière semi-aléatoire sur la map. Cela permet de construire l’ambiance globale de notre jeu, tout en lui apportant l’intérêt majeur d’être attractif car à chaque nouvelle partie, la map sera différente. **(Erwan / Martin / Lucas)**

➤ MVP2 : Le personnage principal doit apparaître sur le terrain. A l’issue de cette étape, l’utilisateur sera capable de contrôler les déplacements du joueur grâce aux touches du clavier. Seul le déplacement du personnage sera compris dans cette étape (translation à droite, translation à gauche et saut vertical). Les touches assignées seront la barre espace pour le saut et les touches directionnelles pour les déplacements verticaux. **(Hammam / Yani)**

➤ MVP3 : La caméra suivra désormais le déplacement du personnage sur la map. **(Lucas / Erwan)**

➤ MVP4 : On fera apparaître automatiquement le personnage principal à l’extrémité gauche de la map. La quête principale du joueur prendra forme lors de cette étape. Une porte à l'extrémité droite sera créée, elle correspondra à l'endroit où doit se rendre notre personnage. Une fois la porte franchie, le niveau est gagné. Nous projetons de créer plusieurs niveaux dans une MVP ultérieure. Ainsi, la porte sera aussi le moyen d’accéder aux niveaux suivants. **(Erwan / Tristan)**

➤ MVP5 : Nous ajouterons ensuite deux types d’ennemis. Au sol, une catégorie de monstre suivra le joueur dans une zone de détection définie par l’utilisateur. Lorsque ce monstre sera bloqué par un bloc de la map, il devra être en capacité de sauter par lui-même pour franchir cet obstacle. Dans les airs, un monstre volant suivra le joueur en continu mais à une vitesse de déplacement inférieure à ce dernier.
Lors de la création de ces ennemis, nous esquisserons donc les premières interactions des monstres avec notre personnage, le but étant que ces derniers détectent et attaquent le personnage principal. La prise en compte des pertes de points de vie du joueur et des monstres aura lieu dans une MVP ultérieure. **(Tristan / Hammam / Martin)**

➤ MVP6 : Nous créerons une fonctionnalité qui remplira le rôle de “Manager’, et qui permettra de faire apparaître un nombre de monstres prédéfini par l’utilisateur sur la map. **(Lucas / Tristan)**

➤ MVP7 : Nous ajoutons ensuite les interactions complémentaires entre notre personnage et les ennemis.  Lors de cette phase, le joueur sera muni d’une barre de vie. Il perdra alors des points de vie lors d’une collision avec un des monstres présentés en MVP4. Une fois touchée, le personnage aura une phase d’immunité d’une durée modulable par l’utilisateur. Cette invincibilité sera caractérisée par un clignotement du personnage. **(Tristan / Martin)**

➤ MVP8 : Dans cette phase, nous donnerons à notre personnage la capacité d’infliger des dégâts aux ennemis. Pour ce faire, nous créerons des armes qui pourront être assignées au personnage principal grâce au choix de l'utilisateur (avec possibilité d’intervertir les armes). Par l’intermédiaire de ces dernières, le personnage pourra combattre les ennemis. Ainsi la quête du personnage sera complétée : en plus de devoir atteindre la porte, ce dernier devra tuer un certain nombre d’ennemis pour que la porte vers le niveau suivant s’ouvre. **(Erwan / Yani)**

➤ MVP9 : Dans cette phase, nous donnerons au joueur la possibilité de miner des blocs de terre à l’aide d’une pioche.  **(Erwan / Yani)**

➤ MVP10 : Lorsque le niveau de vie d’un personnage (joueur ou ennemi) deviendra nul, ce dernier sera automatiquement détruit. **(Martin / Hammam / Tristan)**

➤ MVP11 : Nous ajouterons ensuite l'interface d’interaction entre le jeu et l’utilisateur. Cela comprend : 
Menu principal (Play, Credits, Touches du jeu, Quit)
Pendant la partie, l’affichage du nombre restant de monstres à tuer pour que la porte s’ouvre
L’affichage d’un message “Niveau réussi” ou “Perdu” avec possibilité de revenir au menu principal le cas échéant 
**(Lucas / Martin)**

➤ MVP12 : Lors de cette étape, nous élaborons les différents niveaux comme souhaité dès la MVP4. Pour que la différence de niveaux soit significative, plusieurs paramètres seront amenés à changer. Premièrement, les environnements seront modifiés (neige, nuit, …). Deuxièmement, pour que la difficulté soit croissante d’un niveau à l’autre, le nombre de monstres augmentera. **(Erwan)**


➤ MVP13 : Nous assignerons des animations au personnage et aux ennemis : 
Mise en mouvement des personnages lors de leurs déplacements.
Lorsqu’un personnage n’a plus de vie, il est détruit en se décomposant
**(Yani / Martin / Erwan)**

➤ MVP14 : Lors de cette phase, nous développerons des effets sonores qui seront assignés à certaines étapes du jeu (grognement lors du déplacement des monstres, effet sonores lors de l’utilisation des armes, bruitages lorsqu'un personnage est touchée et un son différent lors d’une mort). **(Lucas / Hammam)**

➤ MVP15 : Création du fond avec effet parallaxe avec déplacement de nuages. Ajout de particules à la mort d’une entité. (Erwan / Yani)



# 2.MVC 

Un exemple d’architecture MVC pour le GameObject “Player” est disponible dans la présentation "LOTEREO_CW_gp_17.pdf" disponible sur ce repository.

Un MVC (Modèle Vue Contrôleur) décline la logique de jeu entre le modèle du jeu (modèle), sa représentation graphique (vue) et sa logique de contrôle (contrôleur). Dans le cas général, les spécificités de ces trois composants sont : 

➤ Le modèle : il garde les valeurs calculées par le contrôleur en mémoire;

➤ Le contrôleur : il réalise les calculs en temps réel des paramètres du jeu qu’il envoie au modèle. C’est lui qui régit tout le fonctionnement du jeu en prenant en compte les commandes imposées par l’utilisateur;

➤ La vue : elle est une représentation graphique du modèle adaptée à l’utilisateur et qui participe à l’aspect ludique du jeu. Cette représentation évolue au fur et à mesure de l’évolution des paramètres de jeu.
Dans notre cas concret, l’utilisation même de Unity nous impose une logique MVC. En effet, on assigne des scripts à des objets graphiques. Ces scripts prennent en compte des paramètres qui sont amenés à évoluer au cours de la partie. Le contrôleur gère ces calculs en temps réel. Ces objets participent à la représentation visuelle du jeu et interagissent les uns avec les autres selon les valeurs de paramètres enregistrées dans le modèle.



# 3.Cas d’usage

Le public ciblé par ce jeu est une population d’individus âgés de 16 à 50 ans. Aucune expérience en jeu vidéo n’est particulièrement requise. Ce jeu convient parfaitement aux personnes appréciant le Pixel art. 

Dans ce jeu, le joueur devra se challenger pour réussir à passer les niveaux et devenir le champion de LOTEREO. Ce jeu a pour ambition de divertir son utilisateur et le pousser dans ses retranchements face à des ennemis assoiffés de sang.

Le mécanisme de jeu est relativement simple. A chaque niveau le nombre de monstres augmente. Il demande alors plus de concentration et de persévérance pour gagner.

Ce jeu est complet et comprend des effets sonores et des animations participant à la beauté du jeu. Il est idéal pour ravir tous les sens de son utilisateur. 

LOTEREO possède une rejouabilité presque infinie grâce à une génération procédurale des maps. Le joueur ne sombrera jamais dans l’ennui.

Immiscez vous dans un univers fantastique et profitez d’une expérience unique !



# 4.Description des fonctionnalités

Déplacements du joueur : droite ←, gauche →, saut “esp”, tirer “E”, afficher le menu “échap” et changer d’outils/armes “tab”.

Nous pouvons régler grand nombre de caractéristiques de notre jeu grâce à des paramètres libres, à savoir : 

- La taille des caves, densité de végétation, densité d’eau et largeur de la carte ;
- La position sur la carte de la porte à atteindre ;
- Le nombre de monstres qui apparaissent sur la carte à chaque niveau ;
- La hauteur de saut du joueur, sa vitesse de déplacement, son temps d'invincibilité lorsqu’il est touché et son nombre de vie maximum ;
- La gravité imposée aux balles du fusil ;
- La vitesse de déplacement, la hauteur de saut, la distance de détection du joueur, le nombre de dégâts infligés au joueur et la probabilité de saut des montres terrestres ;
- La vitesse de déplacement et le nombre de dégâts infligés au joueur du monstre.



# 5.Instructions d'installation et de lancement du jeu

Vous disposez de 3 options pour lancer le jeu : 

➤ Jouer sur la version “Page web” en cliquant sur le lien suivant : https://play.unity.com/mg/other/webgl-6se

➤ Cloner le repository pour avoir la version exécutable, puis cliquer sur l’exécutable du fichier .exe qui se trouve dans le dossier “Jeu”.

➤ Cloner le repository, ouvrir le projet “2D tile map” sur la version “2002.3.13f1” de Unity, puis appuyer sur Play.
