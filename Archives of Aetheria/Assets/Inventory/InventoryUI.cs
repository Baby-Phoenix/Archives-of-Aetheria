using UnityEngine;

/* This object manages the inventory UI. */

public class InventoryUI : MonoBehaviour
{
	public Transform itemsParent;   // The parent object of all the items
	public GameObject inventoryUI;

	Inventory inventory;    // Our current inventory

	InventorySlot[] slots;
	void Start()
	{
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	// Check to see if we should open/close the inventory
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.G))
        {
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}
	}

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	public void UpdateUI()
	{

		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i]);
			}
			else
			{
				slots[i].ClearSlot();
			}
		}
	}

}