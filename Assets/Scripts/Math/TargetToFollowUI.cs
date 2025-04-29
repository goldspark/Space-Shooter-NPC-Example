using UnityEngine;
using UnityEngine.EventSystems;

public class TargetToFollowUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public static bool PointOnTarget = false;

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointOnTarget = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       PointOnTarget = false;
    }

}
