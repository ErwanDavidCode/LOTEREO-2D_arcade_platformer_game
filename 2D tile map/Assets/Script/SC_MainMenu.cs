using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject InstructionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuBouton();
    }
    public void PlayNowBouton()
    {
        // On crée ici la fonction d'appui sur le bouton
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void CreditsBouton()
    {
        //Montrer les crédits
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }
    public void MainMenuBouton()
    {
        //Montrer le menu principal
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        InstructionsMenu.SetActive(false);
    }
    public void InstructionsBouton()
    {
        //Montrer les commande+instructions
        MainMenu.SetActive(false);
        InstructionsMenu.SetActive(true);
    }
    public void QuitBouton()
    {
        //Quitte le jeu
        Application.Quit();
    }
}
