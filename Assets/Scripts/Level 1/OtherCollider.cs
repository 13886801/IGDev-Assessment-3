using UnityEngine;

public class OtherCollider : MonoBehaviour
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
        if (collision.gameObject.tag == "Wall")
        {
            col = true;
            return;
        }
        col = false;
    }
}
