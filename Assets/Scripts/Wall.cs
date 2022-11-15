using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private bool isVisible;
    public bool IsVisible { get; set; }

    [SerializeField]
    private bool isDangerous;
    public bool IsDangerous { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ApplyChanges()
    {
        if (!isVisible)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (!isDangerous)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
