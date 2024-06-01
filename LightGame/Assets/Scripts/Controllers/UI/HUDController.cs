using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Transform playerHealthBar;
    public Transform playerLightBar;
    public Transform bossHealthBar;
    public Image playerCooldownBarClass;
    public Image playerCooldownBarAbility;
    public GameObject playerCooldownBarDash;

    private float[] scales = new float[4];
    private Vector4 abilityLoadColor; // Color of the ability load bar
    private Vector4 mutedAbilityLoadColor; // Color of the ability load bar when not full

    private bool hasA1 = false;
    private bool hasA2 = false;
    private ProtoClass player = null;

    private void Start(){
        scales[0] = playerHealthBar.localScale.x;
        scales[1] = playerLightBar.localScale.x;
        scales[2] = bossHealthBar.localScale.x;
        scales[3] = playerCooldownBarDash.transform.localScale.x;

        abilityLoadColor = playerCooldownBarAbility.color;
        mutedAbilityLoadColor = new Color(playerCooldownBarAbility.color.r, playerCooldownBarAbility.color.g, playerCooldownBarAbility.color.b, 0.5f);
    }

    private string CooldownFormat(float playerCooldownTimer)
    {
        return string.Format("{0}", playerCooldownTimer <= 0 ? "Ready" : (Mathf.Round(playerCooldownTimer * 100f) / 100f));
    }

    private void Update()
    {
        // Get player and boss
        if (player == null) {
            player = GameLogic.instance.player;
            hasA1 = player.A1ReloadTime != 0;
            hasA2 = player.A2ReloadTime != 0;

            ToggleParentActive(playerCooldownBarClass.transform, hasA1);
            ToggleParentActive(playerCooldownBarAbility.transform, hasA2);
        }

        Boss boss = GameLogic.instance.boss;

        // Update boss health bar
        if (GameLogic.instance.isInBossBattle){
            if(boss.health > 0){
                SetBarSize(boss.health, boss.maxHealth, scales[2], bossHealthBar);
                ToggleParentActive(bossHealthBar, true);
            }else ToggleParentActive(bossHealthBar, false);
        } else ToggleParentActive(bossHealthBar, false);
        
        // Update player
        SetBarSize(player.health, player.maxHealth, scales[0], playerHealthBar); 
        SetBarSize(player.collectedLight, 100, scales[1], playerLightBar);

        // Update player's dash
        if(player.lastDashTime <= 0) playerCooldownBarDash.SetActive(false);
        else {
            playerCooldownBarDash.SetActive(true);
            SetInverseBar(player.lastDashTime, player.dashCooldown, scales[3], playerCooldownBarDash.transform);
        }

        // Update player's abilities
        if (hasA1) SetCircularFill(player.lastAttack1Time, player.A1ReloadTime, playerCooldownBarClass);
        if (hasA2) SetCircularFill(player.lastAttack2Time, player.A2ReloadTime, playerCooldownBarAbility);
    }

    private void SetBarSize(int value, int maxValue, float initialScaleX, Transform bar){
        float ratio = (float)value / maxValue;
        Vector3 newScale = bar.localScale;
        newScale.x = initialScaleX * ratio;
        bar.localScale = newScale;
    }

    private void SetInverseBar(float value, float maxValue, float initialScaleX, Transform bar){
        float ratio = value / maxValue;
        Vector3 newScale = bar.localScale;
        newScale.x = initialScaleX * (1 - ratio);
        bar.localScale = newScale;
    }

    private void SetCircularFill(float value, float maxValue, Image bar){
        bar.fillAmount = (1 - value / maxValue);

        // be abilityLoadColor but less saturated out if not full
        if(bar.fillAmount < 1) bar.color = mutedAbilityLoadColor;
        else bar.color = abilityLoadColor;
    }

    private void ToggleParentActive(Transform self, bool value){
        self.parent.gameObject.SetActive(value);
    }
}