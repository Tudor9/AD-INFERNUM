using System.Collections;
using UnityEngine;

public abstract class Characters : MonoBehaviour
{
    [SerializeField]
    protected int startingHP;
    
    [SerializeField]
    protected int maxHP;

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }

    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(int damage, float interval);
}
