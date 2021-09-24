using UnityEngine;
using UnityEngine.UI;

// clasa care se ocupa de afisarea corecta a UI-ului caracterului inamic
public class HealthBar : MonoBehaviour
{
    public int health;
    public int maxHealth;

    [SerializeField]
    private Image heart0;

    [SerializeField]
    private Image heart1;

    [SerializeField]
    private Image heart2;

    [SerializeField]
    private Hearts hearts;

    public void UpdateHealthBar()
    {
        if (health == 0)
        {
            if (heart0.enabled)
            {
                heart0.enabled = false;
            }
            if (heart1.enabled)
            {
                heart1.enabled = false;
            }
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth / 12) 
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            heart0.sprite = hearts.oneQuarterHeart;
            if (heart1.enabled)
            {
                heart1.enabled = false;
            }
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth * 2 / 12 )
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            heart0.sprite = hearts.halfHeart;
            if (heart1.enabled)
            {
                heart1.enabled = false;
            }
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth * 3 / 12)
        {
                if (!heart0.enabled)
                {
                    heart0.enabled = true;
                }
                heart0.sprite = hearts.threeQuartersHeart;
                if (heart1.enabled)
                {
                    heart1.enabled = false;
                }
                if (heart2.enabled)
                {
                    heart2.enabled = false;
                }
        }
        else if (health <= maxHealth * 4 / 12)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            heart0.sprite = hearts.fullHeart;
            if (heart1.enabled)
            {
                heart1.enabled = false;
            }
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth * 5 / 12)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
            }
            heart1.sprite = hearts.oneQuarterHeart;
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth * 6 / 12)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
                
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
            }
            heart1.sprite = hearts.halfHeart;
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth * 7 / 12)
        {        
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
            }
            heart1.sprite = hearts.threeQuartersHeart;
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth * 8 / 12)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
            }
            heart1.sprite = hearts.fullHeart;
            if (heart2.enabled)
            {
                heart2.enabled = false;
            }
        }
        else if (health <= maxHealth * 9 / 12)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
            }
            if (heart1.sprite != hearts.fullHeart)
            {
                heart1.sprite = hearts.fullHeart;
            }
            if (!heart2.enabled)
            {
                heart2.enabled = true;
            }
            heart2.sprite = hearts.oneQuarterHeart;
        }
        else if (health <= maxHealth * 10 / 12)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
            }
            if (heart1.sprite != hearts.fullHeart)
            {
                heart1.sprite = hearts.fullHeart;
            }
            if (!heart2.enabled)
            {
                heart2.enabled = true;
            }
            heart2.sprite = hearts.halfHeart;
        }
        else if (health <= maxHealth * 11 / 12)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
            }
            if (heart1.sprite != hearts.fullHeart)
            {
                heart1.sprite = hearts.fullHeart;
            }
            if (!heart2.enabled)
            {
                heart2.enabled = true;
            }
            heart2.sprite = hearts.threeQuartersHeart;
        }
        else if (health == maxHealth)
        {
            if (!heart0.enabled)
            {
                heart0.enabled = true;
                heart0.sprite = hearts.fullHeart;
            }
            if (heart0.sprite != hearts.fullHeart)
            {
                heart0.sprite = hearts.fullHeart;
            }
            if (!heart1.enabled)
            {
                heart1.enabled = true;
                heart1.sprite = hearts.fullHeart;
            }
            if (heart1.sprite != hearts.fullHeart)
            {
                heart1.sprite = hearts.fullHeart;
            }
            if (!heart2.enabled)
            {
                heart2.enabled = true;
            }
            heart2.sprite = hearts.fullHeart;
        }
    }
}