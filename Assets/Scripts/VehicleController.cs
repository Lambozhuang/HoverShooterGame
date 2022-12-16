using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed;
    public float boostSpeed;
    public float turnSpeed;

    public GameObject shot;
    public Transform shotSpawn; 
    public float fireRate;
    private float nextFire;
    private GameController gameController;
    public int health;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.state != GameController.GameState.Playing)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }

            GameObject.Find("Minigun_MobilePart").transform.Rotate(0, 0, 1000 * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (gameController.state != GameController.GameState.Playing && gameController.state != GameController.GameState.ReadyToStart)
        {
            return;
        }
        if (gameController.state == GameController.GameState.ReadyToStart && moveVertical != 0)
        {
            gameController.StartGame();
        }

        bool boost = Input.GetKey(KeyCode.LeftShift);

        if (moveHorizontal != 0)
        {
            transform.Rotate(new Vector3(0.0f, moveHorizontal * turnSpeed, 0.0f));
        }

        if (moveVertical != 0)
        {
            Vector3 forward = transform.forward;
            if (boost)
            {
                GetComponent<Rigidbody>().velocity = forward * boostSpeed * moveVertical;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = forward * speed * moveVertical;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameController.state != GameController.GameState.Playing)
        {
            return;
        }
        if (other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);
                health = 0;
                gameController.GameOver();
            }
        }
    }
}
