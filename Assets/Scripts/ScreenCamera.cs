using UnityEngine;

public class ScreenCamera : MonoBehaviour
{
    public Transform target;
    public float minY;
    public float maxY;
    public float minX;
    public float maxX;

    public float limitX = 2f;
    public float limitY = 2f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //limitX = GameObject.Find("Scene Specs").GetComponent<SceneSpecs>().MinX;
    }

    void Update()
    {
        GameObject sceneSpecs = GameObject.Find("Scene Specs");
        if(sceneSpecs != null){
            SceneSpecs sceneSpecsScript = sceneSpecs.GetComponent<SceneSpecs>();
            minY = sceneSpecsScript.minY;
            maxY = sceneSpecsScript.maxY;
            minX = sceneSpecsScript.minX;
            maxX = sceneSpecsScript.maxX;
        }

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

        Vector3 tpos = transform.position;
        tpos.y = Mathf.Clamp(transform.position.y, minY, maxY);
        tpos.x = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = tpos;


    }
}
