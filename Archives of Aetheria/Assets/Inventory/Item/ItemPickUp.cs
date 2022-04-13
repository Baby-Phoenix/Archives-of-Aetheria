using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }
    void PickUp()
    {

        Debug.Log("Picking up the item."+item.name);
        bool wasPickedUp=Inventory.instance.Add(item);
        //Add to inventory
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
