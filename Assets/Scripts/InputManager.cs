using UnityEngine;

public class InputManager : MonoBehaviour
{
    // pointer pos in screen space
    private Vector3 currentPointerScreenPos;
    private bool isMousePressed;
    private bool isMouseReleased;
    private bool isTouchPressed;
    private bool isTouchReleased;
    public Vector3 PointerScreenPos => currentPointerScreenPos;
    public bool IsMousePressed => isMousePressed;
    public bool IsMouseReleased => isMouseReleased;
    public bool IsTouchPressed => isTouchPressed;
    public bool IsTouchReleased => isTouchReleased;

    // Singleton pattern
    private static InputManager instance = null;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputManager>();

                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "SingletonController";
                    instance = go.AddComponent<InputManager>();

                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isTouchPressed = Input.touchCount > 0;
        isTouchReleased = Input.touchCount == 0;
        if (isTouchPressed)
        {
            currentPointerScreenPos = Input.GetTouch(Input.touchCount - 1).position;
        }
#if UNITY_EDITOR
        isMousePressed = Input.GetMouseButton(0) || Input.GetMouseButtonDown(0);
        isMouseReleased = Input.GetMouseButtonUp(0);
        currentPointerScreenPos = Input.mousePosition;
#endif
    }
}
