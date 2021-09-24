using UnityEngine;
using UnityEngine.UI;

// aceasta componenta se ocupa de afisarea corecta a UI-ului jucatorului
public class UI : MonoBehaviour
{
    [SerializeField]
    private ItemCount ammoItem;

    [SerializeField]
    private ItemCount healthItem;

    public HitPoints hitPoints;

    [SerializeField]
    private HighScore highScore;

    [HideInInspector]
    public Player character;

    [SerializeField]
    private Image heart0;

    [SerializeField]
    private Image heart1;

    [SerializeField]
    private Image heart2;

    [SerializeField]
    private Hearts hearts;

    [SerializeField]
    private Text ammoQuantity;
    
    [SerializeField]
    private Text healthPotionsQuantity;

    [SerializeField]
    private Text highScoreText;

    private void Update()
    {
        if (character != null)
        {
            switch (hitPoints.value)
            {
                case 0:
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
                    break;
                case 1:
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
                    break;
                case 2:
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
                    break;
                case 3:
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
                    break;
                case 4:
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
                    break;
                case 5:
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
                    break;
                case 6:
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
                    break;
                case 7:
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
                    break;
                case 8:
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
                    break;
                case 9:
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
                    break;
                case 10:
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
                    break;
                case 11:
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
                    break;
                case 12:
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
                    break;
                default:
                    break;
            }
            ammoQuantity.text = "x" + ammoItem.value;
            healthPotionsQuantity.text = "x" + healthItem.value;
            highScoreText.text = highScore.value + "";
        }
    }
}