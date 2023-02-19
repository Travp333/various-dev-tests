using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float jumpForce;
    CharacterController controller;
    private Vector3 moveDirection;
    [SerializeField]
    float gravityScale;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal")* moveSpeed, moveDirection.y, Input.GetAxis("Vertical")* moveSpeed);
        if (controller.isGrounded && Input.GetButtonDown("Jump")){
            moveDirection.y = jumpForce;
        }

        moveDirection.y = moveDirection.y - (9.81f * gravityScale);

        controller.Move(moveDirection * Time.deltaTime);
    }
}
