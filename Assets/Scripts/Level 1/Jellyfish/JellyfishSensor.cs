using UnityEngine;

public class JellyfishSensor : MonoBehaviour
{
    private bool isColliding = false;
    private Transform pacStudent;
    private int collisionCount;
    private Vector2 pos;

    void Start()
    {
        pacStudent = GameObject.FindGameObjectWithTag("PacStudent").transform;
    }

    public bool IsColliding()
    {
        pos = gameObject.transform.position;
        if (pos.x > 27 || pos.x < 1 ||
            (12 <= pos.x && pos.x <= 15 && -2.5f <= pos.y && pos.y <= 2))
        {
            return true;
        }
        return isColliding;
    }

    public int CalculateDistance()
    {
        return (int)Vector2.Distance(gameObject.transform.position, pacStudent.position);
    }

    public int CalculateDistance(Vector2 position)
    {
        return (int)Vector2.Distance(gameObject.transform.position, position);
    }

    void Update()
    {
        isColliding = collisionCount > 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            collisionCount++;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            collisionCount--;
        }
    }
}