using UnityEngine;

public class PacStudentCollider : MonoBehaviour
{
    private bool col = false;
    private int collisionCount;

    public bool isThereAWall()
    {
        return col;
    }

    void Update()
    {
        col = collisionCount > 0;
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
