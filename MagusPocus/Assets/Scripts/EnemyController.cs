using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    public float moveTime = 0.2f;
    public float gridSize = 1f;
    public LayerMask obstacleLayer;

    private Transform player;
    private bool isMoving = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<SpriteRenderer>().sprite = enemyData.sprite;
    }

    // Chiamato da PlayerMovement quando il player ha finito di muoversi
    public void TakeTurn()
    {
        if (!isMoving)
        {
            Vector3 targetPos = GetBestAvailableStep();
            StartCoroutine(MoveTo(targetPos));
        }
    }

    private Vector3 GetBestAvailableStep()
    {
        Vector3 direction = player.position - transform.position;
        Vector3 primaryDir;

        // Sceglie l'asse di movimento principale
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            primaryDir = new Vector3(Mathf.Sign(direction.x), 0, 0);
        else
            primaryDir = new Vector3(0, Mathf.Sign(direction.y), 0);

        // Prova direzione primaria
        Vector3 tryPos = transform.position + primaryDir * gridSize;
        if (IsWalkable(tryPos))
            return tryPos;

        // Se bloccato, prova direzioni alternative
        List<Vector3> alternativeDirs = new List<Vector3>()
        {
            new Vector3(0, Mathf.Sign(direction.y), 0),  // verticale
            new Vector3(Mathf.Sign(direction.x), 0, 0),  // orizzontale
            new Vector3(-primaryDir.x, -primaryDir.y, 0) // opposta
        };

        foreach (var dir in alternativeDirs)
        {
            Vector3 altPos = transform.position + dir * gridSize;
            if (IsWalkable(altPos))
                return altPos;
        }

        // Se tutte occupate, resta fermo
        return transform.position;
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
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
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Debug.Log("Preso");

        BattleManager.Instance.StartBattle(player.GetComponent<PlayerStats>(), GetComponent<EnemyStats>());


    }
}