using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enSpeed = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enSpeed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(Random.Range(-11, 11), 7.8f, 0);
        }
    }
}
