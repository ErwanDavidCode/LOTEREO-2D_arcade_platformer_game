using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;
    public LayerMask groundLayer;
    public float moveSpeed = 3f;
    public float jumpForce = 600f;
    public float changeDirectionInterval = 3f; // Interval de temps pour changer de direction
    public float jumpProbability = 0.2f; // Probabilité de sauter
    public float obstacleDetectionDistance = 0.5f; // Distance de détection des obstacles
    public float distanceDeDetection = 10f; // Distance à partir de laquelle l'ennemi détecte le joueur
    private Rigidbody2D rb;
    private bool facingRight = true;
    private float nextDirectionChangeTime;
    public int damageOnCollision = 10;
    public FloatingHealthBarMonster floatingHealthBarMonster;
    public GameObject healthBarContainerMonster;
    public ProceduralGeneration proceduralGeneration;
    public Sword swordScript;
    public GameObject enemyParticule;
    private Time dernierAppel;
    private AudioSource BruitMort;
    private SpriteRenderer spriteRenderer;
    private bool canDamage = true;

    void Start()
    {
        // Régule le comportement du monstre
        rb = GetComponent<Rigidbody2D>();
        nextDirectionChangeTime = Time.time + Random.Range(0f, changeDirectionInterval);

        GameObject Joueur = GameObject.FindWithTag("Player");
        if (Joueur != null)
        {
            player = Joueur.transform;
        }

        GameObject proceduralGenerationObject = GameObject.FindWithTag("Procedural Generation");
        if (proceduralGenerationObject != null)
        {
            proceduralGeneration = proceduralGenerationObject.GetComponent<ProceduralGeneration>();
        }

        GameObject swordObject = GameObject.FindWithTag("Sword");
        if (swordObject != null)
        {
            swordScript = swordObject.GetComponent<Sword>();
        }

        if (floatingHealthBarMonster != null)
        {
            FloatingHealthBarMonster floatingHealthBarMonster = gameObject.GetComponent<FloatingHealthBarMonster>();
        }
        BruitMort = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Extrémité du terrain
        if (transform.position.x <= 0)
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);


        }
        if (transform.position.x >= proceduralGeneration.width)
        {
            transform.position = new Vector3(proceduralGeneration.width, transform.position.y, transform.position.z);

        }

        FollowPlayer();
        CheckDirectionChange();
        CheckObstacle();
        {
            // Test : Réduit la vie de l'ennemi lorsque la touche "L" est enfoncée
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (floatingHealthBarMonster != null)
                {
                    // Inflige 20 de dégâts à l'ennemi (vous pouvez ajuster cette valeur selon vos besoins)
                    TakeDamage(20f);
                }
            }
        }
        Flip2();
    }

    void FollowPlayer()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= distanceDeDetection && canDamage==true)
        {
            Vector2 targetPosition = new Vector2(player.position.x, rb.position.y);
            float step = moveSpeed * Time.deltaTime;
            rb.position = Vector2.MoveTowards(rb.position, targetPosition, step);
        }
    }

    private void Flip2()
    {
        if (player != null)
        {
            if (transform.position.x > player.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Ajuste la rotation du conteneur de la barre de vie
        if (healthBarContainerMonster != null)
        {
            healthBarContainerMonster.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    void CheckDirectionChange()
    {
        if (Time.time > nextDirectionChangeTime)
        {
            // Choisir aléatoirement entre sauter et changer de direction
            float randomValue = Random.value;
            if (randomValue < jumpProbability)
            {
                Jump();
            }
            else
            {
                ChangeDirection();
            }

            // Mettre à jour le prochain temps de changement de direction
            nextDirectionChangeTime = Time.time + Random.Range(0f, changeDirectionInterval);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
    }

    void ChangeDirection()
    {
        // Changer de direction
        facingRight = !facingRight;
        Flip2();
    }

    void CheckObstacle()
    {
        // Vérifier s'il y a un obstacle devant l'ennemi
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingRight ? Vector2.right : Vector2.left, obstacleDetectionDistance, groundLayer);

        if (hit.collider != null)
        {
            // Obstacle détecté, changer de direction ou sauter immédiatement
            if (rb.velocity.magnitude < 0.1f) // Vérifier si l'ennemi est pratiquement à l'arrêt
            {
                float randomValue = Random.value;
                if (randomValue < jumpProbability)
                {
                    Jump();
                }
                else
                {
                    ChangeDirection();
                }
            }
        }
    }



    public void TakeDamage(float damageAmount)
    {
        // Lorsque le monstre prend des dégâts
        floatingHealthBarMonster.currentHealthM -= damageAmount;
        floatingHealthBarMonster.currentHealthM = Mathf.Clamp(floatingHealthBarMonster.currentHealthM, 0f, floatingHealthBarMonster.maxHealthM); // Assure que la santé reste entre 0 et maxHealth
        floatingHealthBarMonster.UpdateHealthBarMonster();

        if (floatingHealthBarMonster.currentHealthM <= 10f)
        {
            if (gameObject != null)
            {
                StartCoroutine(TemporiserFonction());
            }
            CompteurText.comteurMonstreTues--;
        }
    }
    private IEnumerator TemporiserFonction()
    {
        // Temporisation pour les animations
        canDamage=false;
        AudioSource.PlayClipAtPoint(BruitMort.clip, transform.position);
        spriteRenderer.enabled=false;
        floatingHealthBarMonster.enabled=false;
        yield return new WaitForSeconds(0.01f);
        Instantiate(enemyParticule, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Le monstre fait des dégâts au joueur lorsqu'il le touche
        if (collision.transform.CompareTag("Player") && canDamage==true)
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageOnCollision);
        }
    }

}