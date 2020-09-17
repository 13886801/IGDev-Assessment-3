public class LevelMap
{
    private int[,] Map = { //14x15
    {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
    {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
    {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
    {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
    {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
    {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
    {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
    {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
    {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
    {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
    {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
    {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
    {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    public bool isNothing(int value)
    {
        return value == 0;
    }

    public int getValue(int y, int x)
    {
        return Map[y, x];
    }

    public float getAngle(int y, int x)
    {
        int upOne = (y - 1 >= 0) ? getValue(y - 1, x) : 8;
        int downOne = (y + 1 < 15) ? getValue(y + 1, x) : 8;
        int leftOne = (x - 1 >= 0) ? getValue(y, x - 1) : 8;
        int rightOne = (x + 1 < 14) ? getValue(y, x + 1) : 8;

        int[] criteria_1 = new int[] { 1, 2, 7 }; //case 1 and 2
        int[] criteria_2 = new int[] { 3, 4, 7 }; //case 3 and 4

        switch (getValue(y, x))
        {
            case 1:
                if (inCriteria(criteria_1, leftOne))
                {
                    if (inCriteria(criteria_1, upOne))
                    {
                        return 180f;
                    }
                    return -90f;
                }

                if (inCriteria(criteria_1, upOne)) {
                    return 90f;
                }
                break;

            case 2:
                if (inCriteria(criteria_1, downOne) && inCriteria(criteria_1, upOne))
                {
                    return 90f;
                }
                break;

            case 3:
                if (rightOne == 8)
                {
                    if (inCriteria(criteria_2, leftOne))
                    {
                        return -90f;
                    }
                    return 90f;
                }

                if (leftOne == rightOne)
                {
                    if (downOne == 3)
                    {
                        return 90f;
                    }

                    if (upOne == 3)
                    {
                        break;
                    }
                }

                if (inCriteria(criteria_2, leftOne))
                {
                    if (inCriteria(criteria_2, upOne))
                    {
                        return 180f;
                    }
                    return -90f;
                }

                if (inCriteria(criteria_2, upOne))
                {
                    return 90f;
                }
                break;

            case 4:
                if (inCriteria(criteria_2, downOne) && inCriteria(criteria_2, upOne) || y == 14)
                {
                    return 90f;
                }
                break;
        }
        return 0f;
    }

    private bool inCriteria(int[] accepted, int num)
    {
        foreach (int i in accepted)
        {
            if (i == num)
            {
                return true;
            }
        }
        return false;
    }
}
