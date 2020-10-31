using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Type { Buff, Magic, Physical, Transform, Debuff, Install }
public enum Target { Strongest, First, Last, Random, None };
public enum Abilities { None, Fire, Mechanical, Wind, Poison, Electric, Water, Angel, Time, Death, Rainbow, Sacrifice, Summoner, Boost, Frost }

public class Card : MonoBehaviour
{
    public string Name;

    public string Description;

    public int BaseAttack;
    private int actualAttack;

    public float BaseAttackSpeed; // In seconds
    private float actualAttackSpeed; // In seconds

    public float BaseAbility;
    private float actualAbility;

    public float AbilityCooldown; // In seconds
    
    public Abilities Ability;
    private Ability ability;

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

    public void Start()
    {
        actualAbility = BaseAbility;
        actualAttack = BaseAttack;
        actualAttackSpeed = BaseAttackSpeed;
    }

    private void upgradeCard()
    {
        actualAttack += UpgradeATK;
        actualAttackSpeed -= UpgradeATKS;
        actualAbility += UpgradeAbility;
    }

    public string CardAtk()
        => actualAttack.ToString();
    public string CardType()
        => Type.ToString();
    public string CardAtkSpeed()
        => actualAttackSpeed.ToString();
    public string CardTarget()
        => Target.ToString();
    public string CardAbility()
        => Ability.ToString();
    public string CardAbilityDmg()
        => actualAbility.ToString();
    public string CardDescription()
        => Description.ToString();
}
