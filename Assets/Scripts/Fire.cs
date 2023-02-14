using UnityEngine;

public class Fire : MonoBehaviour
{
    public float direction;
    float life = 10f;

    void Update()
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime * 8f);
        life -= Time.deltaTime;

        if (life <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
