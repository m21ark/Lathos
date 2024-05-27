using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player Movement SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference playerJump { get; private set; }
    [field: SerializeField] public EventReference playerDash { get; private set; }

    [field: Header("Player Attacks SFX")]
    [field: SerializeField] public EventReference playerSwordSwing { get; private set; }
    [field: SerializeField] public EventReference playerWizardA2 { get; private set; }
    [field: SerializeField] public EventReference playerSharpshooterA1 { get; private set; }
    [field: SerializeField] public EventReference playerSorcererA2 { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference antAttack { get; private set; }

    [field: Header("Boss SFX")]
    [field: SerializeField] public EventReference bossAttack { get; private set; }

    public static FMODEvents instance { get; private set;}

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one FMODEvents in the scene");
        else instance = this;
    }
}
