using UnityEngine;

public class CameraController1 : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("BoyPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        float positionX = player.transform.position.x;
        float positionY = player.transform.position.y;
        float positionZ = transform.position.z;
        transform.position= new Vector3(positionX,positionY,positionZ);
    }
}
