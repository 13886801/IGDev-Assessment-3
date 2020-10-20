using UnityEngine;

public class JellyFishSensor : MonoBehaviour
{
    private bool isColliding = false;
    private Transform pacStudent;
    private Vector2 pos;

    void Start()
    {
        pacStudent = GameObject.FindGameObjectWithTag("PacStudent").transform;
    }

    public bool IsColliding()
    {
        pos = gameObject.transform.position;
        if (pos.x > 27 || pos.x < 1 ||
            (13 <= pos.x && pos.x <= 14 && -2 <= pos.y && pos.y <= 2))
        {
            return true;
        }
        return isColliding;
    }

    public int CalculateDistance()
    {
        return (int)Vector2.Distance(gameObject.transform.position, pacStudent.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = collision.transform.CompareTag("Wall");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        isColliding = collision.transform.CompareTag("Wall");
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
}
