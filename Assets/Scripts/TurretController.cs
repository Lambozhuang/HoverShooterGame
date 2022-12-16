using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject explosion;
    private GameObject player;
    public GameObject shot;
    public Transform shotSpawn; 
    public float fireRate;
    private float nextFire;
    public float distanceToPlayer;
    public int health;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.state != GameController.GameState.Playing)
        {
            return;
        }
        Vector3 target = player.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target);
        // turn 90 degrees to face the player
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);

        // if the player is close enough
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToPlayer)
        {
            // and it's time to fire
            if (Time.time > nextFire)
            {
                // reset the nextFire time
                nextFire = Time.time + fireRate;
                // and fire
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FriendlyBullet")
        {
            Destroy(other.gameObject);
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                gameController.currentTurrentNum--;
                gameController.AddScore(100);
            }
        }
    }
}
