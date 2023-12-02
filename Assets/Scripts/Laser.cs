using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _lspeed = 8f;
    private bool _isEnemyLaser = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_isEnemyLaser == false) //player laser
        {
            MoveUp();
        }
        else  //enemy laser
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _lspeed * Time.deltaTime);

        if (transform.position.y > 10)
        {
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _lspeed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {  
        _isEnemyLaser = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
