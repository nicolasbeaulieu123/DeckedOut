using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Type { Buff, Magic, Physical, Transform, Debuff, Install }
public enum Target { None, First, Strongest, Random, Last, NotInfected };
public enum Abilities { None, Fire, Mechanical, Wind, Poison, Electric, Water, Angel, Time, Death, Rainbow, Sacrifice, Summoner, Boost, Frost }

public class Card : MonoBehaviour
{
    public string Name;

    public string Description;

    public Color AccentsColor;

    public int BaseAttack;
    public int actualAttack;

    public float BaseAttackSpeed; // In seconds
    public float actualAttackSpeed; // In seconds

    public float BaseAbility;
    public float actualAbility;

    public Abilities Ability;
    public float AbilityCooldown; // In seconds
    public string AbilityDescription;
    
    public Target Target;
    public Type Type;

    // Bonuses per card upgrade
    public int UpgradeATK;
    public float UpgradeATKS;
    public float UpgradeAbility;

    // Bonuses per card powerup ingame
    public int PowerUpATK;
    public float PowerUpATKS;
    public float PowerUpAbility;
    public int PowerUpLevel = 1;
    private static int[] PowerUpCosts = { 100, 200, 400, 700, 0 };
    private int PowerUpCost = PowerUpCosts[0];

    public int starCount = 1;

    public static int CritDamageBoost = 500;

    public void Awake()
    {
        actualAbility = BaseAbility;
        actualAttack = BaseAttack;
        actualAttackSpeed = BaseAttackSpeed;
        activateAbilityTimer = AbilityCooldown;
    }

    public void Update()
    {
        activateAbilityTimer -= Time.deltaTime;
    }

    public void UpgradeCard()
    {
        actualAttack += UpgradeATK;
        actualAttackSpeed -= UpgradeATKS;
        actualAbility += UpgradeAbility;
    }

    public int CostPowerUp(int index)
        => PowerUpCosts[index - 1];

    public void PowerUpCard()
    {
        if (PowerUpLevel < 5)
        {
            PowerUpLevel++;
            PowerUpCost = PowerUpCosts[PowerUpLevel - 1];
            actualAttack += PowerUpATK;
            actualAttackSpeed -= PowerUpATKS;
            actualAbility += PowerUpAbility;
        }
    }

    private float activateAbilityTimer;

    public void TryActivateAbility(Enemy enemy)
    {
        if (activateAbilityTimer < 0)
        {
            activateAbilityTimer = AbilityCooldown;
            ActivateAbility(enemy);
        }
    }

    private void ActivateAbility(Enemy enemy)
    {
        if (gameObject != null)
        {
            switch (Ability)
            {
                case Abilities.Frost:
                    gameObject.GetComponent<FrostAbility>().ApplySlowEffect(enemy, this.actualAbility);
                    break;
                case Abilities.Poison:
                    gameObject.GetComponent<PoisonAbility>().ApplyPoisonEffect(enemy, this.actualAbility);
                    break;
                case Abilities.Death:
                    gameObject.GetComponent<DeathAbility>().TryInstantDeath(enemy, this.actualAbility);
                    break;
                case Abilities.Angel:
                    gameObject.GetComponent<AngelAbility>().TryExtraLifeAbility();
                    break;
            }
        }
    }

    public string CardAtk()
        => actualAttack.ToString();
    public string CardType()
        => Type.ToString();
    public string CardAtkSpeed()
        => actualAttackSpeed.ToString();
    public string CardTarget()
        => Target.ToString();
    public string CardAbilityDmg()
        => actualAbility.ToString();
    public string CardDescription()
        => Description.ToString();
}
