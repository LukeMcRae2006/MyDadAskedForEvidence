using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Player Camera Input")]
    [SerializeField] private InputActionReference interactInput, BackAction;
    [Header("Taking Photos")]
    private Texture2D photo;
    [SerializeField] private Camera photoCamera;
    [SerializeField] private RenderTexture photoTexture;
    [SerializeField] private RawImage recentlyTakenPhoto;
    

    [Header("Editing Mode")]
    //actionUI is the UI to delete photos, editingUI is the UI to view photos in the editor
    [SerializeField] private GameObject editingUI;
    [SerializeField] private GameObject actionUI;
   

    [Header("Camera State")]
    public CameraState cameraState;

    public enum CameraState
    {
        Capture,
        Editing,
        Deletion
    }

    void Update()
    {
        switch (cameraState)
        {
            case CameraState.Capture:
                // Handle capture mode logic
                Capture();
                break;
            case CameraState.Editing:
                // Handle editing mode logic
                Editing();
                break;
            case CameraState.Deletion:
                // Handle deletion mode logic
                Deletion();
                break;
        }
    }

    #region Capturing Photos
    public void TakePhoto()
    {
        photo = new Texture2D(
            photoTexture.width,
            photoTexture.height,
            TextureFormat.RGB24,
            false
        );

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = photoTexture;

        photo.ReadPixels(
            new Rect(0, 0, photoTexture.width, photoTexture.height),
            0,
            0
        );

        photo.Apply();

        RenderTexture.active = previous;

        byte[] imageBytes = photo.EncodeToPNG();

        string photoFolder = Path.Combine(
            Application.persistentDataPath,
            "Photos"
        );

        Directory.CreateDirectory(photoFolder);

        string fileName = "Evidence_" +
            System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        string path = Path.Combine(photoFolder, fileName);

        File.WriteAllBytes(path, imageBytes);

        //add an effect to show the image
        StartCoroutine(ShowRecentlyCapturedPhoto(photo));

        

        Debug.Log("Photo saved to: " + path);
    }


    private IEnumerator ShowRecentlyCapturedPhoto(Texture photo)
    {
        // Display the captured photo
        recentlyTakenPhoto.enabled = true;
        recentlyTakenPhoto.texture = photo;

        // Keep it visible for 1 second
        yield return new WaitForSeconds(1f);

        // Hide the image
        recentlyTakenPhoto.texture = null;
        recentlyTakenPhoto.enabled = false;

        // Destroy the texture after we're done using it
        Destroy(photo);

    }


    #endregion

    #region Editing Mode    
    public void EnterEditMode()
    {
        if (cameraState == CameraState.Capture)
        {
            cameraState = CameraState.Editing;
        }
        else
        {
            cameraState = CameraState.Capture;
            PlayerController.Instance.GoBackToPreviousState();  
        }

    }
    #endregion

    #region Camera State
    private void Capture()
    {
        // Logic for capturing photos
        editingUI.SetActive(false);
        
    }

    private void Editing()
    {
        //enable edit viewer UI
        editingUI.SetActive(true);
        //disable actionUI
        actionUI.SetActive(false);
        if (interactInput.action.WasPressedThisFrame())
        {
            cameraState = CameraState.Deletion;

        }

    }
       

    private void Deletion()
    {
        //Logic for deleting photos
        //Delete actionUI
        actionUI.SetActive(true);
        if (BackAction.action.WasPressedThisFrame())
        {
            cameraState = CameraState.Editing;

        }
    }

    #endregion
}