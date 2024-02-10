using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    public int nombreBlocsNecessaires = 10;
    public GameObject porteOuverte;
    public GameObject porteFermee;
    //private GameObject souris;
    public GameObject WinMenu;

    // Start is called before the first frame update
    void Awake()
    {
        //souris = GameObject.FindWithTag("Souris"); //Player controller  : clique droit || clique gauche

        porteOuverte.SetActive(false);
        porteFermee.SetActive(true);
        WinMenu.SetActive(false);
    }

    void Update()
    {
        if (CompteurText.comteurMonstreTues <= 0) //|| souris.GetComponent<PlayerController>().nombreDeBlocsCasses >= nombreBlocsNecessaires)
        {
            // Changer le sprite de la porte
            porteOuverte.SetActive(true);
            porteFermee.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (CompteurText.comteurMonstreTues <= 0)) //|| souris.GetComponent<PlayerController>().nombreDeBlocsCasses >= nombreBlocsNecessaires))
        {
            Gagne();
        }
    }
    public void Gagne()
    {
        SC_Pause.Impossible=true;
        Time.timeScale = 0;
        WinMenu.SetActive(true);
    }
}
