using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager {

    public const int LIMIT_NB_UNIT = 0xFF;
    //public const int LIMIT_NB_TYPE = 0xFF;
    public const int LIMIT_NB_ITEM = 0xFF;

    public const uint ITEM_ID = 0x00FF;
    //public const uint ITEM_TYPE = 0x00FF00;
    public const uint ITEM_UNIT = 0xFF00;

    public const int INDEX_ITEM_ID = 2;
  //  public const int INDEX_ITEM_TYPE = 2;
    public const int INDEX_ITEM_UNIT = 0;

    public const int LEN_ITEM_ID = 2;
    //public const int LEN_ITEM_TYPE = 2;
    public const int LEN_ITEM_UNIT = 2;
    public const int LEN_ITEM_CODE = 4;
    public const int LEN_DELIMITER = 1;
    public const int LEN_TOTAL = LEN_ITEM_CODE + LEN_DELIMITER;



    public static uint GetItemID(uint item)
    {
        return (item & ITEM_ID) ;
    }
    /*
    public uint GetItemType(uint item)
    {
        return (item & ITEM_TYPE);
    }*/
    public static uint GetItemUnit(uint item)
    {
        return (item & ITEM_UNIT) >> (INDEX_ITEM_UNIT + INDEX_ITEM_ID) * 4;
    }
    public static string EncodeItem(uint itemID, uint itemUnit)
    {
        uint item = 0;
       // uint itemType = 1;
        //itemType <<= (LEN_ITEM_TYPE+LEN_ITEM_ID) * 4;
        itemUnit <<= (INDEX_ITEM_UNIT + INDEX_ITEM_ID) * 4;
        item = itemUnit + itemID;
        return item.ToString("X4");
    }
    public static void ResetInventory()
    {
        PlayerPrefs.SetString("Inventory", "");
    }
    public static string LoadInventoryString()
    {
        return PlayerPrefs.GetString("Inventory");
    }
    public static string StoreToInventory(uint itemID, uint itemUnit)
    {
        string inventoryList;
        
        int curItem; //current item.
        int curItemIndex; //current item index.

        inventoryList = PlayerPrefs.GetString("Inventory");


      //  UnityEditor.EditorUtility.DisplayDialog("Test1", inventoryList, "OK", "CANCEL");
        curItem = FindItem(inventoryList, itemID);
        curItemIndex = curItem * LEN_TOTAL;

     //   UnityEditor.EditorUtility.DisplayDialog("Test", "Current:"+ curItem.ToString(), "OK", "CANCEL");
        
        if (curItem == -1)
        {
            if (inventoryList.Length == 0)
            {
                inventoryList += EncodeItem(itemID, itemUnit);
            }
            else
            {
                inventoryList += ',' + EncodeItem(itemID, itemUnit);
            }
        }
        else
        {
             
            string oldItemUnit = inventoryList.Substring(curItemIndex + INDEX_ITEM_UNIT, LEN_ITEM_UNIT);
            string oldItemCode = inventoryList.Substring(curItemIndex, LEN_ITEM_CODE);
            string curItemCode;
            


            itemUnit += uint.Parse(oldItemUnit,System.Globalization.NumberStyles.AllowHexSpecifier);
            itemUnit = (itemUnit <= LIMIT_NB_UNIT ? itemUnit : LIMIT_NB_UNIT); //Limiter le nombre d'objets.
            curItemCode = itemUnit.ToString("X2") + itemID.ToString("X2");
            inventoryList = inventoryList.Replace(oldItemCode, curItemCode);
        }
        PlayerPrefs.SetString("Inventory", inventoryList);
        return inventoryList;
    }
    public string StoreToInventory(string itemEncode)
    {
        uint itemID = uint.Parse(itemEncode.Substring(INDEX_ITEM_ID,LEN_ITEM_ID), System.Globalization.NumberStyles.AllowHexSpecifier);
        uint itemUnit = uint.Parse(itemEncode.Substring(INDEX_ITEM_UNIT, LEN_ITEM_UNIT), System.Globalization.NumberStyles.AllowHexSpecifier);

        return StoreToInventory(itemID, itemUnit);
    }
    public static int FindItem(string inventoryList, uint itemID)
    {
        int iItem = 0;
        int length = inventoryList.Length;
        //012345,789012,456789,123456,8
        for (int i = 0; i < length; i += (LEN_TOTAL))
        {
            if(inventoryList.Substring(i + INDEX_ITEM_ID, LEN_ITEM_ID).CompareTo(itemID.ToString("X2")) == 0)
                return iItem;
            iItem ++;
        }
        return -1;
    }
    public static uint[] LoadFromInventory()
    {
        string inventoryList;
        string[] strItems;
        int nbItems;
        uint[] int1DInventory;
        inventoryList = PlayerPrefs.GetString("Inventory");
        if(inventoryList.Length > 0)
        { 
            strItems = inventoryList.Split(',');
            nbItems = strItems.Length;

            if (nbItems > 0)
            {
                int1DInventory = new uint[nbItems];
                for (int i = 0; i < nbItems; i++)
                    int1DInventory[i] = uint.Parse(strItems[i],System.Globalization.NumberStyles.HexNumber);
                return int1DInventory;
            }
        }
        return null;
    }
}
