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
    [field: SerializeField] public EventReference playerHurt { get; private set; }
    [field: SerializeField] public EventReference playerDie { get; private set; }

    [field: Header("Player Base Attacks")]
    [field: SerializeField] public EventReference playerBaseA0 { get; private set; }

    [field: Header("Player Fighter Attacks")]
    [field: SerializeField] public EventReference playerFighterA0 { get; private set; }
    [field: SerializeField] public EventReference playerFighterA1 { get; private set; }

    [field: SerializeField] public EventReference playerKnightA0 { get; private set; }
    [field: SerializeField] public EventReference playerKnightA1 { get; private set; }
    [field: SerializeField] public EventReference playerKnightA2 { get; private set; }

    [field: SerializeField] public EventReference playerBerserkerA0 { get; private set; }
    [field: SerializeField] public EventReference playerBerserkerA1 { get; private set; }
    [field: SerializeField] public EventReference playerBerserkerA2 { get; private set; }

    [field: Header("Player Ranger Attacks")]
    [field: SerializeField] public EventReference playerRangerA0 { get; private set; }
    [field: SerializeField] public EventReference playerRangerA1 { get; private set; }

    [field: SerializeField] public EventReference playerSharpshooterA0 { get; private set; }
    [field: SerializeField] public EventReference playerSharpshooterA1 { get; private set; }
    [field: SerializeField] public EventReference playerSharpshooterA2 { get; private set; }

    [field: SerializeField] public EventReference playerRogueA0 { get; private set; }
    [field: SerializeField] public EventReference playerRogueA1 { get; private set; }
    [field: SerializeField] public EventReference playerRogueA2 { get; private set; }

    [field: Header("Player Mage Attacks")]
    [field: SerializeField] public EventReference playerMageA0 { get; private set; }
    [field: SerializeField] public EventReference playerMageA1 { get; private set; }

    [field: SerializeField] public EventReference playerWizardA0 { get; private set; }
    [field: SerializeField] public EventReference playerWizardA1 { get; private set; }
    [field: SerializeField] public EventReference playerWizardA2 { get; private set; }

    [field: SerializeField] public EventReference playerSorcererA0 { get; private set; }
    [field: SerializeField] public EventReference playerSorcererA1 { get; private set; }
    [field: SerializeField] public EventReference playerSorcererA2 { get; private set; }

    [field: Header("Opening Lines")]
    [field: SerializeField] public EventReference opening0 { get; private set; }
    [field: SerializeField] public EventReference opening1 { get; private set; }
    [field: SerializeField] public EventReference opening2 { get; private set; }
    [field: SerializeField] public EventReference opening3 { get; private set; }

    [field: Header("Ending Lines")]
    [field: SerializeField] public EventReference endingFighter { get; private set; }
    [field: SerializeField] public EventReference endingRanger { get; private set; }
    [field: SerializeField] public EventReference endingMage { get; private set; }

    [field: Header("Ant SFX")]
    [field: SerializeField] public EventReference antAttack { get; private set; }
    [field: SerializeField] public EventReference antWalk { get; private set; }
    [field: SerializeField] public EventReference antHurt { get; private set; }
    [field: SerializeField] public EventReference antDie { get; private set; }

    [field: Header("Wasp SFX")]
    [field: SerializeField] public EventReference waspAttack { get; private set; }
    [field: SerializeField] public EventReference waspWalk { get; private set; }
    [field: SerializeField] public EventReference waspHurt { get; private set; }
    [field: SerializeField] public EventReference waspDie { get; private set; }

    [field: Header("Boss SFX")]
    [field: SerializeField] public EventReference bossAttack { get; private set; }
    [field: SerializeField] public EventReference bossWalk { get; private set; }
    [field: SerializeField] public EventReference bossHurt { get; private set; }
    [field: SerializeField] public EventReference bossDie { get; private set; }

    public static FMODEvents instance { get; private set;}

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one FMODEvents in the scene");
        else instance = this;
    }
}
