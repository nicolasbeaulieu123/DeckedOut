using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAbility : MonoBehaviour
{
    [SerializeField] private float splashRange;
    [SerializeField] private GameObject pfAbilityAnimation;
    [SerializeField] private GameObject pfDeathAnimation;
    private Card card;
    private float damageModifier = 1f;
    void Start()
    {
        card = gameObject.GetComponent<Card>();
    }
    GameObject oldEnemy = null;
    public void ApplySplashDamage(Transform projectilePosition)
    {
        var hitColliders = Physics2D.OverlapCircleAll(projectilePosition.position, splashRange);
        if (oldEnemy == null)
        {
            damageModifier = 1f;
            oldEnemy = projectilePosition.gameObject;
        }
        if (hitColliders.Length > 0)
        {
            GameObject pe = Instantiate(pfAbilityAnimation);
            pe.transform.position = new Vector3(projectilePosition.transform.position.x, projectilePosition.transform.position.y, 0);
            pe.transform.SetParent(GameObject.Find("Animations").transform, true);
        }
        foreach (var hitcollider in hitColliders)
        {
            Enemy enemy = hitcollider.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.Damage(card.actualAbility * damageModifier, true);
                DamagePopup.Create(enemy.GetPosition(), (int)(card.actualAbility * damageModifier), false, ColorUtility.ToHtmlStringRGBA(card.AccentsColor));
                if (enemy.health <= 0)
                {
                    GameObject deathAnim = Instantiate(pfDeathAnimation);
                    deathAnim.transform.position = new Vector3(enemy.GetPosition().x, enemy.GetPosition().y, 45);
                    deathAnim.transform.SetParent(GameObject.Find("Animations").transform, true);
                }
            }
        }
        damageModifier = damageModifier == 2f ? 2f : damageModifier + 0.1f;
    }
}
