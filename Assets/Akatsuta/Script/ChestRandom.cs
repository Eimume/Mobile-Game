using UnityEngine;

public class ChestRandom : MonoBehaviour
{
    public WeightedRandomList<GameObject> lootTable;

    public Transform itemHolder;
    public bool CanOpen = true;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject Hand_player;
    public GameObject This_Item;

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        itemHolder = transform.Find("Item_Holder");
        Hand_player = GameObject.Find("Handle_Item");
        GetButton = transform.Find("Get_Button").gameObject;
        GetButton.SetActive(false);
    }

    private void Update()
    {
        if (CanOpen)
        {
            IsPlayerInRange();
        }
    }

    public void OpenChest()
    {
        if (CanOpen)
        {
            Debug.Log("open");
            CanOpen = false;
            GetButton.SetActive(false);
            ShowItem();
            this.enabled = false;
        }
    }
    bool IsPlayerInRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("Found");
                GetButton.SetActive(true);
                return true;
            }
        }
        GetButton.SetActive(false);
        return false;
    }
    void ShowItem()
    {
        itemHolder.localScale = Vector3.one;
        GameObject item = lootTable.GetRandom();
        This_Item = Instantiate(item, itemHolder);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


