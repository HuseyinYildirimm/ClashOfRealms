using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TurretManager turretManager;

    public bool weakTurretDropped;
    public bool strongTurretDropped;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableTurret draggableTurret = dropped.GetComponent<DraggableTurret>();
        draggableTurret.parentAfterDrag = transform;

        if (draggableTurret.type == "WeakTurret")
        {
            weakTurretDropped = true;
            gameManager.money -= 200f;
        }

        if (draggableTurret.type == "StrongTurret")
        {
            gameManager.money -= 500f;
            strongTurretDropped = true;
        }
    }
    public void Update()
    {
        if (transform.childCount > 0 && gameObject.CompareTag("Slot") && (weakTurretDropped || strongTurretDropped))
        {
            gameObject.SetActive(false);
        }
    }
}
