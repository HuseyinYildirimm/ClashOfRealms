using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableTurret : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("MANAGERS")]
    [SerializeField] private TurretManager turretManager;
    [SerializeField] private GameManager gameManager;

    [Space(10)]

    [Header("Slots")]
    [SerializeField] private TurretSlot slot1;
    [SerializeField] private TurretSlot slot2;

    [Space(10)]

    [SerializeField] private CameraController camController;
    [HideInInspector] public Transform parentAfterDrag;
    [SerializeField] private Transform parentObj;
    public Image image;

    [Space(10)]

    public string type = null;
    [HideInInspector] public bool onBeginDrag;
    [HideInInspector] public bool onDrag;
    [HideInInspector] public bool onEndDrag;

    float turretPrice;

    public void Start()
    {
        if (type == "WeakTurret") turretPrice = 200f;

        if (type == "StrongTurret") turretPrice = 500f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        camController.isDragging = false;
        image.raycastTarget = false;
        onEndDrag = false;
        onDrag = false;
        onBeginDrag = true;

        SlotActive(true);

        turretManager.floor1Frame.SetActive(false);
        turretManager.floor2Frame.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        parentObj.GetChild(1).gameObject.SetActive(true);
        camController.isDragging = false;
        onEndDrag = false;
        onDrag = true;
        onBeginDrag = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        onEndDrag = true;
        onDrag = false;
        onBeginDrag = false;
        gameObject.SetActive(false);

        SlotActive(false);
        turretManager.floor1Frame.SetActive(true);
        turretManager.floor2Frame.SetActive(true);
    }

    void SlotActive(bool value)
    {
        if (type == "WeakTurret")
        {
            slot1.transform.GetChild(0).gameObject.SetActive(value);
            slot2.transform.GetChild(0).gameObject.SetActive(value);
        }
        if (type == "StrongTurret")
        {
            slot1.transform.GetChild(1).gameObject.SetActive(value);
            slot2.transform.GetChild(1).gameObject.SetActive(value);
        }
    }

    public void Update()
    {
        if (gameManager.money > turretPrice && !turretManager.isFullTurret &&!onBeginDrag )
        {
            image.raycastTarget = true;
        }
        if(gameManager.money < turretPrice || onDrag || turretManager.isFullTurret)
        {
            image.raycastTarget = false;
        }
    }
}
