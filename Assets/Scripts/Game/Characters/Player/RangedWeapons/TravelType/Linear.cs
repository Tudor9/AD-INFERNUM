using UnityEngine;

// contine logica de traversare a unui obiect, utilizat in cazul glontului
public class Linear : MonoBehaviour
{
    public Rigidbody2D rb2d;

    private void start()
    {
        rb2d.velocity = new Vector2(0, 50);
    }
    public void Travel(Vector2 direction)
    {
        rb2d.velocity = direction;
    }
}
