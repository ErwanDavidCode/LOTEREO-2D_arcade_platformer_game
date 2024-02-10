using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public ProceduralGeneration proceduralGeneration;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    public GameObject monsterPrefab;
    public int numberOfMonsters = 5;
    public float spawnIntervalMonster = 2f;

    IEnumerator SpawnMonsters()
    {
        // Génère un monstre à un intervalle de temps défini par l'utilisateur
        for (int i = 0; i < numberOfMonsters; i++)
        {
            SpawnMonster();
            yield return new WaitForSeconds(spawnIntervalMonster);
        }
    }

    void SpawnMonster()
    {
        // On choisit les coordonnées possibles d'apparition des monstres
        Vector3 spawnPosition = new Vector3(Random.Range(20f, proceduralGeneration.width), 80f, 0f);

        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }
}

