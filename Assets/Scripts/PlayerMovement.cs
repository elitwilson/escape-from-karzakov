using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private MovementController movementController;

    void Start()
    {
        // Get the MovementController component from the GameObject
        movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        // Handle player input
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalize the direction to ensure consistent movement speed in all directions
        moveDirection.Normalize();

        // Call the Move function on the MovementController
        if (moveDirection != Vector2.zero)
        {
            movementController.Move(moveDirection);
        }
    }
}
