using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityRandom = UnityEngine.Random; //Sert � lever l'ambig�it� entre UnityEngine.Random et System.Random
using UnityEngine.SceneManagement;

public class ProceduralGeneration : MonoBehaviour
{


    [Header("World Size")]
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] private int noFold;

    [Header("Ground")]
    [SerializeField] private float seedGround;
    [SerializeField] int minDirtSpawnDistance, maxDirtSpawnDistance; //A FAIRE VARIER EN FONCTION DE HEIGHT
    [Range(0, 100)]
    [SerializeField] private float unzoom_ground;
    [SerializeField] private float amplitudePerlin; //Donne les variations de hauteurs du bruit Perlin
    [Header("Cave")]
    [SerializeField] private float seedCave;
    [Range(0, 100)]
    [SerializeField] private float freqCave;
    [Range(0, 100)]
    [SerializeField] private float unzoom_cave; //pourcentage de d�zoom. unzoom_cave augmente => d�zoome de Perlin

    [Header("Minerais")]
    //stone
    [SerializeField] private float seedStone;
    //[SerializeField] int minStoneSpawnDistance, maxStoneSpawnDistance;
    [Range(0, 100)]
    [SerializeField] private float freqStone; //freqStone augmente => plus de taches de pierre
    [Range(0, 100)]
    [SerializeField] private float unzoom_stone; //pourcentage de d�zoom. unzoom_stone augmente => d�zoome de Perlin
    //[SerializeField] private float centerStone; //A FAIRE VARIER EN FONCTION DE HEIGHT
    [Range(0, 100)]
    [SerializeField] private float percentHeightStone;
    [SerializeField] private float lambdaStone; //lambda augmente => transition franche Pierre/Terre (� partir de 0.5)
    //clay
    [SerializeField] private float seedClay;
    [SerializeField] private float freqClay;
    [Range(0, 100)]
    [SerializeField] private float unzoom_clay;
    //gold
    [SerializeField] private float seedGold;
    [SerializeField] private float freqGold;
    [Range(0, 100)]
    [SerializeField] private float unzoom_gold;
    [Range(0, 100)]
    [SerializeField] private float percentHeightGold;
    [SerializeField] private float lambdaGold;

    [Header("Vegetation")]
    //Herb and flower
    [SerializeField] private float seedHerb;
    [SerializeField] private float freqHerb;
    [SerializeField] private float freqFlower;
    [Range(0, 100)]
    [SerializeField] private float unzoom_herb;
    //Tree
    [SerializeField] private int minHeightTree;
    [SerializeField] private int maxHeightTree;
    [SerializeField] private float seedTree;
    [Range(0, 100)]
    [SerializeField] private float unzoom_tree;
    [Range(0, 100)]
    [SerializeField] private float chanceSpawnTree; //chanceSpawnTree augmente => plus on a de chance qu'un arbre apparaisse ind�pendamment de la carte de Perlin
    [Range(0, 1)]
    [SerializeField] private float chanceSpawnTreePerlin; //chanceSpawnTreePerlin augmente => plus on a de chance d'avoir des forets 
    [Range(0, 11)]
    [SerializeField] private int chanceSpawnBranches; //chanceSpawnBranches augmente => on a plein de branches
    //Liana
    [SerializeField] private int minHeightLiana;
    [SerializeField] private int maxHeightLiana;
    [SerializeField] private float seedLiana;
    [SerializeField] private float freqLiana;
    [Range(0, 100)]
    [SerializeField] private float unzoom_liana;

    [Header("Liquid")]
    [Range(0, 100)]
    [SerializeField] private float freqWater;

    [Header("TileMap & TileAtlass")]
    [SerializeField] public Tilemap topology;
    [SerializeField] public Tilemap bottomCave;
    [SerializeField] public Tilemap vegetation;
    [SerializeField] public Tilemap fold;
    [SerializeField] public Tilemap liquid;

    [SerializeField] public TileAtlas tileAtlas;

    //Autres variables
    private GameObject doorSpawn;


    public int[,] map, mapCaves, mapVegetation, mapFold, mapLiquid; //on cr�er une Array accessible dans tout le script de g�n�ration proc�durale

    void Start()
    {
        seedGround = UnityRandom.Range(-1000000, 1000000); //on d�finit la seed de notre monde (c'est l'axe y sur lequel on se balade pour le Bruit de Perlin du sol)
        seedCave = UnityRandom.Range(-1000000, 1000000);
        seedStone = UnityRandom.Range(-1000000, 1000000);
        seedClay = UnityRandom.Range(-1000000, 1000000);

        (map, mapCaves, mapVegetation, mapFold, mapLiquid) = GenerateArray(width, height);
        (map, mapCaves) = GenerateTerrain(map, mapCaves);
        mapVegetation = SpawnVegetation_Water(map, mapVegetation);
        //(map, mapCaves, mapVegetation) = FilterFloatingGrass(map, mapVegetation);
        //(map, mapCaves) = SpawnOre(map, mapCaves);
        SpawnDoor();
        RenderMap(map, mapCaves);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            seedGround = UnityRandom.Range(-1000000, 1000000); //on d�finit la seed de notre monde (c'est l'axe y sur lequel on se balade pour le Bruit de Perlin du sol)
            seedCave = UnityRandom.Range(-1000000, 1000000);
            seedStone = UnityRandom.Range(-1000000, 1000000);
            seedClay = UnityRandom.Range(-1000000, 1000000);

            topology.ClearAllTiles(); //on nettoie la map pr�c�dante
            bottomCave.ClearAllTiles(); //on nettoie la bottomCave pr�c�dante
            vegetation.ClearAllTiles(); //on nettoie la vegetation pr�c�dante
            liquid.ClearAllTiles(); //on nettoie la vegetation pr�c�dante
            (map, mapCaves, mapVegetation, mapFold, mapLiquid) = GenerateArray(width, height);
            (map, mapCaves) = GenerateTerrain(map, mapCaves);
            mapVegetation = SpawnVegetation_Water(map, mapVegetation);
            //(map, mapCaves, mapVegetation) = FilterFloatingGrass(map, mapVegetation);
            //(map, mapCaves) = SpawnOre(map, mapCaves);
            SpawnDoor();
            RenderMap(map, mapCaves);
        }
    }


    public void destroyTile(int x, int y, bool compteurBloc)
    {
        topology.SetTile(new Vector3Int(x, y, 0), null);

        if (compteurBloc) //si on a passé le bool compteurBloc à true, on incrémente nombreDeBlocsCasses
        {
            if (map[x, y] > 0)
            {
                GameObject souris = GameObject.FindWithTag("Souris"); //Player controller  : clique droit || clique gauche
                if (souris != null && souris.activeSelf)
                    souris.GetComponent<PlayerController>().nombreDeBlocsCasses++;
            }
        }

        map[x, y] = -1;
        if ((x - 1 > 0) && mapLiquid[x - 1, y] != 0)
            StartCoroutine(WaterFlow(x - 1, y, 0));
        if ((x + 1 < width) && mapLiquid[x + 1, y] != 0)
            StartCoroutine(WaterFlow(x + 1, y, 0));
        if ((y + 1 < height) && mapLiquid[x, y + 1] != 0)
            StartCoroutine(WaterFlow(x, y + 1, 0));

        //vegetation arbre fleur
        if ((y + 2 < height) && (mapVegetation[x, y + 1] != 0 || mapVegetation[x, y + 2] != 0 || mapVegetation[x, y + 3] != 0) && map[x, y + 1] <= 0)
        {
            if (mapVegetation[x + 1, y + 1] != 0) //check branches droite
            {
                mapVegetation[x + 1, y + 1] = 0;
                vegetation.SetTile(new Vector3Int(x + 1, y + 1, 0), null);
            }
            if (mapVegetation[x - 1, y + 1] != 0) //check branche gauche
            {
                mapVegetation[x - 1, y + 1] = 0;
                vegetation.SetTile(new Vector3Int(x - 1, y + 1, 0), null);
            }

            mapVegetation[x, y + 1] = 0;
            vegetation.SetTile(new Vector3Int(x, y + 1, 0), null);
            destroyTile(x, y + 1, false);
        }
        //vegetation liane
        if ((y - 1 > 0) && (mapVegetation[x, y - 1] != 0))
        {
            mapVegetation[x, y - 1] = 0;
            vegetation.SetTile(new Vector3Int(x, y - 1, 0), null);
            destroyTile(x, y - 1, false);
        }
    }



    public (int[,], int[,], int[,], int[,], int[,]) GenerateArray(int width, int height)
    {
        int[,] map = new int[width, height]; //on initialise map
        int[,] mapCaves = new int[width, height]; //on initialise mapCaves
        int[,] mapVegetation = new int[width, height]; //on initialise mapVegetation
        int[,] mapFold = new int[width, height]; //on initialise mapFold
        int[,] mapLiquid = new int[width, height]; //on initialise mapLiquid

        //on parcourt l'array
        for (int x = 0; x < width; x++) //les colonnes
        {
            for (int y = 0; y < height; y++) //les lignes
            {
                map[x, y] = 0;
                mapCaves[x, y] = 0;
                mapVegetation[x, y] = 0;
                mapFold[x, y] = 0;
                mapLiquid[x, y] = 0;
            }
        }
        return (map, mapCaves, mapVegetation, mapFold, mapLiquid);
    }


    //Indices bottom (caves)                 ->  bottomStone = -3, bottomDirt = -2, bottomDirt = -1

    //Indices ground (blocs)                 ->  Vide = 0, Grass = 1, Dirt = 2, Stone = 3, Clay = 4, Gold = 5, Door = 100

    //Indices vegetation (arbres & herbres)  ->  Tronc = 1, Feuille = 2, Herb = 3, Fleur1 = 4, Liane = 5

    //Indices Liquid (liquide)                 ->  eau = 1


    public (int[,], int[,]) GenerateTerrain(int[,] map, int[,] mapCaves)
    {
        int perlinHeight;
        //on parcourt l'array de nouveau pour la remplir
        for (int x = 0; x < width; x++)
        {
            int offset = 10; // définir un offset pour que le haut de la carte ne soit pas juste à finalDirtSpawnDistance+Perlinheight

            //hauteur du sol
            float xCoord = (float)x / 100 * unzoom_ground + seedGround;
            perlinHeight = Mathf.RoundToInt(amplitudePerlin * Mathf.PerlinNoise(xCoord, seedGround)) + offset; //1D

            //on obtient par colonne une hauteur de Dirt
            int minDirtSpawnDistanceTemp = height - perlinHeight - minDirtSpawnDistance;
            int maxDirtSpawnDistanceTemp = height - perlinHeight - maxDirtSpawnDistance;
            int finalDirtSpawnDistance = UnityRandom.Range(minDirtSpawnDistanceTemp, maxDirtSpawnDistanceTemp);


            for (int y = 0; y < height - perlinHeight; y++)
            {
                if (y <= height - perlinHeight - noFold)
                {
                    mapFold[x, y] = 1;
                }

                if (y <= finalDirtSpawnDistance)
                {
                    //Spawn dirt
                    map[x, y] = 2;
                }

                else
                {
                    //Spawn grass
                    map[x, y] = 1;
                }

                //Spawn Stone
                //Perlin Stone
                float perlin_stone = PerlinCustom(unzoom_stone, seedStone, x, y);
                //Seuil Spawn Stone
                float centerStone = height * percentHeightStone / 100;
                double sigmoidValueStone = SigmoidCentered((double)y, 1, centerStone, lambdaStone);
                double adjustedThresholdStone = freqStone + sigmoidValueStone * (1.0 - freqStone);
                //Creation 
                if (perlin_stone < adjustedThresholdStone)
                {
                    map[x, y] = 3;
                }

                //Spawn clay
                float perlin_clay = PerlinCustom(unzoom_clay, seedClay, x, y);
                if (perlin_clay < freqClay)
                {
                    map[x, y] = 4;
                }

                //Spawn Gold
                //Perlin Gold
                float perlin_gold = PerlinCustom(unzoom_gold, seedGold, x, y);
                //Seuil Spawn Gold
                float centerGold = height * percentHeightGold / 100;
                double sigmoidValueGold = SigmoidCentered((double)y, 1, centerGold, lambdaGold);
                double adjustedThresholdGold = freqGold + sigmoidValueGold * (1.0 - freqGold);
                if (perlin_gold < adjustedThresholdGold)
                {
                    map[x, y] = 5;
                }

                //Spawn cave
                float perlin_cave = PerlinCustom(unzoom_cave, seedCave, x, y);
                if (perlin_cave < freqCave)
                {
                    map[x, y] *= -1;
                }
            }
        }
        return (map, mapCaves);
    }


    public double SigmoidCentered(double x, double h, float xCenter, float lambda)
    //h: amplitude
    //xCenter: centre de la sigmoid
    //x: valeur en laquelle évaluée
    {
        return h / (1 + Math.Exp(-(x - xCenter) * lambda));
    }


    public float PerlinCustom(float unzoom, float seed, int x, int y)
    {
        float xCoord = (float)x * unzoom / 100 + seed;
        float yCoord = (float)y * unzoom / 100 + seed;
        return 100 * Mathf.PerlinNoise(xCoord, yCoord);
    }



    public void generateTree(int x, int y, int heightTree)
    {
        //le tronc
        for (int i = 0; i < heightTree; i++) //taille de l'arbre;
        {
            if (y + i < height) //conditions limites de bord de carte
                mapVegetation[x, y + i] = 1; //on ajoute un tronc

            if (UnityRandom.Range(0, 10) < chanceSpawnBranches)
            {
                int var_aleatoire = (int)UnityRandom.Range(0, 3);
                if (var_aleatoire == 0 && x + 1 < width && map[x + 1, y + i] <= 0 && mapVegetation[x + 1, y + i] == 0) //gauche ou droite  +  condition bord  de map + pas de blocs d�j� + pas de vegetation d�j�
                {
                    mapVegetation[x + 1, y + i] = 2; //on ajoute une branche � droite
                }
                else if (var_aleatoire == 1 && x - 1 > 0 && map[x - 1, y + i] <= 0 && mapVegetation[x - 1, y + i] == 0)
                {
                    mapVegetation[x - 1, y + i] = 2; //on ajoute une branche � gauche
                }
                else if (var_aleatoire == 2 && x - 1 > 0 && x + 1 < width && map[x - 1, y + i] <= 0 && map[x + 1, y + i] <= 0 && mapVegetation[x - 1, y + i] == 0 && mapVegetation[x + 1, y + i] == 0)
                {
                    mapVegetation[x - 1, y + i] = 2; //on ajoute une branche � gauche
                    mapVegetation[x + 1, y + i] = 2; //on ajoute une branche � droite
                }
            }
        }
        //ajout canopée
        if (y + heightTree + 2 < height)
        {
            mapVegetation[x, y + heightTree + 2] = 2;
        }
    }


    public void generateLiana(int x, int y)
    {
        int heightLiana = UnityRandom.Range(minHeightLiana, maxHeightLiana);
        for (int i = 0; i < heightLiana; i++) //taille de la liane
        {
            if (y - i > 0 && map[x, y - i] <= 0 && mapVegetation[x, y - i] == 0) //Limite carte + pas de bloc + poas de vegetation
            {
                mapVegetation[x, y - i] = 5;
            }
        }
    }


    public int[,] SpawnVegetation_Water(int[,] map, int[,] mapVegetation)
    {
        for (int x = 1; x < width; x++) //on commence � 1 car : 
        {
            for (int y = 0; y < height; y++)
            {
                // Spawn sources eau
                //if ((x > 0) && (y > 0) && (y + 1 < height) && map[x - 1, y + 1] > 0 && map[x - 1, y] > 0 && map[x, y - 1] > 0 || (y > 0) && (x + 1 < width) && (y + 1 < height) && map[x + 1, y + 1] > 0 && map[x + 1, y] > 0 && map[x, y - 1] > 0)
                if ((x > 0) && (x + 1 < width) && map[x - 1, y] > 0 && map[x, y] > 0 && map[x + 1, y] > 0)
                {
                    if (UnityRandom.Range(1, 100) < freqWater)
                    {
                        StartCoroutine(WaterFlow(x, y, 0));
                    }
                }

                // Spawn Vegetation
                if (map[x, y] == 1) //si c'est de la grass
                {
                    //Spawn herb and flowers
                    float perlin_herb = PerlinCustom(unzoom_herb, seedHerb, x, y);

                    //Spawn herb and flowers
                    if (y + 1 < height && (map[x, y + 1] <= 0) && mapVegetation[x, y + 1] != 1) //si il y a de la place pour l'herbe
                    {
                        if (perlin_herb < freqFlower)
                        {
                            mapVegetation[x, y + 1] = 4;
                        }
                        else if (perlin_herb < freqHerb)
                        {
                            mapVegetation[x, y + 1] = 3;
                        }
                    }

                    //Spawn liana
                    float perlin_liana = PerlinCustom(unzoom_liana, seedLiana, x, y);
                    if (y - 1 > 0 && (map[x, y - 1] <= 0) && mapVegetation[x, y - 1] == 0) //si il y a de la place pour l'herbe
                    {
                        if (perlin_liana < freqLiana)
                        {
                            generateLiana(x, y - 1);
                        }
                    }

                    //Spawn tree : que sur grass
                    float chanceSpawnTreeSeuil = UnityRandom.Range(1, 100);
                    float perlin_tree = PerlinCustom(unzoom_tree, seedTree, x, y);

                    if ((y + 1 + (maxHeightTree - minHeightTree) / 2 < height) && chanceSpawnTreeSeuil - perlin_tree * chanceSpawnTreePerlin <= chanceSpawnTree && mapVegetation[x - 1, y + 1 + (maxHeightTree - minHeightTree) / 2] != 1 && x < width - 1 && x != width - 3 && x != width - 4 && x != width - 2) //on veut pas que 2 arbres soient coll�s
                    {
                        int condition = 0;
                        int heightTree = UnityRandom.Range(minHeightTree, maxHeightTree);
                        for (int i = 1; i < heightTree + 12; i++) //On vérifie que tout l'arbre peut spawner, 12 pour prendre en compte la taille de la canop�e et 8 de marge
                        {
                            if (y + i < height && map[x, y + i] <= 0)
                            {
                                condition += 1;
                            }
                            if (condition == heightTree + 11) //si c'est le cas, on spawn l'arbre. Le +11 EST EN LIEN avec le +12 au dessus
                            {
                                generateTree(x, y + 1, heightTree);
                            }
                        }
                    }
                }
            }
        }
        return mapVegetation;
    }


    public IEnumerator WaterFlow(int x, int y, int z)
    {
        // Check for obstacles & borders before starting the coroutine recursion
        if ((y - 1 < 0 || map[x, y - 1] > 0 || mapLiquid[x, y - 1] == 1) && (x - 1 < 0 || map[x - 1, y] > 0 || mapLiquid[x - 1, y] == 1) && (x + 1 >= width || map[x + 1, y] > 0 || mapLiquid[x + 1, y] == 1))
        {
            yield break;
        }


        // Proceed with the original coroutine logic
        yield return new WaitForSeconds(0.2f);
        mapLiquid[x, y] = 1;

        if ((y > 0) && map[x, y - 1] <= 0)
        {
            liquid.SetTile(new Vector3Int(x, y - 1, 0), tileAtlas.waterRightTile.tileSprite);
            mapLiquid[x, y - 1] = 1;
            yield return StartCoroutine(WaterFlow(x, y - 1, 0));
        }

        if (x > 0 && y > 0 && map[x - 1, y] <= 0 && !(map[x, y - 1] <= 0))
        {
            liquid.SetTile(new Vector3Int(x - 1, y, 0), tileAtlas.waterLeftTile.tileSprite);
            mapLiquid[x - 1, y] = 1;
            StartCoroutine(WaterFlow(x - 1, y, 0));
        }

        if (x + 1 < width && y > 0 && map[x + 1, y] <= 0 && !(map[x, y - 1] <= 0))
        {
            liquid.SetTile(new Vector3Int(x + 1, y, 0), tileAtlas.waterRightTile.tileSprite);
            mapLiquid[x + 1, y] = 1;
            StartCoroutine(WaterFlow(x + 1, y, 0));
        }
    }


    public void SpawnDoor()
    {
        // GameObject souris = GameObject.FindWithTag("Souris"); //Player controller  : clique droit || clique gauche
        // souris.GetComponent<PlayerController>().nombreDeBlocsCasses = 0;
        GameObject endDoor = GameObject.FindWithTag("EndDoor"); //Player controller  : clique droit || clique gauche
        Transform[] enfants = endDoor.GetComponentsInChildren<Transform>(true);
        // enfants[0].gameObject.SetActive(true);
        enfants[1].gameObject.SetActive(true);

        for (int y = 3; y < height; y++)
        {
            if (map[width - 3, height - y] > 0)
            {
                doorSpawn = GameObject.FindGameObjectWithTag("EndDoor");

                //doorSpawn.SetActive(true);

                map[width - 3, height - y] = 100;
                destroyTile(width - 3, height - y, false);
                destroyTile(width - 4, height - y, false);
                destroyTile(width - 5, height - y, false);
                destroyTile(width - 2, height - y, false);

                destroyTile(width - 3, height - y + 1, false);
                destroyTile(width - 4, height - y + 1, false);
                destroyTile(width - 5, height - y + 1, false);
                destroyTile(width - 2, height - y + 1, false);

                destroyTile(width - 3, height - y + 2, false);
                destroyTile(width - 4, height - y + 2, false);
                destroyTile(width - 5, height - y + 2, false);
                destroyTile(width - 2, height - y + 2, false);

                doorSpawn.transform.position = new Vector3(width - 3, height - y, 0f);

                //topology.SetTile(new Vector3Int(width - 1, height - y, 0), tileAtlas.doorCloseTile.tileSprite);
                //Debug.Log("Porte");
                break;
            }
        }
    }


    public void RenderMap(int[,] map, int[,] mapCaves)
    {
        //R�cup�ration des coordonn�es en bas � gauche de la cam�ra
        Camera mainCamera = Camera.main;
        //Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector3Int bottomLeft = Vector3Int.FloorToInt(mainCamera.ViewportToWorldPoint(new Vector3Int(0, 0, 0)));

        //SI LEVEL PRAIRIE
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2")
        {
            Debug.Log("level 1 ou 2");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //ajouter une couche de bloc pour qu'aucune cave débouche sur le vide
                    if (y == 0)
                    {
                        map[x, y] = 3;
                    }

                    // if (mapFold[x, y] == 1) //si on a du brouillard
                    //     fold.SetTile(new Vector3Int(x, y, 0), tileAtlas.foldTile.tileSprite);

                    //stone
                    if (map[x, y] == 1) //si on a de la grass
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.grassTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == 2)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.dirtTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == 3)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.stoneTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }

                    else if (map[x, y] == 4)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.clayTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }

                    else if (map[x, y] == 5)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.goldTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }

                    //Caves
                    else if (map[x, y] == -1) //si on a de la bottomStoneTile (pierre de grotte)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == -2)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == -3)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }
                    else if (map[x, y] == -4)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == -5)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }

                    //Vegetation
                    if (mapVegetation[x, y] == 1) //Tronc
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.trunkTile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 2) //Branches
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.leafTile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 3) //Herb
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.herbTile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 4) //Fleur
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.flower1Tile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 5) //Fleur
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.lianaTile.tileSprite);
                    }
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level3" || SceneManager.GetActiveScene().name == "Level4")
        {
            //SI LEVEL NEIGE
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // if (mapFold[x, y] == 1) //si on a du brouillard
                    //     fold.SetTile(new Vector3Int(x, y, 0), tileAtlas.foldTile.tileSprite);

                    //stone
                    if (map[x, y] == 1) //si on a de la grass
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.snowTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == 2)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.dirtTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == 3)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.stoneTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }

                    else if (map[x, y] == 4)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.darkStoneTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }

                    else if (map[x, y] == 5)
                    {
                        topology.SetTile(new Vector3Int(x, y, 0), tileAtlas.goldTile.tileSprite);
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }

                    //Caves
                    else if (map[x, y] == -1) //si on a de la bottomStoneTile (pierre de grotte)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == -2)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == -3)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }
                    else if (map[x, y] == -4)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomDirtTile.tileSprite);
                    }
                    else if (map[x, y] == -5)
                    {
                        bottomCave.SetTile(new Vector3Int(x, y, 0), tileAtlas.bottomStoneTile.tileSprite);
                    }

                    //Vegetation
                    if (mapVegetation[x, y] == 1) //Tronc
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.freezeTrunkTile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 2) //Branches
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.freezeLeafTile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 3) //Herb
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.herbTile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 4) //Fleur
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.freezeFlower1Tile.tileSprite);
                    }
                    else if (mapVegetation[x, y] == 5) //Fleur
                    {
                        vegetation.SetTile(new Vector3Int(x, y, 0), tileAtlas.freezeLianaTile.tileSprite);
                    }
                }
            }
        }
    }

}