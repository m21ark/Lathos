using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public GameObject hud;
    public TextMeshProUGUI hud_timer;
    public TextMeshProUGUI hud_player_health;
    public TextMeshProUGUI hud_boss_health;
    public TextMeshProUGUI hud_light;
    public TextMeshProUGUI hud_playerClassName;
    public TextMeshProUGUI hud_playerCooldowns;

    private string CooldownFormat(float playerCooldownTimer)
    {
        return string.Format("{0}", playerCooldownTimer <= 0 ? "Ready" : (Mathf.Round(playerCooldownTimer * 100f) / 100f));
    }

    void Update()
    {
        // Update timer in the format MM:SS
        float gameTime = GameLogic.instance.gameTime;
        int minutes = Mathf.FloorToInt(gameTime / 60F);
        int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
        hud_timer.text = string.Format("Time: {0:0}:{1:00}", minutes, seconds);

        // Get player and boss
        ProtoClass player = GameLogic.instance.player;
        Boss boss = GameLogic.instance.boss;

        // Update boss and player's health 
        hud_player_health.text = string.Format("Health: {0}", player.health > 0 ? player.health : 0);
        if (GameLogic.instance.isInBossBattle)
            hud_boss_health.text = string.Format("Boss Health: {0}", boss.health > 0 ? boss.health : 0);
        else hud_boss_health.text = "";

        // Update light collected and current player class
        hud_light.text = string.Format("Light: {0}", player.collectedLight);
        hud_playerClassName.text = string.Format("Class: {0}", player.getClassName());

        // Update player's cooldowns
        string baseCool = CooldownFormat(player.lastAttackTime);
        string classCool = CooldownFormat(player.lastAttack1Time);
        string AbilityCool = CooldownFormat(player.lastAttack2Time);
        string DashCool = CooldownFormat(player.lastDashTime);
        hud_playerCooldowns.text = string.Format("Base: {0}\nClass: {1}\nAbility: {2}\nDash: {3}", baseCool, classCool, AbilityCool, DashCool);
    }
}
