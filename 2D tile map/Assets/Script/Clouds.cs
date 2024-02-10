using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteGenerator : MonoBehaviour
{
    public Sprite sprite1, sprite2, sprite3, sprite4, sprite5, sprite6;
    public ProceduralGeneration proceduralGeneration;

    public float spawnInterval = 1.5f; // interval spawn nuage
    public float minSpeed = 0.8f; // Vitesse minimale
    public float maxSpeed = 2f; // Vitesse maximale
    public float minSize = 10f; // Vitesse maximale
    public float maxSize = 20f; // Vitesse maximale

    public int numberOfCloudsInitial = 15;

    void Start()
    {
        GenerateInitialClouds(numberOfCloudsInitial);

        // Utilisez StartCoroutine pour lancer la coroutine GenerateRandomSpriteCoroutine
        StartCoroutine(GenerateRandomSpriteCoroutine(proceduralGeneration.width + 10));
    }
    void GenerateInitialClouds(int numberOfClouds)
    {
        for (int i = 0; i < numberOfClouds; i++)
        {
            // Générez une position x aléatoire entre 0 et width
            float randomX = Random.Range(0f, proceduralGeneration.width);

            // Générez une hauteur aléatoire cohérente entre height-28 et height-8
            int randomHeight = Random.Range(proceduralGeneration.height - 28, proceduralGeneration.height - 8);

            // Appelez GenerateRandomSpriteObject avec la position x et la hauteur aléatoires
            GenerateRandomSpriteObject(randomX);
        }
    }

    void GenerateRandomSpriteObject(float xPosition)
    {
        // Choisissez un sprite aléatoire parmi les trois
        Sprite selectedSprite = GetRandomSprite();
        //hauteur aléatoire cohérente
        int randomheightClouds = Random.Range(proceduralGeneration.height - 27, proceduralGeneration.height - 3);
        // Définissez l'Order in Layer aléatoire
        int orderInLayer = Random.Range(-1, -4); //-4 exclu
        //Vitesse aléatoire
        float speed_temp = Random.Range(minSpeed, maxSpeed);
        float speed = Mathf.Lerp(speed_temp, speed_temp / 3f, Mathf.InverseLerp(-1, -3, orderInLayer));
        //taille en fonction de distance
        float t = Mathf.InverseLerp(-1f, -3f, orderInLayer);
        float size = Mathf.Lerp(maxSize, minSize, t);

        // Créez un nouvel objet avec le Sprite choisi
        GameObject newObject = new GameObject("RandomSpriteObject");
        newObject.transform.parent = transform; //on créer nuage comme enfant du GameObject "Background"
        newObject.transform.position = new Vector3(xPosition, randomheightClouds, 0);
        // Ajoutez un SpriteRenderer au nouvel objet et assignez le Sprite choisi
        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = selectedSprite;
        spriteRenderer.sortingOrder = orderInLayer;
        // Ajustez la taille en fonction de orderInLayer
        newObject.transform.localScale = new Vector3(size, size, 1f);
        //Ajouter le script mouve and destroy
        MoveAndDestroy moveAndDestroy = newObject.AddComponent<MoveAndDestroy>();
        moveAndDestroy.Initialize(speed, -10);

        // Rendre les nuages de plus en plus bleutés à mesure que orderInLayer devient plus négatif
        float blueTint = Mathf.Clamp01(-orderInLayer / 3f); // Assurez-vous que blueTint est entre 0 et 1
        spriteRenderer.color = new Color(1f - blueTint / 5, 1f, 1f);
    }

    IEnumerator GenerateRandomSpriteCoroutine(float xPosition)
    {
        while (true)
        {
            // Attendez pendant la durée de l'intervalle
            yield return new WaitForSeconds(spawnInterval);

            GenerateRandomSpriteObject(proceduralGeneration.width + 10);
        }
    }

    Sprite GetRandomSprite()
    {
        // Génère un nombre aléatoire entre 0 et 2 inclus pour choisir le Sprite
        int randomIndex = Random.Range(0, 7);

        // Retourne le Sprite correspondant à l'index généré
        switch (randomIndex)
        {
            case 0:
                return sprite1;
            case 1:
                return sprite2;
            case 2:
                return sprite3;
            case 3:
                return sprite3;
            case 4:
                return sprite3;
            case 5:
                return sprite3;
            default:
                return sprite1; // Si quelque chose ne va pas, retourne le premier Sprite
        }
    }
}
