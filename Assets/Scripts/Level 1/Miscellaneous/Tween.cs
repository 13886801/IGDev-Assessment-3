using UnityEngine;

public class Tween
{
    private Vector3 startpos;
    public Vector3 endPos { get; private set; }

    private float speed;
    private float tick;
    public bool hasTween { get; private set; } = false;

    public void setTweenValues(Vector3 startingPos, Vector3 destination, float duration)
    {
        startpos = startingPos;
        endPos = destination;
        speed = duration;
        tick = 0f;
        hasTween = true;
    }

    public Vector3 calculatePosition(float time)
    {
        tick += time;
        float timeFraction = tick/speed;
        if (timeFraction > 1f)
        {
            hasTween = false;
            return endPos;
        }
        return Vector3.Lerp(startpos, endPos, timeFraction);
    }

    public void stopTween()
    {
        hasTween = false;
    }
}
