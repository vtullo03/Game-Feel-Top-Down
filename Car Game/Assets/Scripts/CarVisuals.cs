using UnityEngine;

public class CarVisuals : MonoBehaviour
{
    [SerializeField] private float maxAngle = 30f;
    [SerializeField] private float rotationSpeed;
	[SerializeField] private float resetRotationSpeed;
    private float currentAngle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) || 
			Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) || 
			Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            if(transform.rotation.eulerAngles.z < maxAngle)
				transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) || 
			Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) || 
			Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A))
        {
            if(transform.rotation.eulerAngles.z > -maxAngle)
				transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));
        }
		
		if (Mathf.Abs(transform.rotation.eulerAngles.z) > 0 && !Input.GetKey(KeyCode.W)
			&& !Input.GetKey(KeyCode.S))
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * resetRotationSpeed);
		}
    }
}
