using UnityEngine;

public class ControllerCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject hoverImageObject;
    [SerializeField] private GameObject selectImageObject;
    [SerializeField] private GameObject buttonImageObject;

    private void Awake()
    {
        HideAllImages();
    }

    public void ShowHoverImage()
    {
        HideAllImages();
        if (hoverImageObject != null)
        {
            hoverImageObject.SetActive(true);
        }
    }

    public void ShowSelectImage()
    {
        HideAllImages();
        if (selectImageObject != null)
        {
            selectImageObject.SetActive(true);
        }
    }

    public void HideAllImages()
    {
        if (hoverImageObject != null)
        {
            hoverImageObject.SetActive(false);
        }
        if (selectImageObject != null)
        {
            selectImageObject.SetActive(false);
        }
        if (buttonImageObject != null)
        {
            buttonImageObject.SetActive(false);
        }
    }
    
    public void ShowButtonImage()
    {
        HideAllImages();
        if (hoverImageObject != null)
        {
            buttonImageObject.SetActive(true);
        }
    }
}