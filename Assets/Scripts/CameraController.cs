using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float rotationDamping;
    public float distance;
    public float height;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.state == GameController.GameState.GameOver)
        {
            return;
        }
        
        float wantedRotationAngle = player.transform.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = player.transform.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        Vector3 newPosition = new Vector3(transform.position.x, height, transform.position.z);
        transform.position = newPosition;

        transform.LookAt(player.transform);
    }
}
