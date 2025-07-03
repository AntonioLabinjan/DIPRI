using UnityEngine;
//using static UnityEditor.Progress;
//using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public Item[] inventory = new Item[9];
    public Transform handMount;

    private Pickable currentItem;

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipSlot(i);
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropCurrent();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("No camera tagged as 'MainCamera'");
                return;
            }

            if (Physics.SphereCast(cam.transform.position, 0.3f, cam.transform.forward, out RaycastHit hit, 3f))
            {
                Pickable pickable = hit.collider.GetComponent<Pickable>();
                if (pickable != null && pickable.data != null)
                {
                    if (AddToInventory(pickable.data))
                    {
                        Destroy(pickable.gameObject);
                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            PrintInventory();
        }

        if (Input.GetMouseButtonDown(0) && currentItem != null)
        {
            var usable = currentItem.GetComponent<IUsable>();
            if (usable != null)
                usable.Use();
        }

    }


    public bool AddToInventory(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                Debug.Log($"Added {item.itemName} to slot {i}");
                PrintInventory();
                return true;
            }
        }
        Debug.Log("Inventory full!");
        return false;
    }

    public void RemoveFromInventory(int slot)
    {
        if (slot < 0 || slot >= inventory.Length) return;

        Debug.Log($"Removed {inventory[slot]?.itemName ?? "nothing"} from slot {slot}");
        inventory[slot] = null;
        PrintInventory();
    }

    public void EquipSlot(int slot)
    {
        if (slot < 0 || slot >= inventory.Length)
            return;

        if (inventory[slot] == null)
        {
            if (currentItem != null)
            {
                Destroy(currentItem.gameObject);
                currentItem = null;
                Debug.Log($"Unequipped item (slot {slot} was empty)");
            }
            return;
        }

        if (currentItem != null)
            Destroy(currentItem.gameObject);

        GameObject go = Instantiate(inventory[slot].prefab, handMount);
        go.transform.localPosition = Vector3.zero;
        //go.transform.localRotation = Quaternion.identity;

        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);
        foreach (Collider col in go.GetComponentsInChildren<Collider>())
            col.enabled = false;

        currentItem = go.GetComponent<Pickable>();
        Debug.Log($"Equipped {inventory[slot].itemName} from slot {slot}");
    }


    public void DropCurrent()
    {
        if (currentItem == null)
        {
            Debug.LogWarning("Tried to drop item but nothing is equipped.");
            return;
        }

        if (currentItem.data == null)
        {
            Debug.LogError("Equipped item has no data assigned!");
            return;
        }

        Vector3 dropPosition = transform.position + transform.forward;
        var dropped = Instantiate(currentItem.data.prefab, dropPosition, Quaternion.identity);
        var droppedPickable = dropped.GetComponent<Pickable>();
        if (droppedPickable != null)
            droppedPickable.isActive = currentItem.isActive; 

        Debug.Log($"Dropped {currentItem.data.itemName}");

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == currentItem.data)
            {
                inventory[i] = null;
                break;
            }
        }

        Destroy(currentItem.gameObject);
        currentItem = null;

        PrintInventory();
    }


    public void PrintInventory()
    {
        string contents = "Inventory:\n";
        for (int i = 0; i < inventory.Length; i++)
        {
            contents += $"Slot {i}: {(inventory[i] != null ? inventory[i].itemName : "[Empty]")}\n";
        }
        Debug.Log(contents);
    }
}
