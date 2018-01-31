using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objet : MonoBehaviour {

    public SpriteRenderer srenderer;

    //BoxCollider2D bc;
    public uint itemID;
    
    //public uint item;
    void Start() {
        srenderer = GetComponent<SpriteRenderer>();
    }
   
    // Update is called once per frame
    void Update () {
		
	}
    public void setItemID(uint ID)
    {
        itemID = ID;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ItemManager.StoreToInventory(itemID, 1);
            Destroy(gameObject);
        }
    }

}
