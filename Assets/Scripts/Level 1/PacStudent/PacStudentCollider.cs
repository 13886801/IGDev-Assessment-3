using UnityEngine;

public class PacStudentCollider : MonoBehaviour
{
    private bool col = false;

    public bool isThereAWall()
    {
        return col;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        col = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        col = collision.gameObject.tag == "Wall";
    }
}
