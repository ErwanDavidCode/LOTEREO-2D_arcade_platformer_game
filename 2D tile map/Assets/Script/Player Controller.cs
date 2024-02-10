using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public Vector2Int mousePos;
    public Tilemap topology;
    public int nombreDeBlocsCasses = 0;
    private ProceduralGeneration proceduralGeneration;

    // Start is called before the first frame update
    void Start()
    {
        proceduralGeneration = GameObject.FindWithTag("Procedural Generation").GetComponent<ProceduralGeneration>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ce script permet de détruire des blocs à l'aide de la souris
        Camera mainCamera = Camera.main;
        Vector3Int bottomLeft = Vector3Int.FloorToInt(mainCamera.ViewportToWorldPoint(new Vector3Int(0, 0, 0)));

        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        // Détruit un bloc à l'aide du clic gauche
        if (Input.GetMouseButton(0))
        {
            proceduralGeneration.destroyTile(mousePos.x, mousePos.y, true);
            if (proceduralGeneration.map[mousePos.x, mousePos.y] > 0)
            {
                if (gameObject != null && gameObject.activeSelf)
                    nombreDeBlocsCasses++;
            }
        }
        // Permet d'ajouter de l'eau à l'aide du clic droit
        if (Input.GetMouseButton(1))
        {
            proceduralGeneration.StartCoroutine(proceduralGeneration.WaterFlow(mousePos.x, mousePos.y, 0));
        }
    }
}
