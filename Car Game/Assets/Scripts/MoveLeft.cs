using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float velocity = -10.0f * Time.deltaTime;
        transform.position += new Vector3(velocity, 0, 0);
    }
}
