using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _lspeed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move up
        //speed variable 8
        transform.Translate(Vector3.up * _lspeed * Time.deltaTime);

        if (transform.position.y > 8)
        {
            Destroy(this.gameObject);
        }
    }
}
