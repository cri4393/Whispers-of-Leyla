using UnityEngine;

public class OpenCloseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuActive;
    [SerializeField] private GameObject onMenuToActive;
    [SerializeField] private bool haveAnotherGO;
    [SerializeField] private GameObject anotherMenu;

    private void Update()
    {
        if (InputManager.GetInstance().GetCloseMenu())
        {
            ActiveNewMenu();
        }
    }

    private void ActiveNewMenu()
    {
        menuActive.SetActive(false);
        onMenuToActive.SetActive(true);
        if (haveAnotherGO)
        {
            anotherMenu.SetActive(false);
        }
    }
}
