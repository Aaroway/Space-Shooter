using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAdvanced : MonoBehaviour
{
    [SerializeField]
    private float _sideSpeed = 3f;
    [SerializeField]
    private float _enemySpeed = 5.0f;
    public int scoreValue = 25;
    private Player _player;
    private Animator _anim;
    private AudioClip _explosionSoundClip;
    private AudioSource _audioSource;
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f; //x= -13 to 13, y= 4.5


    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("player is null");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        EnemyFire();
    }
}
