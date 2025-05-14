using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySFX("End", 0); // Memutar suara Hytam
        AudioManager.Instance.PlayBackgroundMusicWithTransition("End", 0, 1f); // Ganti dengan nama musik yang sesuai
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
