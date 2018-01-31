using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {
    public int vitesse = 10;
    public Animator anim;
    private Rigidbody2D rb;
    public Transform checkGround;
    bool touchGround = false;
    float rayonGround = 0.3f;
    public LayerMask ground;
    public SpriteRenderer srenderer;
    public GameObject objet;


    public GameObject projectile;
    public Transform emmitter;
    public float firingRange = 5;

    public ItemData itemData;
    public string objetListePath;
    //0x010FF, 0x0101FB
    // Use this for initialization
    void Start ()
    {
        itemData = new ItemData();
        objetListePath = Application.dataPath + "/ObjetListe.json";
        if(File.Exists(objetListePath))
        { 
           string dataAsJson = File.ReadAllText(objetListePath);
           itemData = JsonUtility.FromJson<ItemData>(dataAsJson);
        }
        objet = Resources.Load<GameObject>("Prefabs/Objets/Objet");
        rb = GetComponent<Rigidbody2D>();
        srenderer = this.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        touchGround = Physics2D.OverlapCircle(checkGround.position, rayonGround, ground);
        anim.SetBool("Ground", touchGround);

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetButton("right") && Input.GetButton("left"))
        {
            anim.SetBool("Courir", false);
        }
        else if (Input.GetButton("right"))
        {
            //srenderer.flipX = false;
            gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            anim.SetTrigger("RetourJump");
            anim.SetBool("Courir", true && touchGround);
        }
        else if (Input.GetButton("left"))
        {
            //srenderer.flipX = true;
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.SetTrigger("RetourJump");
            anim.SetBool("Courir", true && touchGround);
        }
        else if (Input.GetButtonUp("right") || Input.GetButtonUp("left"))
        {
            anim.SetBool("Courir", false);
        }
        else if (Input.GetButtonDown("CreateItem0"))
        {
            ItemManager.ResetInventory();

        }
        else if (Input.GetButtonDown("CreateItem1"))
        {
            CreateItem(objet, itemData.data[0].path, itemData.data[0].id, new Vector2(4.91f, 1.28f), new Vector3(1.0f, 1.0f, 1.0f));

        }
        else if (Input.GetButtonDown("CreateItem2"))
        {
            CreateItem(objet, itemData.data[1].path, itemData.data[1].id, new Vector2(4.91f, 0.28f), new Vector3(1.0f, 1.0f, 1.0f));
        }
        else if (Input.GetButtonDown("CreateItem3"))
        {
            CreateItem(objet, itemData.data[2].path, itemData.data[2].id, new Vector2(4.91f, -1.28f), new Vector3(1.0f, 1.0f, 1.0f));
        }
        else if (Input.GetButtonDown("Inventory"))
        {
            SceneManager.LoadScene("Inventory");

        }

        if (touchGround && Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("RetourJump");
            anim.SetBool("Courir", false);
            rb.AddForce(new Vector2(0, 1700));
        }
        if (Input.GetButtonDown("Fire1") && (stateInfo.IsTag("Iddle") || stateInfo.IsTag("Jump")))
        {
            if (touchGround&&!stateInfo.IsTag("Jump"))
            {
                anim.SetTrigger("Attaque");
                anim.SetTrigger("Retour");
            }
            if (!touchGround && !stateInfo.IsTag("Iddle"))
            {
                anim.SetTrigger("AttaqueJump");
                anim.SetTrigger("RetourJump");
            }
        }
        if (Input.GetButtonDown("Fire2") && (stateInfo.IsTag("Iddle") || stateInfo.IsTag("Jump")))
        {
            Vector3 position = emmitter.TransformPoint(Vector3.up * 0.5f);
            GameObject projectileInstance = (GameObject)Instantiate(projectile, position, emmitter.rotation);
            if (touchGround && !stateInfo.IsTag("Jump"))
            {
                anim.SetTrigger("Throw");
                anim.SetTrigger("RetourThrow");
            }
            if (!touchGround && !stateInfo.IsTag("Iddle"))
            {
                anim.SetTrigger("ThrowJump");
                anim.SetTrigger("RetourThrowJump");
            }
        }
        if (Input.GetButtonDown("damage"))
        {
            GameManager.Life = GameManager.Life - 20;
            print(GameManager.Life);
        }

        }

    // Update is called once per frame
    void Update () {

        transform.Translate(Vector3.right * Input.GetAxis("left") * vitesse * Time.deltaTime);
        transform.Translate(Vector3.right * Input.GetAxis("right") * vitesse * Time.deltaTime);
    }
    void CreateItem(GameObject item, string item_path, uint itemID, Vector2 location,Vector3 sizeScale)
    {
        SpriteRenderer spirteObjet;
        spirteObjet = objet.GetComponent<SpriteRenderer>();
        spirteObjet.sprite = Resources.Load<Sprite>(item_path);
        objet.GetComponent<Objet>().setItemID(itemID);
        spirteObjet.transform.localScale = sizeScale;
        Instantiate(item, location, Quaternion.identity);
    }
}
