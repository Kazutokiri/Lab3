using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Vector3 startPosition;
    Rigidbody myRigidbody;
    public float speed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        myRigidbody = GetComponent<Rigidbody>();

    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            transform.position = startPosition;
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.useGravity = true;
        }
    }
}
