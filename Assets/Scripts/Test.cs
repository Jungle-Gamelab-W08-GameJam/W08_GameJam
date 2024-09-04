using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            Debug.Log("fasdfasd");

        }) );
        sequence.Join(this.transform.DOScale(new Vector3(3f, 3f, 3f), 1f));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
