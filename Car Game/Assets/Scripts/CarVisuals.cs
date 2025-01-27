using UnityEngine;

public class CarVisuals : MonoBehaviour
{
    [SerializeField] private float maxAngle = 30f;
    [SerializeField] private float rotationSpeed;
    private float currentAngle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if(transform.rotation.eulerAngles.z < maxAngle) 
                transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }
      
        if (Input.GetKey(KeyCode.S))
        {
            if(transform.rotation.eulerAngles.z > -maxAngle) 
                transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));
        }
    }
}
