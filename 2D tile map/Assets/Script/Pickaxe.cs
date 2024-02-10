using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickaxe : MonoBehaviour
{
    private bool isRotating = false; // Indique si la rotation est en cours
    private float rotationAngle = 30f; // Angle de rotation en degrés
    private float rotationTime = 0.16f; // Durée de la rotation en secondes
    private float currentRotationTime = 0f; // Temps écoulé depuis le début de la rotation
    public SpriteRenderer sr;
    private Quaternion initialRotation; // Rotation de l'épée au début
    public bool pickaxe_right = true;
    public ProceduralGeneration proceduralGeneration;
    public int varDirectionMinage;
    public float delay = 5f;
    private bool canMine = true;

    private void Start()

    {
        sr = GetComponent<SpriteRenderer>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Vérifie si la touche Entrée est pressée
        if (Input.GetButtonDown("Fire1"))
        {
            SoundEffects.Instance.MakePickaxeSound();
            // Si la rotation n'est pas en cours, démarre la rotation
            if (!isRotating)
            {
                isRotating = true;
                currentRotationTime = 0f;
            }
            //minage
            if (canMine)
            {
                Transform grandParent = gameObject.transform.parent.parent; //positioon du joueur
                int x = (int)grandParent.position.x;
                int y = (int)grandParent.position.y;
                StartCoroutine(DestroyBlockWithDelay(x, y, delay));
            }


        }

        // Si la rotation est en cours
        if (isRotating)
        {
            // Calcule l'angle de rotation en fonction du temps écoulé
            float rotationProgress = currentRotationTime / rotationTime;
            float currentAngle = Mathf.Lerp(0f, rotationAngle, rotationProgress);

            // Effectue la rotation de l'objet
            if (pickaxe_right)
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

    private IEnumerator DestroyBlockWithDelay(int x, int y, float delay)
    {
        // Détruit les blocs devant le joueur 
        canMine = false;

        if (x + varDirectionMinage < proceduralGeneration.width && x + varDirectionMinage > 0)
            proceduralGeneration.destroyTile(x + varDirectionMinage, y, false);
        if (y + 1 < proceduralGeneration.height)
            proceduralGeneration.destroyTile(x + varDirectionMinage, y + 1, false);

        if (y + 2 < proceduralGeneration.height)
            proceduralGeneration.destroyTile(x + varDirectionMinage, y + 2, false);

        if (y + 3 < proceduralGeneration.height)
            proceduralGeneration.destroyTile(x + varDirectionMinage, y + 3, false);

        if (y - 1 < proceduralGeneration.height)
            proceduralGeneration.destroyTile(x + varDirectionMinage, y - 1, false);

        yield return new WaitForSeconds(delay); //délai entre coups

        canMine = true;

    }

}
