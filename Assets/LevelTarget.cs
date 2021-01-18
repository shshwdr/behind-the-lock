using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTarget : MonoBehaviour
{
    public LevelController level;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag == "Character")
        //{

        //    Debug.Log($"Magic Spotlight revealed {other.gameObject.name}");
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
