using UnityEngine;

public class ControllerCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject hoverImageObject;
    [SerializeField] private GameObject selectImageObject;

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
    }
}