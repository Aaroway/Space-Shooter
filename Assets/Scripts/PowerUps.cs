using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;


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
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            HandlePowerUpActivation(player);

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
                player.LifeCollected();
                break;
            case 4:
                player.ReplinishAmmunition();
                break;
            case 5:
                player.Overload();
                break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }
}



