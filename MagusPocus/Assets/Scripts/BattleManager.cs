using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("UI Riferimenti")]
    [SerializeField] private GameObject battleUI;
    [SerializeField] private TextMeshProUGUI battleLogText;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private TextMeshProUGUI playerShieldsText;
    [SerializeField] private TextMeshProUGUI playerAtkText;
    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private TextMeshProUGUI enemyShieldsText;
    [SerializeField] private TextMeshProUGUI enemyAtkText;


    [SerializeField] public GameObject attackButton;

    private PlayerStats player;
    private EnemyStats enemy;
    private EnemyData data;

    private bool playerTurn = false;

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

    public void StartBattle(PlayerStats playerStats, EnemyStats enemyStats, EnemyData enemyData)
    {
        player = playerStats;
        enemy = enemyStats;
        data = enemyData;

        enemy.Init();

        // UI setup
        battleUI.SetActive(true);

        playerHpText.text = player.CurrentHealth.ToString();
        playerShieldsText.text = player.CurrentShields.ToString();
        playerHealthBar.maxValue = player.MaxHealth;
        playerHealthBar.value = player.CurrentHealth;
        playerAtkText.text = player.CurrentDamage.ToString();
       

        enemyImage.sprite = enemy.enemyData.sprite;
        enemyNameText.text = enemy.enemyData.enemyName;
        enemyHealthBar.maxValue = enemy.CurrentHealth;
        enemyHealthBar.value = enemy.CurrentHealth;
        enemyShieldsText.text = enemy.CurrentShields.ToString();
        enemyHealthText.text = enemy.CurrentHealth.ToString();
        enemyAtkText.text = enemy.CurrentDamage.ToString();


        battleLogText.text = "Il combattimento inizia!";

        // Blocca movimento player
        player.GetComponent<PlayerMovement>().enabled = false;

        // Determina chi inizia
        if (player.baseVelocity >= data.baseVelocity)
        {
            playerTurn = true;
            Debug.Log("Inizia il Player");
            OnAttackButton(); // primo attacco automatico
           
        }
        else
        {
            playerTurn = false;
            Debug.Log("Inizia il Nemico");
            StartCoroutine(EnemyAttack());
        }
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
        enemyHealthText.text = enemy.CurrentHealth.ToString();
        enemyShieldsText.text = enemy.CurrentShields.ToString();

        yield return new WaitForSeconds(0.5f);

        if (enemy.IsDead())
        {
            battleLogText.text = "Nemico sconfitto!";
            EndBattle();
        }
        else
        {
            // Pausa di 2 secondi prima che torni al player
            yield return new WaitForSeconds(1f);

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
        playerShieldsText.text = player.CurrentShields.ToString();

        yield return new WaitForSeconds(0.5f);

        if (player.CurrentHealth <= 0)
        {
            battleLogText.text = "Sei stato sconfitto...";
            EndBattle();
        }
        else
        {
            // Pausa di 2 secondi prima che torni al player
            yield return new WaitForSeconds(1f);



            attackButton.SetActive(true);

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

        if (enemy != null && enemy.IsDead())
        {
            Destroy(enemy.gameObject);
        }
    }
}