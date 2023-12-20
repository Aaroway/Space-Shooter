using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private AudioSource _audioSource;



    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    


    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        Laser laser = other.GetComponent<Laser>();
        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            SpawnMan.Instance.StartSpawning();
            Destroy(this.gameObject, 0.5f);
            _audioSource.Play();
        }
        else
        {
            Debug.LogError("Explosion is null");
        }
    }
}
