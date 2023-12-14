using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private int _collectibleID;

    // Start is called before the first frame update
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
                HandleCollectibleActivation(player);
            }

            Destroy(gameObject);
        }

        void HandleCollectibleActivation(Player player)
        {


            switch (_collectibleID)
            {
                case 0:
                    player.LifeCollected();
                    break;
                case 1:
                    player.ReplinishAmmunition();
                    break;
                default:
                    Debug.Log("Default Collectible Value");
                    break;
            }
        }
    }
}
