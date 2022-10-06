using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float translationSpeed_;
    [SerializeField] float RotationSpeed_;
    Rigidbody rb_;

    void Awake()
    {
        rb_ = gameObject.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Kinematic Behaviors
        // float vAxis = Input.GetAxisRaw("Vertical");
        // float hAxis = Input.GetAxisRaw("Horizontal");

        // Vector3 moveVect = vAxis * transform.forward * Time.deltaTime * translationSpeed_;
        // transform.Translate(moveVect, Space.World);

        // float rotAngle = hAxis * RotationSpeed_ * Time.deltaTime;
        // transform.Rotate(transform.up, rotAngle);
    }

    void FixedUpdate()
    {
        float vAxis = Input.GetAxisRaw("Vertical");
        float hAxis = Input.GetAxisRaw("Horizontal");

        Vector3 moveVect = vAxis * transform.forward * Time.fixedDeltaTime * translationSpeed_;
        Vector3 newPos = rb_.position + moveVect;
        rb_.MovePosition(newPos);

        // float rotAngle = hAxis * RotationSpeed_ * Time.deltaTime;
        // rb_.MoveRotation(rotAngle);
    }
}
