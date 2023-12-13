using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;


    //name methods as if a stranger is working on it
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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
        switch (_powerupID)
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
            case 3:
                player.ReplinishAmmunition();
                break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }
}



