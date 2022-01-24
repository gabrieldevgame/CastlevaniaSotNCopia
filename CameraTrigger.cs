using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Vector2 maxXandY, minXandY;

    private CameraFollow cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            cameraFollow.maxXAndY = maxXandY;
            cameraFollow.minXAndY = minXandY;
        }
    }
}
