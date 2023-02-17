using UnityEngine;
using UnityEngine.UI;

public enum HealthFullness
{
    Empty,
    OneQuarter,
    Half,
    ThreeQuarters,
    Full,
}

public class Heart : MonoBehaviour
{
    public Sprite oneQuarter;
    public Sprite half;
    public Sprite threeQuarters;
    public Sprite full;

    new Image renderer;

    void Start()
    {
        renderer = GetComponent<Image>();
    }

    void SetFullness(HealthFullness fullness)
    {
        switch (fullness)
        {
            case HealthFullness.Empty:
                {
                    renderer.enabled = false;
                }
                break;

            case HealthFullness.OneQuarter:
                {
                    renderer.enabled = true;
                    renderer.sprite = oneQuarter;
                }
                break;

            case HealthFullness.Half:
                {
                    renderer.enabled = true;
                    renderer.sprite = half;
                }
                break;

            case HealthFullness.ThreeQuarters:
                {
                    renderer.enabled = false;
                    renderer.sprite = threeQuarters;
                }
                break;

            case HealthFullness.Full:
                {
                    renderer.enabled = false;
                    renderer.sprite = full;
                }
                break;
        }
    }
}
