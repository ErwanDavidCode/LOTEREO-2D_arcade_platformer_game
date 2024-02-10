using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonsterManager : MonoBehaviour
{
    public GameObject flyingMonsterPrefab;
    public int numberOfFlyingMonsters = 5;
    public float spawnIntervalFlyingMonster = 2f;
    public ProceduralGeneration proceduralGeneration;

    // Start is called before the first frame update
    void Start()
    {
        // Permet l'apparition des monstres toutes les x secondes
        StartCoroutine(SpawnFlyingMonsters());
    }


    IEnumerator SpawnFlyingMonsters()
    {
        for (int i = 0; i < numberOfFlyingMonsters; i++)
        {
            SpawnFlyingMonster();
            // La commande suivant permet de mettre un délai entre les itérations de la boucle for
            yield return new WaitForSeconds(spawnIntervalFlyingMonster);
        }
    }

    void SpawnFlyingMonster()
    {
        // On choisit les coordonnées possibles d'apparition des monstres
        Vector3 spawnPosition = new Vector3(Random.Range(20f, proceduralGeneration.width-5), 80f, 0f);

        Instantiate(flyingMonsterPrefab, spawnPosition, Quaternion.identity);
    }
}

