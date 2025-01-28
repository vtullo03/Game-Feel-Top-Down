using UnityEngine;

public class OffscreenDespawner : MonoBehaviour
{
    private float deathXValue = -11.0f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < deathXValue) Destroy(gameObject);   
    }
}
