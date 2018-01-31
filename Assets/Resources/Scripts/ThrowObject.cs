using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {

    public float speed = 0.5f;
    public float force = 5;
    public GameObject player;
    [HideInInspector]
    public float range = 10;
    private float _distance;
    private Rigidbody2D rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (GameObject.FindGameObjectWithTag("Player").transform.rotation.y==0)
        {
            rb.AddForce(new Vector2(force, force));
            Debug.Log("rotation 0");
        }
        else
        {
            rb.AddForce(new Vector2(-force, force));
            Debug.Log("rotation 180");
        }
    }
	
	// Update is called once per frame
	void Update () {
        _distance += speed * Time.fixedDeltaTime;
        if (_distance > range)
        {
            Destroy(gameObject);
        }
    }
}
