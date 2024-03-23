using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobHealthBar : MonoBehaviour
{
    private Transform trans;
    private ProtoMob mob;
    public Transform greenChild;
    public TextMeshProUGUI mobHealthText;

    private float initialScaleX;

    void Start()
    {
        trans = GetComponent<Transform>();
        mob = transform.parent.GetComponent<ProtoMob>();

        if (mob == null)
        {
            Debug.LogError("MobHealthBar: Mob component not found in parent object.");
            return;
        }

        // Get the initial scale of the Green child object
        initialScaleX = greenChild.localScale.x;
    }

    void Update()
    {
        if (mob == null)
            return;

        // Make the health bar face the camera
        trans.LookAt(Camera.main.transform);
        trans.Rotate(0, 180, 0);

        // Calculate the health ratio
        float healthRatio = (float)mob.health / mob.maxHealth;

        // Update the health text
        mobHealthText.text = mob.health + " / " + mob.maxHealth;

        // Resize the Green child object based on health
        Vector3 newScale = greenChild.localScale;
        newScale.x = initialScaleX * healthRatio;
        greenChild.localScale = newScale;
    }
}
