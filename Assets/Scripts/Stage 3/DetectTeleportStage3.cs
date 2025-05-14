using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTeleportStage3 : MonoBehaviour
{

    public static DetectTeleportStage3 instance;

    public bool isTeleporting = false;
    // Start is called before the first frame update
    public void Start()
    {
        instance = this;
        
    }

    public void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Mengubah isTeleporting berdasarkan parameter
    public void SetTeleporting(bool value)
    {
        isTeleporting = value;
    }

}
