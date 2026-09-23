using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private float fixedY;
    private float fixedZ;
    public float offset;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPos = new Vector3(player.position.x + offset, fixedY, fixedZ);
        transform.position = Vector3.Lerp(transform.position, desiredPos, speed);
    }
}
