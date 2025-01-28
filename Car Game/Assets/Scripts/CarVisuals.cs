using UnityEngine;
using System.Collections;

public class CarVisuals : MonoBehaviour
{
    [SerializeField] private float maxAngle = 30f;
    [SerializeField] private float rotationSpeed;
	[SerializeField] private float resetRotationSpeed;

	[SerializeField] private GameObject tireMarksPrefab;
	// [SerializeField] private float tireMarkSpawnDelay;
	// private bool isSpawningTireMark;
	
	/* IEnumerator TireMarkSpawn()
	{
		isSpawningTireMark = true;
		Instantiate(tireMarksPrefab, new Vector3(transform.position.x, transform.position.y, 1.0f), Quaternion.identity);
		yield return new WaitForSeconds(tireMarkSpawnDelay);
		isSpawningTireMark = false;
	} */
	
    // Update is called once per frame
    void Update()
    {
	    // ROTATION LOGIC
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
		
		// TIREMARK LOGIC
		if (Input.GetKey(KeyCode.A) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)))
			Instantiate(tireMarksPrefab, new Vector3(transform.position.x, transform.position.y, 1.0f), Quaternion.identity);
    }
}
