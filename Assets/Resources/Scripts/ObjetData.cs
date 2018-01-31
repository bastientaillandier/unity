using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public SpecData[] data;
    public int IndexOf_ID(uint itemID)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].id == itemID)
                return i;
        }
        return -1;
    }
}
