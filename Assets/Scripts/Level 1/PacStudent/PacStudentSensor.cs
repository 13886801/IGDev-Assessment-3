using UnityEngine;

public class PacStudentSensor : MonoBehaviour
{
    private GameObject pacStudent;
    private PacStudentCollider[] sensors = new PacStudentCollider[4];

    void Start()
    {
        pacStudent = GameObject.FindGameObjectWithTag("PacStudent");

        for (int i = 0; i < 4; i++)
        {
            sensors[i] = gameObject.transform.GetChild(i).GetComponent<PacStudentCollider>();
        }
    }

    public bool IsThereAWall(string direction)
    {
        switch(direction)
        {
            case "Up":
                return sensors[0].isThereAWall();

            case "Left":
                return sensors[1].isThereAWall();

            case "Down":
                return sensors[2].isThereAWall();

            case "Right":
                return sensors[3].isThereAWall();

            default:
                break;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = pacStudent.transform.position;
    }
}
