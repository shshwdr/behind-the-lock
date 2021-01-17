using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUtils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pause()
    {
        Utils.Pause = !Utils.Pause;
        if (Utils.Pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    //public void playBGM()
    //{
    //    GameObject.Find("BGM").GetComponentInChildren<BMG>().playBMG();
    //    // gameObject.transform.parent.GetComponentInChildren<BMG>().playBMG();
    //}
}
