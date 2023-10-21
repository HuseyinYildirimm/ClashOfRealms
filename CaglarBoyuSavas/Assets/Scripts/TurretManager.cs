using UnityEngine;
using DG.Tweening;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject floor1;
    [SerializeField] private GameObject floor2;

    public GameObject turretsButton;
    public GameObject turrets;

    [SerializeField] private GameObject turretSlot1;
    [SerializeField] private GameObject turretSlot2;
    public bool isFullTurret;

    [SerializeField] private GameObject floor1Change;
    [SerializeField] private GameObject floor2Change;

    public GameObject floor1Frame;
    public GameObject floor2Frame;

    bool weakTurret;
    bool strongTurret;
    bool highweakTurret;
    bool highstrongTurret;

    public void Start()
    {
        floor1Change.SetActive(false);
        floor2Change.SetActive(false);

        floor1Change.transform.DOMoveY(floor1Change.transform.position.y + .3f, 1.5f).
            SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBack);

        floor2Change.transform.DOMoveY(floor2Change.transform.position.y + .3f, 1.5f).
           SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBack);
    }

    public void Update()
    {
        TurretPlacement();

        if (gameManager.money > 500)
        {
            if (weakTurret && floor1Change != null) floor1Change.SetActive(true);

            if (highweakTurret && floor2Change != null) floor2Change.SetActive(true);
        }
        else
        {
            if (floor1Change != null) floor1Change.SetActive(false);

            if (floor2Change != null) floor2Change.SetActive(false);
        }
    }

    void TurretPlacement()
    {
        if (turretSlot1.GetComponent<TurretSlot>().weakTurretDropped && !weakTurret)
        {
            floor2.SetActive(true);
            floor1.transform.GetChild(0).gameObject.SetActive(true);
            turretSlot2.SetActive(true);

            weakTurret = true;
        }
        else if (turretSlot1.GetComponent<TurretSlot>().strongTurretDropped && !strongTurret)
        {
            floor2.SetActive(true);
            floor1.transform.GetChild(1).gameObject.SetActive(true);
            turretSlot2.SetActive(true);

            strongTurret = true;
        }

        if (turretSlot2.GetComponent<TurretSlot>().weakTurretDropped && !highweakTurret)
        {
            floor2.transform.GetChild(0).gameObject.SetActive(true);
            turretSlot2.SetActive(false);

            highweakTurret = true;
        }
        else if (turretSlot2.GetComponent<TurretSlot>().strongTurretDropped && !highstrongTurret)
        {
            floor2.transform.GetChild(1).gameObject.SetActive(true);
            turretSlot2.SetActive(false);
        }
        if (turretSlot1.transform.childCount > 3 && turretSlot2.transform.childCount > 3)
        {
            isFullTurret = true;
        }
        else isFullTurret = false;
    }

    public void TurretUpgrade(GameObject destroyObj)
    {
        gameManager.money -= 500f;
        Destroy(destroyObj);
    }

}

