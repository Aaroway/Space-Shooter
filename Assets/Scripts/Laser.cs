using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _lspeed = 8f;
    private bool _isEnemyLaser = false;
    private bool _isMegaLaserActive = false;
    [SerializeField]
    private GameObject _megaLaser;
    private float _megaLaserDuration = 5f;
    



    void Update()
    {
        IsEnemyLaser();
        IsMegaLaserActive();
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

    void IsEnemyLaser()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void IsMegaLaserActive()
    {
        if (!_isMegaLaserActive && !_isEnemyLaser)
        {
            MoveUp();
        }
    }

    public void ActivateMegaLaser()
    {
        _isMegaLaserActive = true;
        Instantiate(_megaLaser);
        StartCoroutine(DeactivateMegaLaser());
    }

    private IEnumerator DeactivateMegaLaser()
    {
        yield return new WaitForSeconds(_megaLaserDuration);
        Destroy(this.gameObject);
        _megaLaser.SetActive(true);
        _isMegaLaserActive = false;
    }
}
