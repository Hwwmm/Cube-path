using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Range(1, 10)]
    public float cameraFollowSpeed = 1;
    public PlayerController playerController;
    public Color[] colors;

    Transform playerTransform;
    Vector3 newPosition;
    float initialDeltaZ;
    float initialDeltaX;

    void Start()
    {
        // Set background color
        playerTransform = playerController.transform;
        int randomIndex = Random.Range(0, colors.Length);
        Camera.main.backgroundColor = colors[randomIndex];

        // Init values for camera follow
        newPosition = transform.position;
        initialDeltaX = playerTransform.position.x - transform.position.x;
        initialDeltaZ = playerTransform.position.z - transform.position.z;
    }

    void Update()
    {
        
        if (playerController != null && playerController.isCameraFollow)
        {
            float deltaZ = playerTransform.position.z - transform.position.z;
            float deltaX = playerTransform.position.x - transform.position.x;

            if (Mathf.Abs(deltaZ - initialDeltaZ) >= 1)
            {
                newPosition.z += (deltaZ - initialDeltaZ) * cameraFollowSpeed * Time.deltaTime;
            }

            if (Mathf.Abs(deltaX - initialDeltaX) >= 1)
            {
                newPosition.x += (deltaX - initialDeltaX) * cameraFollowSpeed * Time.deltaTime;
            }

            transform.position = newPosition;
        }
    }
}
