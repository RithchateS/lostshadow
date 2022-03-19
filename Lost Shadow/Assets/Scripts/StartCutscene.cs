using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public Animator camAnim;
    [SerializeField] GameObject playerPrefab;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.tag == "Player")
        {
            camAnim.SetBool("Cutscene",true);

            //player must can't move during cutscene


            //after play StartAnimation must StopCutscene

        }
    }

    void StopCutscene()
    {
        camAnim.SetBool("Cutscene",false);
        
        Destroy(this.gameObject);
    }
}
