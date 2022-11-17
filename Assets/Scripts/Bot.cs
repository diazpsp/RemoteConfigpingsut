using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public CardPlayer player;
    public CardGameManager gameManager;
    public BotStats stats;
    
    public bool isReady = false;

    private float timer;

    int lastSelected = 0;
    Card[] cards;
    

    IEnumerator Start()
    {
        // no player
        cards = player.GetComponentsInChildren<Card>();
        while(player.isReady == false)
        {
            yield return null;
        }
        yield return new WaitUntil(()=>player.isReady);
        SetStats(this.stats);
        this.isReady = true;
    }

    void Update()
    {
        if (gameManager.State != CardGameManager.GameState.ChooseAttack)
        {
            timer = 0;
            return;
        }

        if (timer < stats.CHoosingInterval)
        {
            timer += Time.deltaTime;
            return;
        }

        timer = 0;
        ChooseAttack();
    }

    public void ChooseAttack()
    {
        var random = Random.Range(1, cards.Length);
        var selection = (lastSelected + random) % cards.Length;
        lastSelected = selection;
        player.SetChoosenCard(cards[selection]);
    }
     public void SetStats(BotStats newStats,bool restoreFullHealth = false)
    {
        this.stats = newStats;

        var newPlayerStats = new PlayerStats
        {
            MaxHealth = this.stats.MaxHealth,
            RestoreValue = this.stats.RestoreValue,
            DamageValue = this.stats.DamageValue
        };
        player.SetStats(newPlayerStats, restoreFullHealth);
    }
}
