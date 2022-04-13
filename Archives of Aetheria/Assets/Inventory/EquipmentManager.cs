using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	#region Singleton


	public static EquipmentManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<EquipmentManager>();
			}
			return _instance;
		}
	}
	static EquipmentManager _instance;

	void Awake()
	{
		_instance = this;
	}

	#endregion

	public SkinnedMeshRenderer targetMesh;
	public Equipment[] currentEquipment;
	SkinnedMeshRenderer[] currentMeshes;
	public Transform RootBone;

	public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
	public event OnEquipmentChanged onEquipmentChanged;

	Inventory inventory;
    private void Start()
    {
		inventory = Inventory.instance;

		int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numSlots];
		currentMeshes = new SkinnedMeshRenderer[numSlots];
	}

	public void Equip(Equipment newItem)
    {
		int slotIndex = (int)newItem.equipSlot;

		Equipment oldItem = null;

		if (currentEquipment[slotIndex] != null)
		{
			oldItem = currentEquipment[slotIndex];
			inventory.Add(oldItem);
		}

		if (onEquipmentChanged != null)
		{
			onEquipmentChanged.Invoke(newItem, oldItem);
		}
		currentEquipment[slotIndex] = newItem;
		SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
		newMesh.transform.parent = targetMesh.transform;

		newMesh.bones = targetMesh.bones;
		newMesh.rootBone = RootBone;
		currentMeshes[slotIndex] = newMesh;
	}

	public void Unequip (int slotIndex)
    {
		if(currentEquipment[slotIndex] != null)
        {
			if(currentMeshes[slotIndex]!= null)
            {
				Destroy(currentMeshes[slotIndex].gameObject);
            }
			Equipment oldItem = currentEquipment[slotIndex];
			inventory.Add(oldItem);

			currentEquipment[slotIndex] = null;

			if (onEquipmentChanged != null)
			{
				onEquipmentChanged.Invoke(null, oldItem);
			}
		}
    }

	public void UnequipAll()
    {
		for(int i = 0; i < currentEquipment.Length; i++)
        {
			Unequip(i);
        }
    }

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
			UnequipAll();
		}
    }
}
