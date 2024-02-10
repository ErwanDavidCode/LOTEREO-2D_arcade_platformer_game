using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    public float moveSpeed = 4;
    public float destroyX = -10;

    public void Initialize(float speed, float destroyPosition)
    {
        moveSpeed = speed;
        destroyX = destroyPosition;
    }

    void Update()
    {
        // Déplace l'objet vers la gauche
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Détruit l'objet s'il atteint la position de destruction
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}
