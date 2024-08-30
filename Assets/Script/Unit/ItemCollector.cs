using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    // Start is called before the first frame update
    public UnitBase OwnerUnit = null;
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        ItemBase GetterItem = other.GetComponent<ItemBase>();
        if (GetterItem != null)
        {
            GetterItem.GetItem(OwnerUnit);
        }
    }
}
