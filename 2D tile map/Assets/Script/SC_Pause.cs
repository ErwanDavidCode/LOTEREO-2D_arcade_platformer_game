using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SC_Pause : MonoBehaviour
{
    public static bool JeuEnPause = false;
    public static bool Impossible = false;
    public GameObject MenuPause;

    //Start 
    void Start()
    {
        MenuPause.SetActive(false);
        Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        // Met le jeu en pause lorsque l'on appuie sur échap
        if (Input.GetKeyDown(KeyCode.Escape) && Impossible == false)
        {
            if (JeuEnPause)
            {
                Continuer();
            }
            else
            {
                Pause();
            }
        }
    }
    //Création de la fonction Pause qui arrête le temps et affiche le menu
    void Pause()
    {
        MenuPause.SetActive(true);
        Time.timeScale = 0;
        JeuEnPause = true;
    }
    public void Continuer() // Reprendre la partie
    {
        MenuPause.SetActive(false);
        Time.timeScale = 1;
        JeuEnPause = false;
    }
    public void MenuPrincipal() // Retourner au menu principal
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        Impossible=false;
    }
    public void Niveau2() // Aller au niveau 2
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
        Impossible=false;
    }
}
