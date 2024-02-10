using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarFlyingMonster : MonoBehaviour
{
    public Slider healthSliderFlyingMonster; // Référence au composant Slider de la barre de vie
    // public Camera camera;
    public float maxHealthFM = 100f; // Santé maximale de l'ennemi
    public float currentHealthFM; // Santé actuelle de l'ennemi

    public GameObject flyingMonster;
    void Start()
    {
        currentHealthFM = maxHealthFM;
        UpdateHealthBarFlyingMonster();
    }


    // Méthode pour mettre à jour la barre de vie
    public void UpdateHealthBarFlyingMonster()
    {
        // On s'assure que healthSlider ne soit pas nul
        if (healthSliderFlyingMonster != null)
        {
            // Met à jour la valeur du Slider en fonction de la santé actuelle
            healthSliderFlyingMonster.value = currentHealthFM / maxHealthFM;
        }
    }
}