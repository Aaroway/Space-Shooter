using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisruptor : MonoBehaviour
{

    private float _lspeed = 6f;

    private void Update()
    {
        MoveDown();
    }







    void MoveDown()
    {
        transform.Translate(Vector3.down * _lspeed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
                Destroy(transform.parent.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }
}
