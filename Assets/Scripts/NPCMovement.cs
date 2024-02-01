using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class NPCMovement : MonoBehaviour
{
    public float BoundaryRadius = 1.5f;
    public float RandomnessAngle = 20f;

    private Vector2 moveDirection;
    private Vector2 boundaryCenter;

    private MovementController mc;
    private PhaseState currentPhaseState;

    private void Start()
    {
        boundaryCenter = transform.position;
        mc = GetComponent<MovementController>();
        moveDirection = Random.insideUnitCircle; // Set an initial random direction
        GameManager.Instance.OnPhaseChange += HandlePhaseChange;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPhaseChange -= HandlePhaseChange;
    }

    private void Update()
    {
        switch (currentPhaseState)
        {
            case PhaseState.Default:
                // Check if the current position exceeds the boundary radius
                if (Vector2.Distance(boundaryCenter, transform.position) > BoundaryRadius)
                {
                    // If so, set a new random direction
                    SetNewDirection();
                }

                mc.Move(moveDirection);
                break;
            case PhaseState.RandomConflag:
                break;
            case PhaseState.KarzakovConflag:
                if (transform.parent.gameObject.name != "Karzakov")
                {
                    mc.moveSpeed = GameManager.Instance.Player.GetComponent<MovementController>().moveSpeed + 0.5f;
                    RunFromKarzakov();                    
                } else
                {
                    // THIS IS KARZAKOV
                    mc.moveSpeed = GameManager.Instance.Player.GetComponent<MovementController>().moveSpeed + 0.75f;
                    // Calculate the vector from B to A
                    Vector2 directionToPlayer = GameManager.Instance.Player.transform.position - gameObject.transform.position;
                    moveDirection = directionToPlayer.normalized;
                }
                mc.Move(moveDirection);
                break;
        }
    }

    void SetNewDirection()
    {
        // Reflect the direction
        Vector2 directionToCenter = (boundaryCenter - (Vector2)transform.position).normalized;
        moveDirection = Vector2.Reflect(moveDirection, directionToCenter).normalized;

        // Add randomness
        float randomAngle = Random.Range(-RandomnessAngle / 2, RandomnessAngle / 2);
        moveDirection = Quaternion.Euler(0, 0, randomAngle) * moveDirection;
    }

    public void SetBoundaryCenter(Vector2 newPos)
    {
        boundaryCenter = newPos;

        // Probably need new logic here to move the NPC to the new boundary center
        moveDirection = (boundaryCenter - (Vector2)transform.position).normalized;
    }

    void RunFromKarzakov()
    {
        var k = GameManager.Instance.Karzakov.transform;

        // Calculate the vector from B to A
        Vector2 directionToKarzakov = k.position - gameObject.transform.position;

        // Normalize the vector to get the unit direction vector
        Vector2 oppositeDirection = -directionToKarzakov.normalized;

        moveDirection = oppositeDirection;
    }

    private void HandlePhaseChange(PhaseState newPhase)
    {
        // Handle the phase change here
        currentPhaseState = newPhase;
    }
}