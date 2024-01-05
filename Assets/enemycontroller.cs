using UnityEngine;

public class enemycontroller : MonoBehaviour
{
    public float movementSpeed = 15f;
    public float detectionRadius = 10f;
    private GameObject enemyExplosion;
    private GameObject playerExplosion;

    private Transform target;
    private bool isPlayerDetected = false;

    private Rigidbody rb;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        enemyExplosion = GameObject.FindGameObjectsWithTag("enemyExplosion")[0];
        playerExplosion = GameObject.FindGameObjectsWithTag("playerExplosion")[0];
    }


    private void Update()
    {
        target = FindNearestPlayer();
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= detectionRadius)
            {
                isPlayerDetected = true;
                Vector3 moveDirection = (target.position - transform.position).normalized;
                transform.position += moveDirection * movementSpeed * movementSpeed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayerDetected && collision.gameObject.CompareTag("Player"))
        {
            PlayerMonopoly pc = FindObjectsOfType<PlayerMonopoly>()[0];
            pc.score += 5;
            Debug.Log("enemy collided with player");
            Destroy(collision.gameObject);
            Instantiate(enemyExplosion, transform.position, Quaternion.identity);
            Instantiate(playerExplosion, transform.position, Quaternion.identity);
            Object.Destroy(this.gameObject);
        }
    }

    private Transform FindNearestPlayer()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        // Debug.Log("found players " + playerObjects.Length);
        Transform nearestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject playerObject in playerObjects)
        {
            float distance = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = playerObject.transform;
            }
        }

        return nearestPlayer;
    }
}
