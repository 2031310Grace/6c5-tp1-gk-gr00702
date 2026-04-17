using UnityEngine;

public class WallMoving : MonoBehaviour
{
    public float speed = 2f;      
    public float distance = 3f;
    private Vector3 start;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Sin(Time.time * speed) * distance;
        transform.position = start + new Vector3(x, 0, 0);
    }

    public bool IsOpen()
    {
        return transform.position.x <= start.x - distance + 0.1f;
    }
}
