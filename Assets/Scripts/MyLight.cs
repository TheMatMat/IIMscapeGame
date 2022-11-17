using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MyLight : MonoBehaviour
{

    private SpriteRenderer sr;

    bool isFading = false;
    float waitTimeBeforeFade = 0;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        waitTimeBeforeFade = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > waitTimeBeforeFade && !isFading)
        {
            StartFade();
            isFading = true;
        }
    }

    private void StartFade()
    {
        float delay = Random.Range(0, 5);

        sr.DOFade(0.5f, 2.0f).SetDelay(delay).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 2.0f).SetDelay(delay).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
    }
}
