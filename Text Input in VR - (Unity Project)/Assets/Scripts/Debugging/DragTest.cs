using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTest : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("DRAAAAAAAG");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("DRAAAAAAAAAAAAAAAAAG ENDDDDD");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
