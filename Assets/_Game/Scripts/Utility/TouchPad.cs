using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour
{
    public static TouchPad instance;

    public RectTransform analogBackground;
    public RectTransform analogCenter;

    public bool Down { get; private set; }
    public bool Up { get; private set; }
    public bool Drag { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _activeAnalog = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Down = true;
            SetAnalogPosition((Input.mousePosition / Screen.height) * 1000f, true);
        }
        else if (Input.GetMouseButton(0))
        {
            Drag = true;
            SetAnalogPosition((Input.mousePosition / Screen.height) * 1000f);
        }
        else
        {
            SetAnalogPosition((Input.mousePosition / Screen.height) * 1000f, true);
            Down = false;
            Drag = false;
            Up = true;
        }
    }

    private void LateUpdate()
    {
        Down = Up = false;
    }

    public Vector3 velocityDirection
    {
        get
        {
            Vector2 tempVect = (analogCenter.position - analogBackground.position) / _analogRadius;
            return new Vector3(tempVect.x, 0, tempVect.y);
        }
    }

    private void SetAnalogPosition(Vector2 position, bool defaultPosition = false)
    {
        if (defaultPosition)
        {
            analogBackground.position = position;
            analogCenter.position = position;
            return;
        }

        analogCenter.position = position;
        Vector3 distance = analogBackground.position - analogCenter.position;
        if (distance.magnitude > _analogRadius)
        {
            analogBackground.position = analogCenter.position + distance.normalized * _analogRadius;
        }
    }

    private float _analogRadius => analogBackground.rect.width / 2;
    private bool _activeAnalog
    {
        set
        {
            analogBackground.gameObject.SetActive(value);
            analogCenter.gameObject.SetActive(value);
        }
    }
}