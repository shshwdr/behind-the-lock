using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : Singleton<SFXPlayer>
{

    public void playSound(string soundName)
    {
        AudioClip clip = (AudioClip)Resources.Load(soundName);
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
