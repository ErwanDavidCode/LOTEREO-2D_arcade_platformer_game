using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;
    private Vector3 tempPos;
    public ProceduralGeneration proceduralGeneration;

    [SerializeField]
    private float minX, maxX;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        GetComponent<Camera>().orthographicSize = 8;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            tempPos = transform.position;
            tempPos.x = playerTransform.position.x;
            tempPos.y = playerTransform.position.y;
            
            //Récupérer le nouvelles abscisses potentielles de la caméra
            minX = GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect;
            maxX = proceduralGeneration.width - GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect;

            // Bord gauche et bord droit
            if (tempPos.x < minX)
            {
                tempPos.x = minX;
            }
            if (tempPos.x > maxX)
            {
                tempPos.x = maxX;
            }

            transform.position = tempPos; //Mise à jour
        }
    }
}
