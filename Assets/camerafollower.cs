using UnityEngine;
using TMPro;

public class CameraFollow : MonoBehaviour
{
    // public Transform target;  
    private Vector3 offset = new Vector3(0, 20f, -27f);   
    public float smoothSpeed = 0.005f; 
    public Transform selfTransform;
    public TextMeshPro tmpText;

    void LateUpdate()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        if (players.Length > 0){
            Transform target = players[0].GetComponent<Transform>();
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.x = 0;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            smoothedPosition = new Vector3(smoothedPosition.x, 21.5f, smoothedPosition.z);
            transform.position = smoothedPosition;
            Vector3 newpos = new Vector3(selfTransform.position.x, selfTransform.position.y-10.4f, selfTransform.position.z + 12f);
            tmpText.GetComponent<Transform>().position = newpos;
            transform.LookAt(new Vector3(0f, target.position.y, target.position.z));   
            // Debug.Log("camera pos: "+selfTransform.position);
        }
    }
}
