using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBackgroundMusicWithTransition("Stage2", 0, 2f);
        AudioManager.Instance.PlayBackgroundMusicWithTransition("Stage2", 1, 2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
