using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID; //0=trippleshot,1=speedboost


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            switch (_powerupID)
            {
                case 0:
                    player.TrippleShotActive();
                    break;
                case 1:
                    player.SpeedBoostActive();
                    break;
                case 2:
                    Debug.Log("Collected Shields");
                    break;
                default:
                    Debug.Log("Default Value");
                    break;
            }

            if (player != null)
            {
                player.TrippleShotActive();
            }
            Destroy(this.gameObject);
        }
        }
    }

            

