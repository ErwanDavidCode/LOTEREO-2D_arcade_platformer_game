using UnityEngine;
using UnityEngine.UI;

public class CompteurText : MonoBehaviour
{
    // Variable partagée
    public static int comteurMonstreTuesInitial = 10;
    public static int comteurMonstreTues;

    // Référence au composant Text
    private Text textComponent;

    void Start()
    {
        // Obtenez la référence au composant Text attaché à cet objet
        textComponent = GetComponent<Text>();
        comteurMonstreTues = comteurMonstreTuesInitial; //on initialise à 5
    }

    void Update()
    {
        // Mettez à jour le texte avec la valeur partagée
        if (textComponent != null)
            if (comteurMonstreTues > 0)
            {
                textComponent.text = "Nombre de monstres a tuer : " + comteurMonstreTues.ToString();
            }
            else
            {
                textComponent.text = "Porte ouverte !";
            }
    }
}
