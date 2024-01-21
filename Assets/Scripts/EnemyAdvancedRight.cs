﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvancedRight : MonoBehaviour
{
    private float _speed = 3.0f;
    [SerializeField]
    private float _fireRateMultiplier = 1.5f; // Fire rate multiplier

    private float _fireRate = 4.0f;
    private float _canFire = -1f;
    [SerializeField]
    private GameObject _disruptorPrefab;
    [SerializeField]
    private Player _player;

    private AudioSource _audioSource;
    private UI_Manager _uiManager;
    private Animator _anim;
    public byte scoreValue = 40;

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        

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

    void Update()
    {
        CalculateMovement();
        EnemyFire();
    }


    private void CalculateMovement()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.x < -13f)
        {
            transform.position = new Vector3(13f, 4.3f, 0);
        }
    }

    private void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(2f, 5f) * _fireRateMultiplier;
            _canFire = Time.time + _fireRate;

            GameObject disruptor = Instantiate(_disruptorPrefab, transform.position, Quaternion.identity);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                    TakeDamage();
                }
                break;

            case "Laser":
                Destroy(other.gameObject);
                TakeDamage();
                break;
        }
    }

    private void TakeDamage()
    {
        _anim.SetTrigger("OnEnemyAdvDestroy");
        _speed = 0;

        if (_uiManager != null)
        {
            _uiManager.OnEnemyDestroyed(scoreValue);
        }

        Destroy(GetComponent<Collider2D>());
        _audioSource.Play();
        Destroy(this.gameObject, 2.5f);
    }



}
