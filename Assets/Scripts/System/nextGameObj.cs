using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextGameObj : MonoBehaviour
{
    [SerializeField] private GameObject nextGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.gameObject.SetActive(false);
            nextGameObject.SetActive(true);
        }
    }
}
