using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverKey : MonoBehaviour
{
    [SerializeField] GameObject silverBlock;
    private SpriteRenderer _blockSR;
    private BoxCollider2D _blockBC2D;
    void Start()
    {
        _blockSR = silverBlock.GetComponent<SpriteRenderer>();
        _blockBC2D = silverBlock.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _blockSR.enabled = false;
            _blockBC2D.enabled = false;
            Destroy(this.gameObject);
        }
    }
}
