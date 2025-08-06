using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveTime = 0.2f;      // Quanto dura ogni spostamento
    public float moveDelay = 0.1f;     // Tempo da aspettare prima del prossimo spostamento se si tiene premuto
    public float gridSize = 1f;        // Grandezza della griglia

    public GameObject inventoryUI;

    public bool isUIOpen = false;

    private bool isMoving = false;
    private Vector2 currentInput = Vector2.zero;

    private void Update()
    {
        if (!isMoving)
        {
            // Controllo continuo: prende la direzione premuta più recentemente
            if (Keyboard.current.wKey.isPressed) currentInput = Vector2.up;
            else if (Keyboard.current.sKey.isPressed) currentInput = Vector2.down;
            else if (Keyboard.current.aKey.isPressed) currentInput = Vector2.left;
            else if (Keyboard.current.dKey.isPressed) currentInput = Vector2.right;
            else currentInput = Vector2.zero;

            if (currentInput != Vector2.zero)
            {
                Vector3 destination = transform.position + (Vector3)(currentInput * gridSize);
                StartCoroutine(Move(destination));
            }
        }

        openInventory();



    }

    private IEnumerator Move(Vector3 destination)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, destination, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;

        // Pausa opzionale per evitare movimenti troppo rapidi quando si tiene premuto
        yield return new WaitForSeconds(moveDelay);

        isMoving = false;
    }


   public void openInventory()
   {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isUIOpen = !isUIOpen;
            inventoryUI.SetActive(isUIOpen);
        }


    }

}