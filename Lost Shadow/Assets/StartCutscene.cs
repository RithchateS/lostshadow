using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public Animator camAnim;
    public Animator player;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            camAnim.SetBool("Cutscene",true);
            //player must can't move
            
        }
    }

    void StopCutscene()
    {
        camAnim.SetBool("Cutscene",false);
    }
}
