using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("UI Riferimenti")]
    public GameObject battleUI;
    public Image playerImage;
    public TextMeshProUGUI playerHpText;
    public Image enemyImage;
    public TextMeshProUGUI enemyNameText;
    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    public TextMeshProUGUI battleLogText;

    private PlayerStats player;
    private EnemyStats enemy;



    private bool playerTurn = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        battleUI.SetActive(false);
    }

    public void StartBattle(PlayerStats playerStats, EnemyStats enemyStats)
    {
        player = playerStats;
        enemy = enemyStats;

        // Inizializza nemico
        enemy.Init();

        // UI setup
        battleUI.SetActive(true);

        playerHpText.text = player.CurrentHealth.ToString();
        

        enemyImage.sprite = enemy.enemyData.sprite;
        enemyNameText.text = enemy.enemyData.enemyName;

        playerHealthBar.maxValue = player.MaxHealth;
        playerHealthBar.value = player.CurrentHealth;

        enemyHealthBar.maxValue = enemy.CurrentHealth;
        enemyHealthBar.value = enemy.CurrentHealth;

        battleLogText.text = "Il combattimento inizia!";

        // Stop movimento
        player.GetComponent<PlayerMovement>().enabled = false;

        playerTurn = true;

        

        OnAttackButton();   //test per primo attacco

    }




    public void OnAttackButton()
    {
        if (!playerTurn) return;

        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        battleLogText.text = "Attacchi il nemico!";
        enemy.TakeDamage(player.CurrentDamage);
        enemyHealthBar.value = enemy.CurrentHealth;

        yield return new WaitForSeconds(0.5f);

        if (enemy.IsDead())
        {
            battleLogText.text = "Nemico sconfitto!";
            EndBattle();
        }
        else
        {
            playerTurn = false;
            StartCoroutine(EnemyAttack());
        }
    }

    private IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(0.5f);

        battleLogText.text = enemy.enemyData.enemyName + " ti attacca!";
        player.TakeDamage(enemy.CurrentDamage);
        playerHealthBar.value = player.CurrentHealth;
        playerHpText.text = player.CurrentHealth.ToString();


        yield return new WaitForSeconds(0.5f);

        if (player.CurrentHealth <= 0)
        {
            battleLogText.text = "Sei stato sconfitto...";
            EndBattle();
        }
        else
        {
            playerTurn = true;
        }
    }

    private void EndBattle()
    {
        StartCoroutine(EndBattleRoutine());
    }

    private IEnumerator EndBattleRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        battleUI.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;

        if (enemy.IsDead() && enemy != null)
        {
            Destroy(enemy.gameObject);
        }
    }
}
