using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private int powerupID; // 0=triple shot, 1=speed boost
    [SerializeField]
    private AudioClip _clip;

    void Update()
    {
        MovePowerUp();
    }

    void MovePowerUp()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                HandlePowerUpActivation(player);
            }

            Destroy(gameObject);
        }
    }

    void HandlePowerUpActivation(Player player)
    {
        switch (powerupID)
        {
            case 0:
                player.TripleShotActive();
                break;
            case 1:
                player.SpeedBoostActive();
                break;
            case 2:
                player.ShieldBoostActive();
                break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }
}



