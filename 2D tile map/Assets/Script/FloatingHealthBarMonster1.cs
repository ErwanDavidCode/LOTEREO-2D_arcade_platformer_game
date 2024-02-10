using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarMonster : MonoBehaviour
{
    public Slider healthSliderMonster; // Référence au composant Slider de la barre de vie
    // public Camera camera;
    public float maxHealthM = 100f; // Santé maximale de l'ennemi
    public float currentHealthM; // Santé actuelle de l'ennemi

    public GameObject monster;

    void Start()
    {
        currentHealthM = maxHealthM;
        UpdateHealthBarMonster();
    }

    // Méthode pour mettre à jour la barre de vie
    public void UpdateHealthBarMonster()
    {
        // On s'assure que healthSlider ne soit pas nul
        if (healthSliderMonster != null)
        {
            // Met à jour la valeur du Slider en fonction de la santé actuelle
            healthSliderMonster.value = currentHealthM / maxHealthM;
        }
    }
}