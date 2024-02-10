using UnityEngine;

public class Sword : MonoBehaviour
{
    private bool isRotating = false; // Indique si la rotation est en cours
    private float rotationAngle = 30f; // Angle de rotation en degrés
    private float rotationTime = 0.16f; // Durée de la rotation en secondes
    private float currentRotationTime = 0f; // Temps écoulé depuis le début de la rotation
    public SpriteRenderer sr;
    private Quaternion initialRotation; // Rotation de l'épée au début
    public bool sword_right = true;
    public int swordForce = 10000;
    public float swordDamage;

    private void Start()

    {
        sr = GetComponent<SpriteRenderer>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Vérifie si la touche d'action ("E" dans notre cas) est pressée
        if (Input.GetButtonDown("Fire1"))
        {
            int resultat = Random.Range(0, 2);
            if (resultat == 0)
            {
                SoundEffects.Instance.MakeSwordSound1();
            }
            if (resultat == 1)
            {
                SoundEffects.Instance.MakeSwordSound1();
            }
            // Si la rotation n'est pas en cours, démarre la rotation
            if (!isRotating)
            {
                isRotating = true;
                currentRotationTime = 0f;
            }
        }

        // Si la rotation est en cours
        if (isRotating)
        {
            // Calcule l'angle de rotation en fonction du temps écoulé
            float rotationProgress = currentRotationTime / rotationTime;
            float currentAngle = Mathf.Lerp(0f, rotationAngle, rotationProgress);

            // Effectue la rotation de l'objet
            if (sword_right)
                transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, currentAngle);
            else
                transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, -currentAngle);

            // Incrémente le temps écoulé depuis le début de la rotation
            currentRotationTime += Time.deltaTime;

            // Si le temps écoulé est supérieur à la durée de rotation souhaitée, arrête la rotation
            if (currentRotationTime >= rotationTime)
            {
                isRotating = false;

                // Réinitialise la rotation de l'épée à sa position de base
                transform.rotation = initialRotation;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Monster>() != null && collision.CompareTag("Monster") && Input.GetButtonDown("Fire1"))
        {
            collision.GetComponent<Monster>().TakeDamage(swordDamage);

            // knockback (Recul)
            Vector2 recoilDirection = (collision.transform.position - transform.position).normalized;
            collision.attachedRigidbody.AddForce(recoilDirection * swordForce);
        }

        if (collision.GetComponent<FlyingMonsters>() != null && collision.CompareTag("Monster") && Input.GetButtonDown("Fire1"))
        {
            collision.GetComponent<FlyingMonsters>().TakeDamage(swordDamage);

            // knockback
            Vector2 recoilDirection = (collision.transform.position - transform.position).normalized;
            collision.attachedRigidbody.AddForce(recoilDirection * swordForce);
        }
    }

}
