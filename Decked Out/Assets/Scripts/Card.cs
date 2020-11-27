using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Type { Buff, Magic, Physical, Transform, Debuff, Install, Merge }
public enum Target { None, First, Strongest, Random, Last, NotInfected };
public enum Abilities { None, Fire, Mechanical, Wind, Poison, Electric, Water, Angel, Time, Death, Rainbow, Sacrifice, Summoner, Boost, Frost }

public class Card : MonoBehaviour
{
    public string Name;

    public string Description;

    public Color AccentsColor;

    public int CardPrice;

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
    public int PowerUpCost = PowerUpCosts[0];

    public int starCount = 1;

    public static int CritDamageBoost = 500;

    public bool canShoot = true;
    public bool canMerge = true;

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

    public void TryActivateAbility(Enemy enemy = null, bool merged = false, GameObject card = null)
    {
        if (activateAbilityTimer < 0)
        {
            activateAbilityTimer = AbilityCooldown;
            ActivateAbility(enemy, merged, card);
        }
    }

    private void ActivateAbility(Enemy enemy, bool merged = false, GameObject card = null)
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
                case Abilities.Electric:
                    gameObject.GetComponent<ElectricAbility>().ApplyElectricAbility(enemy);
                    break;
                case Abilities.Sacrifice:
                    gameObject.GetComponent<SacrificeAbility>().AddCPFromMerge(merged);
                    break;
                case Abilities.Summoner:
                    if (card != null)
                        gameObject.GetComponent<SumonnerAbility>().SummonNewCard(merged, card.GetComponent<Card>());
                    break;
                case Abilities.Time:
                    gameObject.GetComponent<TimeAbility>().TryRewindEnemies();
                    break;
                case Abilities.Rainbow:
                    if (card != null)
                        gameObject.GetComponent<RainbowAbility>().CopyTargetCard(merged, card.GetComponent<Card>());
                    break;
                case Abilities.Fire:
                    gameObject.GetComponent<FireAbility>().ApplySplashDamage(enemy.transform);
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

    public Card CopyCard()
    {
        Card newCard = new Card();
        newCard.Name = this.Name;
        newCard.Description = this.Description;
        newCard.AccentsColor = this.AccentsColor;
        newCard.BaseAttack = this.BaseAttack;
        newCard.actualAttack = this.actualAttack;
        newCard.BaseAbility = this.BaseAbility;
        newCard.actualAbility = this.actualAbility;
        newCard.BaseAttackSpeed = this.BaseAttackSpeed;
        newCard.actualAttackSpeed = this.actualAttackSpeed;
        newCard.Ability = this.Ability;
        newCard.AbilityCooldown = this.AbilityCooldown;
        newCard.AbilityDescription = this.AbilityDescription;
        newCard.Target = this.Target;
        newCard.Type = this.Type;
        newCard.UpgradeATK = this.UpgradeATK;
        newCard.UpgradeAbility = this.UpgradeAbility;
        newCard.UpgradeATKS = this.UpgradeATKS;
        newCard.PowerUpATK = this.PowerUpATK;
        newCard.PowerUpATKS = this.PowerUpATKS;
        newCard.PowerUpAbility = this.PowerUpAbility;
        newCard.PowerUpLevel = this.PowerUpLevel;
        newCard.PowerUpCost = this.PowerUpCost;
        return newCard;
    }
}
