using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 200;
    public int currentHealth;

    public float invicibilityTimeAfterHit = 1f;
    public float invincibilityFlashDelay = 0.1f;
    public bool isInvincible = false;

    public SpriteRenderer graphics;

    public HealthBar healthBar;
    public GameObject LooseMenu;
    public GameObject playerParticule;

    void Start()
    {
        // Inititalisation de la barre de vie
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        LooseMenu.SetActive(false);
    }

    void Update()
    {
        // Test: le joueur perd de la vie quand on appuie sur la touche "H"
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        // Gestion de la vie du joueur lorsqu'il prend des dégâts (ainsi que son temps d'invincibilité)
        if (!isInvincible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            SoundEffects.Instance.PlayerTakeDamageSound();
            StartCoroutine(HandleInvicibilityDelay());
            StartCoroutine(InvicibilityFlash());
            
        }
        if (currentHealth <= 0) // Mort du joueur
        {
            SoundEffects.Instance.MakePlayerDeathSound(); 
            Loose();
        }
    }

    public IEnumerator InvicibilityFlash() //Le joueur clignote lorsqu'il est invincible
    {
        while (isInvincible)
        {
            Debug.Log("C'est dans la boucle");
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    public IEnumerator HandleInvicibilityDelay() // Temps pendant lequel le joueur est invincible
    {
        isInvincible = true;
        yield return new WaitForSeconds(invicibilityTimeAfterHit);
        isInvincible = false;
    }
    public void Loose() // Affiche le game over
    {
        SC_Pause.Impossible = true;
        Instantiate(playerParticule, transform.position, Quaternion.identity);
        StartCoroutine(playerDeath(0.3f));

    }

    public IEnumerator playerDeath(float delayTime) // Permet l'affichage des effets de mort du joueur
    {
        yield return new WaitForSeconds(delayTime);

        LooseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
