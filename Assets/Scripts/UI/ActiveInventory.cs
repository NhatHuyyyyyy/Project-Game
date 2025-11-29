using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable() {

        if (playerControls == null) return;

        playerControls.Enable();
        playerControls.Inventory.Keyboard.performed += OnKeyboardPressed;
    }

    private void OnDisable()
    {
        if (playerControls == null) return;

        playerControls.Inventory.Keyboard.performed -= OnKeyboardPressed;
        playerControls.Disable();
    }

    private void Start()
    {
        EquipStartingWeapon();
    }

    private void OnKeyboardPressed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue) {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum) {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        if (weaponToSpawn == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
