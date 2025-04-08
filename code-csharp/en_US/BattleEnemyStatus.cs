using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemyStatus : MonoBehaviour
{
    public BattleActiveEnemy ActiveEnemy;

    [SerializeField]
    private Image HealthBar;

    [SerializeField]
    private Image SpareBar;

    [SerializeField]
    private TextMeshProUGUI Health_Text;

    [SerializeField]
    private TextMeshProUGUI Spare_Text;

    public TextMeshProUGUI EnemyName;

    public void UpdateStatusGraphics()
    {
        base.gameObject.name = ActiveEnemy.EnemyInBattle.EnemyName + "_status";
        HealthBar.fillAmount = ActiveEnemy.Enemy_Health / ActiveEnemy.Enemy_MaxHealth;
        SpareBar.fillAmount = ActiveEnemy.Enemy_MercyAmount / 100f;
        Health_Text.text = Mathf.Ceil(ActiveEnemy.Enemy_Health / ActiveEnemy.Enemy_MaxHealth * 100f) + "%";
        Spare_Text.text = ActiveEnemy.Enemy_MercyAmount + "%";
        EnemyName.color = Color.white;
        if (ActiveEnemy.Enemy_MercyAmount >= 100f)
        {
            EnemyName.color = Color.yellow;
        }
        if (ActiveEnemy.Enemy_IsTired)
        {
            EnemyName.color = Color.blue;
        }
        EnemyName.text = ActiveEnemy.EnemyInBattle.EnemyName;
    }
}
