using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class wallcontroller : MonoBehaviour
{
    private PlayerMonopoly pm;
    public bool canSpawn = true; 
    int result;
    public TextMeshPro tmpText;
    // public Transform transformX;
    private bool setUp = false;
    public Transform selfTransform;
    private wallcontroller partner;

    private void Start(){
        tmpText.text = "?";
        PlayerController[] followers = FindObjectsOfType<PlayerController>();
        wallcontroller[] otherWalls = FindObjectsOfType<wallcontroller>();
        foreach (wallcontroller other in otherWalls)
            if (selfTransform.position.z == other.GetComponent<Transform>().position.z){
                partner = other;
                break;
            }
        // Debug.Log((selfTransform.position.z - followers[0].GetComponent<Transform>().position.z) + " away from player");
    }

    private void getRandAdd(int x){
        int factor = Mathf.Min(Random.Range(1, x*10),13);
        result = factor; // the thing that is passed to spawner/destructr
        tmpText.text = "+ "+factor; 
    }
    private void getRandMultiply(int x){
        int factor = Random.Range(2, 6);
        result = x*(factor-1); // the thing that is passed to spawner/destructr
        tmpText.text = "x "+factor; 
    }
    private void getRandSubtract(int x){
        int factor = Random.Range(-1, -x*2+1);
        result = factor; // the thing that is passed to spawner/destructr
        tmpText.text = "- "+(-factor); 
    }
    private void getRandDivide(int x){
        List<int> factors = new List<int>();
        for (int i=1;i<=5;i++){
            if (x % i == 0){
                factors.Add((int)x/(int)i);
            }
        }
        int randIndex = Random.Range(0,factors.Count);
        result = -(int)(x - x/factors[randIndex]); // the thing that is passed to spawner/destructr
        tmpText.text = "÷ "+factors[randIndex]; 
    }

    private void Update()
    {
        if (! setUp){
            PlayerController[] followers = FindObjectsOfType<PlayerController>();
            bool playerClose = false;
            if (followers.Length > 0){
                
                foreach (PlayerController follower in followers){
                    if (selfTransform.position.z - follower.GetComponent<Transform>().position.z < 20){
                        playerClose = true;
                        break;
                    }
                }
            }
            if (playerClose){
                /*
                < 15 seconds: 0.6 +, 0.3 * , 0.1 - 
                < 30 seconds: 0.3 + , 0.15 *, 0.25 - , 0.3 /
                else: 0.2 + , 0.1 * , 0.4 - , 0.3 / 
                 */
                float diceroll = Random.Range(0f,1f);
                int playersLength = followers.Length;
                int result;
                if (Time.time < 15f){
                    if (diceroll<0.6f){
                        getRandAdd(playersLength);
                    }else{
                        getRandMultiply(playersLength);
                    }
                }else if (Time.time < 30f){
                    if (diceroll<0.3f){
                        getRandAdd(playersLength);
                    }else if (diceroll < 0.45f){
                        getRandMultiply(playersLength);
                    }else{
                        getRandAdd(playersLength);
                    }
                }else{
                    if (diceroll<0.2f){
                        getRandAdd(playersLength);
                    }else if (diceroll < 0.3f){
                        getRandMultiply(playersLength);
                    }else{
                        getRandSubtract(playersLength);
                    }
                }
                setUp = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.canSpawn && other.CompareTag("Player"))
        {
            pm = FindObjectsOfType<PlayerMonopoly>()[0];
            Debug.Log("spawnings!");
            canSpawn = false;
            partner.canSpawn = false;
            if (result > 0){
                for (int i = 0; i < result; i++)
                {
                    pm.SpawnPrefab();
                }
            }else{
                PlayerController[] followers = FindObjectsOfType<PlayerController>();
                for (int i = 0; i < -result; i++){
                    Destroy(followers[i].gameObject);
                }
            }
        }
    }
}
