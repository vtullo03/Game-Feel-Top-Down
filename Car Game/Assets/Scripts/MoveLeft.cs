using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-0.2f, 0, 0);
    }
}
