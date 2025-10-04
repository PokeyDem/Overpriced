using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DaySummaryManager : SingletonDontDestroyOnLoad<DaySummaryManager> {

    private int _moneyEarned;
    private int _moneySpend;
    private Dictionary<NPCType, int> _npcCount;
    private int GoldBalance => _moneyEarned - _moneySpend;

    [SerializeField] private TextMeshProUGUI dayCount;
    [SerializeField] private TextMeshProUGUI moneyEarned;
    [SerializeField] private TextMeshProUGUI moneySpend;
    [SerializeField] private TextMeshProUGUI moneyBalance;

    private void Awake() {
        base.Awake();
        ResetCounts();
    }

    public void ResetCounts() {
        _npcCount[NPCType.Aristocrat] = 0;
        _npcCount[NPCType.Citizen] = 0;
        _npcCount[NPCType.Commoner] = 0;
        _npcCount[NPCType.GuildMaster] = 0;
        _moneyEarned = 0;
        _moneySpend = 0;
    }

    public void TrackMoneyEarned(int money) {
        if (money == 0) {
            return;
        }
        _moneyEarned += money;
    }
    
    public void TrackMoneySpend(int money) {
        if (money == 0) {
            return;
        }
        _moneySpend += money;
    }

    public void TrackNPC(NPCType type) {
        _npcCount[type]++;
    }
}
