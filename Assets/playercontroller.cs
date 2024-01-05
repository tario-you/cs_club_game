using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Rigidbody rb;
    public Transform transform;
    public bool HasMoved = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // private void Update()
    // {

    //     // float minDistance = 1f;
    //     // Transform transformX = FindObjectsOfType<TransformXObject>()[0].GetComponent<Transform>();
    // }

    /* private void Update(){
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = (transform.right * horizontalInput).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, -moveSpeed, moveSpeed);
        float minDistance = 1f;
        Transform transformX = FindObjectsOfType<TransformXObject>()[0].GetComponent<Transform>();

        foreach (PlayerController follower in FindObjectsOfType<PlayerController>())
        {
            float distanceToTransformX = Vector3.Distance(follower.transform.position, transformX.position);
            if (distanceToTransformX < minDistance)
            {
                Vector3 newPosition = transformX.position + (follower.transform.position - transformX.position).normalized * minDistance;
                follower.transform.position = newPosition;
            }
        }
    } */
}
