using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuNavigation : MonoBehaviour
{
    int selectIndex = 2;
    [SerializeField] private InputActionReference scrollUp, scrollDown;
    [SerializeField] private RectTransform selectArrowTransform;
    [SerializeField] private float moveSpace = 5f;
    [SerializeField] private InputActionReference testInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    // Update is called once per frame
    void Update()
    {
        if (scrollUp.action.WasPressedThisFrame())
        {
            Debug.Log("Scroll Up");
            if (selectIndex < 2)
            {
                selectIndex++;
                selectArrowTransform.offsetMax = new Vector2(selectArrowTransform.offsetMax.x, selectArrowTransform.offsetMax.y + moveSpace);
                selectArrowTransform.offsetMin = new Vector2(selectArrowTransform.offsetMin.x, selectArrowTransform.offsetMin.y + moveSpace);
            }
           
        }

        if (scrollDown.action.WasPressedThisFrame())
        {
            if (selectIndex > 0)
            {
                selectIndex--;
                selectArrowTransform.offsetMax = new Vector2(selectArrowTransform.offsetMax.x, selectArrowTransform.offsetMax.y - moveSpace);
                selectArrowTransform.offsetMin = new Vector2(selectArrowTransform.offsetMin.x, selectArrowTransform.offsetMin.y - moveSpace);
            }
            Debug.Log("Scroll Down");
        }
        Debug.Log("Looking For Menu Navigation");


        if(testInput.action.IsPressed())
        {
            Debug.Log("Test Input Is Working");
        }
        

    }
}
