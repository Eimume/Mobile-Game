using UnityEngine;

public class DropItem : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject Hand_player;
    public WeaponSlot weaponSlot;
    void Start()
    {
        if (gameObject.name == "GameObject(Clone)")
        {
            this.enabled = false;
        }
        playerLayer = LayerMask.GetMask("Player");
        Hand_player = GameObject.Find("Handle_Item");
        GetButton = transform.Find("Get_Button_Item").gameObject;
        weaponSlot = FindAnyObjectByType<WeaponSlot>();
        GetButton.SetActive(false);
    }
    void Update()
    {
            IsPlayerInRange();
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
    public void MoveItemToHand()
    {
            GetButton.SetActive(false);
            weaponSlot.AddWeapon(gameObject);
            gameObject.transform.SetParent(Hand_player.transform);
            gameObject.transform.localPosition = Vector3.zero;
            this.enabled = false;
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
