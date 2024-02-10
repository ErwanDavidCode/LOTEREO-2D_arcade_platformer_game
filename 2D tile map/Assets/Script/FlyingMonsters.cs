using UnityEngine;
using System.Collections;
public class FlyingMonsters : MonoBehaviour
{
    public float speed;
    private GameObject player;
    public GameObject healthBarContainer; // Référence à l'objet parent de la barre de vie

    public int damageOnCollision = 20;
    private ProceduralGeneration proceduralGeneration;
    public FloatingHealthBarFlyingMonster floatingHealthBarFlyingMonster; // Référence au script EnemyHealthBar
    public GameObject enemyParticule;
    private AudioSource BruitMort;
    private SpriteRenderer spriteRenderer;
    private bool canDamage = true;
    private Rigidbody2D rb;
    public float maxSpeed = 0.5f;
    bool isFacingRightMonster = false;

    private void Start()
    {
        // On recherche et assigne les objets voulus à des variables
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        GameObject proceduralGenerationObject = GameObject.FindWithTag("Procedural Generation");
        if (proceduralGenerationObject != null)
        {
            proceduralGeneration = proceduralGenerationObject.GetComponent<ProceduralGeneration>();
        }

        //Il peut ne pas y avoir de monstre
        if (floatingHealthBarFlyingMonster != null)
        {
            FloatingHealthBarFlyingMonster floatingHealthBarFlyingMonster = gameObject.GetComponent<FloatingHealthBarFlyingMonster>();
        }
        BruitMort = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Extrémités du terrain
        if (transform.position.x <= 0)
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);


        }
        if (transform.position.x >= proceduralGeneration.width)
        {
            transform.position = new Vector3(proceduralGeneration.width, transform.position.y, transform.position.z);

        }

        if (player == null)
            return;
        Chase();
        Flip();
        {
            // Test : Réduit la vie de l'ennemi lorsque la touche "P" est enfoncée
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (floatingHealthBarFlyingMonster != null)
                {
                    // Inflige 20 de dégâts à l'ennemi
                    TakeDamage(20f);
                }
            }
        }
    }

    // Méthode pour réduire la santé de l'ennemi
    public void TakeDamage(float damageAmount)
    {
        floatingHealthBarFlyingMonster.currentHealthFM -= damageAmount;
        floatingHealthBarFlyingMonster.currentHealthFM = Mathf.Clamp(floatingHealthBarFlyingMonster.currentHealthFM, 0f, floatingHealthBarFlyingMonster.maxHealthFM); // Assure que la santé reste entre 0 et maxHealth
        floatingHealthBarFlyingMonster.UpdateHealthBarFlyingMonster();

        if (floatingHealthBarFlyingMonster.currentHealthFM <= 10f)
        {
            // Destruction de l'objet avec une coroutine (délai) pour laisser les effets de sons et particules s'éxécuter
            if (gameObject != null)
            {
                StartCoroutine(TemporiserFonction());
            }
            CompteurText.comteurMonstreTues--;
        }
    }
    // Délai pour la bonne éxécution des effets
    private IEnumerator TemporiserFonction()
    {
        canDamage=false;
        AudioSource.PlayClipAtPoint(BruitMort.clip, transform.position);
        spriteRenderer.enabled=false;
        floatingHealthBarFlyingMonster.enabled=false;
        yield return new WaitForSeconds(0.01f);
        Instantiate(enemyParticule, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void Chase()
    {
        // Calcul d'un vecteur entre le joueur et le monstre (plus le monstre est loin, plus il prendra de la vitesse)
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 force = direction * speed;

        //Deux conditions en fonction de la position du joueur par rapport au monstre
        if ((player.transform.position.x - transform.position.x) < 0 && isFacingRightMonster)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            isFacingRightMonster = false;
        }
        if ((player.transform.position.x - transform.position.x) > 0 && !isFacingRightMonster)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            isFacingRightMonster = true;
        }
        rb.AddForce(force, ForceMode2D.Force);
    }

    private void Flip()
    {
        // Si le sprite du monstre ne regarde pas le joueur, alors il se tourne
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);

        // Ajuste la rotation du conteneur de la barre de vie
        if (healthBarContainer != null)
        {
            healthBarContainer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Permet au monstre d'attaquer le joueur
        if (collision.transform.CompareTag("Player") && canDamage==true)
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageOnCollision);
        }
    }
}
