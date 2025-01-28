using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleCollisionDetection : MonoBehaviour
{
    private float speed = 5;
    private GameObject player;
    private Vector3 velocity;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    // Update is called once per frame
    void Update()
    {
        velocity.x -= 1;
        velocity = velocity.normalized * (speed * Time.deltaTime);
        
        if (!IsThisTransformTouchingPlayer(transform.position + velocity))
        {
            transform.position += velocity;
        }
        else
        {
            GameObject gameover = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
            gameover.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
    }
    
    bool IsThisTransformTouchingPlayer(Vector3 positionToCheck)
    {
        float xDistance = Mathf.Abs(positionToCheck.x - player.transform.position.x);
        float yDistance = Mathf.Abs(positionToCheck.y - player.transform.position.y);
        // Find the maximum distance in X and Y by adding half of the player's scale and half of the wall's scale
        float xMaxDistance = transform.localScale.x / 2 + player.transform.localScale.x / 2;
        float yMaxDistance = transform.localScale.y / 2 + player.transform.localScale.y / 2;
        
        if (xDistance < xMaxDistance && yDistance < yMaxDistance) return true;
        return false;
    }
}
