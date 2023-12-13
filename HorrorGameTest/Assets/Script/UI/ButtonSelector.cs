using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonSelector : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 offSetPos;
    [SerializeField] private Vector3 offSetScale;
    [SerializeField] private float timeToGet = .35f;
    
    private Vector3 baseTransformPos;
    private Vector3 baseTransformScale;
    private void Start()
    {
        baseTransformPos = GetComponent<Transform>().transform.position;
        baseTransformScale = GetComponent<Transform>().transform.localScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOMove(baseTransformPos + offSetPos, timeToGet);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOMove(baseTransformPos, timeToGet);
    }
}
