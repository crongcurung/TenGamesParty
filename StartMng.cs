using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMng : MonoBehaviour
{
    bool isBool = false;

    void Start()
    {
        AudioMng.ins.Play_BG("Start");

        StartCoroutine(WaitStart_Coroutine());
    }




    public void Press_Start()
    {
        if (isBool.Equals(true))
        {
            AudioMng.ins.PlayEffect("Enter");       // 

            AudioMng.ins.Pause_BG();
            SceneManager.LoadScene("Main");
        }
    }


    IEnumerator WaitStart_Coroutine()
    {
        yield return new WaitForSeconds(0.1f);

        isBool = true;
    }
}
