using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {
    const float WIDTH = 1.65f;
    const float HEIGHT = 1.65f;
    const int NB_COLUMN = 4;
    const int NB_LINE = 4;
    public const int NB_CELL = NB_COLUMN * NB_LINE;
    const float X = 0.0f - (WIDTH * NB_COLUMN / 2.0f);
    const float Y = 0.0f - (HEIGHT * NB_LINE / 2.0f);
    public GameObject cell;
    public GameObject objet;
    public ItemData itemData;
    public uint[] curItems;
    bool bListExistence;
    public Vector3 vector3WorldPos;
    public Vector2 vector2LabelPos;
    public Vector2 vector2Pos;
    public Vector2 vector2LabelSize;
    public Vector3 vector3GridScale;
    public Vector3 vector3ItemScale;
    public GUIStyle style;
    Rect rectLabel;
    // Use this for initialization
    void Start()
    {
        itemData = new ItemData();
        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.black;
        rectLabel = new Rect();
        curItems = ItemManager.LoadFromInventory();
        string itemListPath = Application.dataPath + "/ObjetListe.json";
        bListExistence = File.Exists(itemListPath);
        
        if (bListExistence)
        {
            string dataAsJson = File.ReadAllText(itemListPath);
            itemData = JsonUtility.FromJson<ItemData>(dataAsJson);
            cell = Resources.Load<GameObject>("Inventory/Grille");
            objet = Resources.Load<GameObject>("Prefabs/Objets/Objet");
            vector2Pos = new Vector2();
            vector3WorldPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.nearClipPlane)); 
            print(vector3WorldPos);
            GUIContent content = new GUIContent("x999");
            vector2LabelSize = style.CalcSize(content);
            vector3GridScale = new Vector3(0.5f, 0.5f, 0.5f);
            vector3ItemScale = new Vector3(0.8f, 0.8f, 0.8f);

        }
    }

    void OnGUI()
    {

        if (bListExistence)
        {
            uint itemID;
            uint itemUnit;
            int indexFromList;
            int x0 = Camera.main.pixelWidth;
            int y0 = Camera.main.pixelHeight;
            
            //Texture2D textureToDisplay = new Texture2D(;
            //GUI.color = Color.black;
            for (int i = 0; i < NB_CELL; i++)
            { 
                vector2Pos.Set(X + (WIDTH * (i % NB_COLUMN)), Y + (HEIGHT * ((NB_CELL - (i + 1)) / NB_COLUMN)));
                
                CreateObject(cell, "Sprites/Inventory/Grille", vector2Pos, vector3GridScale);
                
                if (curItems != null && i < curItems.Length)
                {
                    itemID = ItemManager.GetItemID(curItems[i]);
                    itemUnit = ItemManager.GetItemUnit(curItems[i]);
                    indexFromList = itemData.IndexOf_ID(itemID);
                    if (indexFromList != -1)
                    {
                        vector2LabelPos.Set(vector2Pos.x, Y + (HEIGHT * (i / NB_COLUMN)));
                        vector2LabelPos = Camera.main.WorldToScreenPoint(vector2LabelPos);
                        CreateObject(objet, itemData.data[indexFromList].path, vector2Pos, vector3ItemScale);
                        rectLabel.Set(vector2LabelPos.x - (WIDTH * 50 / 2), vector2LabelPos.y + (HEIGHT * 50 / 2), WIDTH * 50, HEIGHT * 50);
                        //ShadowAndOutline.DrawOutline(rectLabel, "x" + itemUnit.ToString("D3"), style , Color.black, Color.white, 1);
                        GUI.Box(rectLabel, "x" + itemUnit.ToString("D3"));

                    }
                }
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetButtonDown("Inventory"))
        {
            SceneManager.LoadScene("Scene1");

        }
    }
    void CreateObject(GameObject obj, string image_path, Vector2 location, Vector3 sizeScale)
    {
        SpriteRenderer spirteObjet;
        spirteObjet = obj.GetComponent<SpriteRenderer>();
        spirteObjet.sprite = Resources.Load<Sprite>(image_path);
        spirteObjet.transform.localScale = sizeScale;
        Instantiate(obj, location, Quaternion.identity);
    }
}
