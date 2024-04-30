using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHints : MonoBehaviour
{
    // Start is called before the first frame update
    private CircleCollider2D col;
    private SpriteRenderer sp;
    void Start()
    {
        col = gameObject.GetComponent<CircleCollider2D>();
        sp = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (col.enabled == true)
        {
            sp.enabled = true;
        }
        else sp.enabled = false;
    }
}
