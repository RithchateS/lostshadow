using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GoldKey : MonoBehaviour
{
    [SerializeField] GameObject goldBlock;
    private SpriteRenderer _blockSR;
    private BoxCollider2D _blockBC2D;
    private Animator _blockAnimator;
    private int _count = 0;
    void Start()
    {
        _blockSR = goldBlock.GetComponent<SpriteRenderer>();
        _blockBC2D = goldBlock.GetComponent<BoxCollider2D>();
        _blockAnimator = goldBlock.GetComponent<Animator>();
    }

    void Update()
    {
        if (_blockAnimator.GetBool("Broken") != false)
        {
            _count += 1;
            if (_count > 14)
            {
                Destroy(goldBlock.gameObject);
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _blockAnimator.SetBool("Broken", true);
            _blockBC2D.enabled = false;
            Destroy(this.gameObject);
        }
    }
}
