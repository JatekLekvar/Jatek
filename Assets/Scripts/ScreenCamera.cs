using UnityEngine;

public class ScreenCamera : MonoBehaviour
{
    public Transform target;

    float limitX = 2f;
    float limitY = 2f;

    void Update()
    {
        Vector3 diff = target.position - transform.position;

        if (diff.x > limitX)
        {
            float disp = diff.x - limitX;
            transform.Translate(new Vector3(disp, 0f, 0f));
        }

        if (diff.x < -limitX)
        {
            float disp = diff.x + limitX;
            transform.Translate(new Vector3(disp, 0f, 0f));
        }

        if (diff.y > limitY)
        {
            float disp = diff.y - limitY;
            transform.Translate(new Vector3(0f, disp, 0f));
        }

        if (diff.y < -limitY)
        {
            float disp = diff.y + limitY;
            transform.Translate(new Vector3(0f, disp, 0f));
        }
    }
}
