using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WALL_TYPE
{
    OUTSIDE,
    HORIZONTAL,
    VERTICAL
}

public class Wall : MonoBehaviour
{

    public int wallIndex = 0;

    [SerializeField]
    private bool isVisible;
    public bool IsVisible { get;}

    [SerializeField]
    private bool isDangerous;
    public bool IsDangerous { get;}

    private SpriteRenderer sr;
    private BoxCollider2D bc2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        bc2D = GetComponent<BoxCollider2D>();
    }

    public void SetAndApplyChanges(bool _isVisible, bool _isDangerous)
    {
        this.isVisible = _isVisible;
        this.isDangerous = _isDangerous;

        if (!isVisible)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }

        if (!isDangerous)
        {
            bc2D.enabled = false;
        }
        else
        {
            bc2D.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if (!isVisible)
        {
            sr.enabled = true;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);

            sr.DOFade(1, 1.0f).SetEase(Ease.OutBounce).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("walls");
    }
}
