using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    SpriteRenderer m_sp;
    // Start is called before the first frame update
    void Start()
    {
        m_sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_sp.color = Color.gray;
    }
}
