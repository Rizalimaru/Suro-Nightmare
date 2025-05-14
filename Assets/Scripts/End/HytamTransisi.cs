using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HytamTransisi : MonoBehaviour
{

    public SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnEnable()
    {
        StartCoroutine(WaitForAnimationAndLoadScene());
    }

    private IEnumerator WaitForAnimationAndLoadScene()
    {


        // Tunggu hingga animasi selesai
        yield return new WaitForSeconds(11f);

        // Setelah animasi selesai, muat scene baru
        sceneController.LoadScene("Mainmenu");
    }
}
