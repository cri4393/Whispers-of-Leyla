using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseWindow : MonoBehaviour
{
    [Header("Window Setup")]
    [SerializeField] private GameObject window;

    [SerializeField] private RectTransform windowRectTransform;
    [SerializeField] private CanvasGroup windowCanvasGroup;

    public enum AnimateToDirection
    {
        Top,
        Bottom,
        Left,
        Right,
    }

    [Header("Animation Setup")]
    [SerializeField] private AnimateToDirection openDirection = AnimateToDirection.Top;
    [SerializeField] private AnimateToDirection closeDirection = AnimateToDirection.Bottom;
    [Space]
    [SerializeField] private Vector2 distanceToAnimate = new Vector2(100, 100);
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Range(0, 1f), SerializeField] private float animationDuration = 0.5f;

    private bool isOpen;
    private Vector2 initialPosition;
    private Vector2 currentPosition;

    private Vector2 upOffset;
    private Vector2 downOffset;
    private Vector2 leftOffset;
    private Vector2 rightOffset;

    private Coroutine animateWindowCoroutine;

    [Header("Helpers")]
    [SerializeField] private bool displayGizmos = true;
    //public static event Action OnOpenWindow;
    //public static event Action OnCloseWindow;

    private enum DisplayGizmosAtLocation
    {
        Open,
        Close,
        Both,
        Situational
    }

    [SerializeField] private DisplayGizmosAtLocation gizmoHandler;
    [SerializeField] private Color gizmosOpenColor = Color.green;
    [SerializeField] private Color gizmosCloseColor = Color.red;
    [SerializeField] private Color gizmosInitialLocationColor = Color.grey;
    private Vector2 windowOpenPositionForGizmos;
    private Vector2 windowClosePositionForGizmos;
    private Vector2 initialPositionForGizmos;

    private void OnValidate()
    {
        if(window != null)
        {
            windowRectTransform = window.GetComponent<RectTransform>();
            windowCanvasGroup = window.GetComponent<CanvasGroup>();
        }

        distanceToAnimate.x = Mathf.Max(0, distanceToAnimate.x);
        distanceToAnimate.y = Mathf.Max(0, distanceToAnimate.y);

        RecalculateGizmosPositions();
    }

    private void Start()
    {
        initialPosition = window.transform.position;

        InitializeOffsetPosition();
    }

    private void InitializeOffsetPosition()
    {
        upOffset = new Vector2(0, distanceToAnimate.y);
        downOffset = new Vector2(0, -distanceToAnimate.y);

        rightOffset = new Vector2(+distanceToAnimate.x, 0);
        leftOffset = new Vector2(-distanceToAnimate.x, 0);
    }

    [ContextMenu("Toggle Open Close")]
    public void ToggleOpenClose()
    {
        if (isOpen)
            CloseWindow();
        else
            OpenWindow(); 
    }

    [ContextMenu("OpenWindow")]
    public void OpenWindow()
    {
        if (isOpen) return; 

        isOpen = true;
        //OnOpenWindow?.Invoke();

        if (animateWindowCoroutine != null)
            StopCoroutine(animateWindowCoroutine);

        animateWindowCoroutine = StartCoroutine(AnimateWindow(true));
    }

    [ContextMenu("Close Menu")]
    public void CloseWindow()
    {
        if (!isOpen) return;

        isOpen = false;

        //OnCloseWindow?.Invoke();

        if (animateWindowCoroutine != null)
            StopCoroutine(animateWindowCoroutine);

        animateWindowCoroutine = StartCoroutine(AnimateWindow(false));
    }

    private Vector2 GetOffset(AnimateToDirection direction)
    {
        switch (direction)
        {
            case AnimateToDirection.Top:
                return upOffset;
            case AnimateToDirection.Bottom:
                return downOffset;
            case AnimateToDirection.Left:
                return leftOffset;
            case AnimateToDirection.Right:
                return rightOffset;
            default:
                return Vector3.zero;
        }
    }

    private IEnumerator AnimateWindow(bool open)
    {
        if (open)
            window.gameObject.SetActive(true);

        currentPosition = window.transform.position;

        float elapsedTime = 0;

        Vector2 targetPosition = currentPosition;

        if (open)
            targetPosition = currentPosition + GetOffset(openDirection);
        else
            targetPosition = currentPosition + GetOffset(closeDirection);

        while (elapsedTime < animationDuration)
        {
            float evaluationAtTime = easingCurve.Evaluate(elapsedTime / animationDuration);

            window.transform.position = Vector2.Lerp(currentPosition, targetPosition, evaluationAtTime);

            windowCanvasGroup.alpha = open
                ? Mathf.Lerp(0f, 1f, evaluationAtTime)
                : Mathf.Lerp(1f, 0f, evaluationAtTime);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        window.transform.position = targetPosition;

        windowCanvasGroup.alpha = open ? 1 : 0;
        windowCanvasGroup.interactable = open;
        windowCanvasGroup.blocksRaycasts = open;

        if (!open)
        {
            window.gameObject.SetActive(false);
            window.transform.position = initialPosition;
        }
    }

    [ContextMenu("Refresh")]
    private void Refesh()
    {
        OnValidate();
    }

    private void RecalculateGizmosPositions()
    {
        InitializeOffsetPosition();

        initialPositionForGizmos = new Vector2(window.transform.position.x, window.transform.position.y) + windowRectTransform.rect.center;
        windowOpenPositionForGizmos = initialPositionForGizmos + GetOffset(openDirection);
        windowClosePositionForGizmos = windowOpenPositionForGizmos + GetOffset(closeDirection);
    }

    private void OnDrawGizmos()
    {
        if (!displayGizmos) return;

        if (window == null) return;

        if (windowRectTransform == null) return;

        Gizmos.color = gizmosInitialLocationColor;
        Gizmos.DrawWireCube(initialPositionForGizmos, windowRectTransform.sizeDelta);

        switch (gizmoHandler)
        {
            case DisplayGizmosAtLocation.Open:
                DrawCube(windowOpenPositionForGizmos, true);
                break;

            case DisplayGizmosAtLocation.Close:
                DrawCube(windowClosePositionForGizmos, false);
                break;

            case DisplayGizmosAtLocation.Both:
                DrawCube(windowClosePositionForGizmos, false);
                DrawCube(windowOpenPositionForGizmos, true);
                break;

            default:
            case DisplayGizmosAtLocation.Situational:
                if (isOpen)
                    DrawCube(windowClosePositionForGizmos, true);
                else
                    DrawCube(windowOpenPositionForGizmos, false);
                break;
        }

        DrawIndicators();
    }
    private void DrawCube(Vector2 windowPosition, bool opens)
    {
        Gizmos.color = opens ? gizmosOpenColor : gizmosCloseColor;
        Gizmos.DrawWireCube(windowPosition, windowRectTransform.sizeDelta);
    }

    private void DrawIndicators()
    {
        Gizmos.color = gizmosOpenColor;
        Gizmos.DrawLine(initialPositionForGizmos, windowOpenPositionForGizmos);

        Gizmos.color = gizmosCloseColor;
        Gizmos.DrawLine(windowOpenPositionForGizmos, windowClosePositionForGizmos);
    }

}
