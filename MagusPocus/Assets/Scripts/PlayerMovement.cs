using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    [Header("Movement Settings")]
    public float moveTime = 0.2f;
    public float moveDelay = 0.1f;
    public float gridSize = 1f;

    [Header("Inventory UI")]
    public GameObject inventoryUI;

    [Header("Collision Settings")]
    public LayerMask obstacleLayer; // layer degli ostacoli

    private bool isMoving = false;
    private bool isUIOpen = false;
    private Vector2 currentInput = Vector2.zero;

    private void Update()
    {
        HandleInventoryToggle();

        // Blocca il movimento se l’inventario è aperto
        if (isUIOpen) return;

        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        if (isMoving) return;

        // Prende l'input direzionale più recente
        if (Keyboard.current.wKey.isPressed) currentInput = Vector2.up;
        else if (Keyboard.current.sKey.isPressed) currentInput = Vector2.down;
        else if (Keyboard.current.aKey.isPressed) currentInput = Vector2.left;
        else if (Keyboard.current.dKey.isPressed) currentInput = Vector2.right;
        else currentInput = Vector2.zero;

        if (currentInput != Vector2.zero)
        {
            Vector3 destination = transform.position + (Vector3)(currentInput * gridSize).normalized;

            // Controllo se la cella è libera
            if (IsWalkable(destination))
            {
                StartCoroutine(MoveTo(destination));
            }
        }
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        // Controllo con un cerchio piccolo se c’è un collider ostacolo
        Collider2D hit = Physics2D.OverlapCircle(targetPosition, 0.1f, obstacleLayer);
        return hit == null;
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, destination, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.TakeTurn();
        }
            
        yield return new WaitForSeconds(moveDelay);
        isMoving = false;
    }

    private void HandleInventoryToggle()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            isUIOpen = !isUIOpen;

            if (inventoryUI != null)
                inventoryUI.SetActive(isUIOpen);
        }
    }

    // Debug per vedere il cerchio di controllo in scena
    private void OnDrawGizmosSelected()
    {
        if (currentInput != Vector2.zero)
        {
            Vector3 destination = transform.position + (Vector3)(currentInput * gridSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(destination, 0.1f);
        }
    }
}