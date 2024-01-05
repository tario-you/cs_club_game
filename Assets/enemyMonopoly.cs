
using UnityEngine;
using TMPro;

public class EnemyMonopoly : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public TextMeshPro tmpText;
    public float detectionRadius = 21f;
    private bool triggered = false;
    private bool spawned = false;
    private bool finishedmoving = false;
    public float moveSpeed = 20f;
    private int len = 0;

    private void Update(){
        if (! triggered){
            PlayerController[] followers = FindObjectsOfType<PlayerController>();
            foreach (PlayerController follower in followers){
                float fz = follower.GetComponent<Transform>().position.z;
                if (transform.position.z-fz < detectionRadius){
                    triggered = true;
                    len = followers.Length;
                    break;
                }
            }
        }else{
            if (! finishedmoving){
                if (transform.position.y > 50){
                    finishedmoving = true;
                }
                Vector3 newPosition = transform.position + Vector3.up * moveSpeed * Time.deltaTime;
                transform.position = newPosition;
            }
            if (! spawned){
                int additional = Random.Range(-2,3);
                
                if (Random.Range(1,10)<3){
                    len += additional;
                    len = Mathf.Max(0, len);
                }
                int toSpawn = Random.Range(1,len);
                tmpText.text = toSpawn.ToString();
                for (int i=0;i<toSpawn-1;i++){
                    SpawnPrefab();
                }
                spawned = true;
            }
        }
    }

    public void SpawnPrefab()
    {
        float randomX = Random.Range(0.5f, 2.5f) * (Random.Range(0, 2) == 0 ? -1 : 1); 
        float randomZ = Random.Range(0.5f, 3.0f) * (Random.Range(0, 2) == 0 ? -1 : 1); 
        float randomY = Random.Range(1.0f, 4.0f); 

        Vector3 modifiedPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y + randomY,
            transform.position.z + randomZ
        );

        Instantiate(prefabToSpawn, modifiedPosition, Quaternion.identity);
    }
}