using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour, IBeginDragHandler,IEndDragHandler
{
    [SerializeField] public ScrollRect autoScrollRect;
    [SerializeField] bool ableToAutoScroll = true;
    [SerializeField] public float autoScrollSpeed = 1f;
    [SerializeField] public float setTimeToAutoScroll = 3;
    private float timeToAutoScroll;

    [SerializeField] public float SetTimeToReturnToStart = 3;
    private float timeToReturnStart;

    [SerializeField] private PauseMenuUI PauseMenuUI;
    private bool timeToReturnUP;
    // Start is called before the first frame update
    void Start()
    {
        if(autoScrollRect != null ) autoScrollRect.GetComponent<ScrollRect>();
        timeToAutoScroll = setTimeToAutoScroll;
        timeToReturnStart = SetTimeToReturnToStart;
    }
    private void Update()
    {
        if (ReachedEnd())
            timeToReturnUP = ReachedEnd();
        if(ReachedTop())
            timeToReturnUP = false;
        if (PauseMenuUI.AutoScrollEnabled == true)
        {
            AutoScrollUpdate();
        }
        else
        {
            autoScrollRect.verticalScrollbar.value = 1;
        }
    }
    public void AutoScrollUpdate()
    {
        if (ableToAutoScroll)
        {
            if (timeToReturnUP)
            {
                if (timeToAutoScroll < 0)
                {
                    timeToAutoScroll += Time.unscaledDeltaTime;
                }
                else
                {
                    autoScrollRect.verticalScrollbar.value += autoScrollSpeed * Time.unscaledDeltaTime;
                }
            }
            else
            {
                if(timeToAutoScroll > 1)
                {
                    timeToAutoScroll -= Time.unscaledDeltaTime;
                }
                else
                {
                    autoScrollRect.verticalScrollbar.value -= autoScrollSpeed * Time.unscaledDeltaTime;

                }
            }
        }
        else
        {
            timeToAutoScroll = setTimeToAutoScroll;
        }
    }
    bool ReachedEnd()
    {
        return autoScrollRect.verticalScrollbar.value <= 0;
    }
    bool ReachedTop()
    {
        return autoScrollRect.verticalScrollbar.value >= 1;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ableToAutoScroll = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ableToAutoScroll = true;
    }

}
