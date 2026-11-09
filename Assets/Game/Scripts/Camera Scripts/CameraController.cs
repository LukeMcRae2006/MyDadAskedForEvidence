using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System;

public class CameraController : MonoBehaviour
{
    [Header("Player Camera Input")]
    [SerializeField] private InputActionReference interactInput, BackInput, nextPhotoInput, prevPhotoInput, zoomInput;
    [Header("Taking Photos")]
    private Texture2D photo;
    [SerializeField] private Camera photoCamera;
    [SerializeField] private RenderTexture photoTexture;
    [SerializeField] private RawImage recentlyTakenPhoto;
    [SerializeField] private GameObject cameraFlash;
    private float zoomLevel = 60f; // Default zoom level
    [SerializeField] private float maxZoomLevel, minZoomLevel;
    private float timeBtwPhotos;
    [SerializeField] private float timeBtwPhotosSetter;
    [SerializeField] private CameraHitDetection cameraHitDetection;


    [Header("Editing Mode")]
    //actionUI is the UI to delete photos, editingUI is the UI to view photos in the editor
    [SerializeField] private GameObject editingUI;
    [SerializeField] private GameObject actionUI;
    [SerializeField] private RawImage photoToEdit;
    public int currentPhotoIndex = 0;
    string[] photos;


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

        if(timeBtwPhotos > 0)
        {
            timeBtwPhotos -= Time.deltaTime;
        }
    }

    #region Capturing Photos
    public IEnumerator TakePhoto()
    {
        if(GetCurrentPhotoCount() >= 5 && timeBtwPhotos <= 0)
        {
            Debug.Log("Photo Limit Reached, please delete photos to take more");
            yield break; // Exit the coroutine if the photo limit is reached
          
        }

        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(0.25f); // Wait for the flash effect to be visible  

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

        //Alright now that the photo is taken, tell the creature that we got a photo of him, if the creature is within the sights of the camera
        if(cameraHitDetection.isCreatureInCamera)
        {
            CreatureController.Instance.CreatureDetected();
        }

     
        // Display the captured photo
        recentlyTakenPhoto.enabled = true;
        recentlyTakenPhoto.texture = photo;
        yield return new WaitForSeconds(0.25f);
        cameraFlash.SetActive(false);

        // Keep it visible for 1 second
        yield return new WaitForSeconds(1f);

        // Hide the image
        recentlyTakenPhoto.texture = null;
        recentlyTakenPhoto.enabled = false;

        // Destroy the texture after we're done using it
        Destroy(photo);

        //set timer back to setter for photo delay
        timeBtwPhotos = timeBtwPhotosSetter;

        Debug.Log("Photo saved to: " + path);
    }


    #endregion

    #region Editing Mode    
    public void EnterEditMode()
    {
        if (cameraState == CameraState.Capture)
        {
            cameraState = CameraState.Editing;
            GetPhotoToEdit();
        }
        else
        {
            cameraState = CameraState.Capture;
            PlayerController.Instance.GoBackToPreviousState();  
        }

    }
   

    public void GetPhotoToEdit()
    {
        string photoFolder = Path.Combine(
            Application.persistentDataPath,
            "Photos"
        );

        //Directory error handler
        if (!Directory.Exists(photoFolder))
            return;

        // Get all saved PNG photos
        photos = Directory.GetFiles(photoFolder, "Evidence_*.png");

        if (photos != null && photos.Length > 0 && currentPhotoIndex < photos.Length)
        {

            // Sort by filename (oldest first)
            Array.Sort(photos);


            // Read the image bytes
            byte[] imageBytes = File.ReadAllBytes(photos[currentPhotoIndex]);

            // Create and load Texture2D
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            photoToEdit.texture = texture;

          
           

        }
        else
        {
            //Show That No Photos Exist
            photoToEdit.texture = null;
            currentPhotoIndex = 0;
        }
        

    }

    public int GetCurrentPhotoCount()
    {
        string photoFolder = Path.Combine(
           Application.persistentDataPath,
           "Photos"
       );

        //Directory error handler
        if (!Directory.Exists(photoFolder))
            return 0;

        // Get all saved PNG photos
        photos = Directory.GetFiles(photoFolder, "Evidence_*.png");

        return photos.Length;
    }

    public void IncreaseCurrentPhotoIndex()
    {
        if (currentPhotoIndex < photos.Length-1)
        {
            if (currentPhotoIndex < 5)
            {
                currentPhotoIndex++;
            }
            else
            {
                currentPhotoIndex = 0;
            }
        }
    }

    public void DecreaseCurrentPhotoIndex()
    {
       
            if (currentPhotoIndex > 0)
            {
                currentPhotoIndex--;
            }
           
        
    }
    #endregion

    #region Camera State
    private void Capture()
    {
        // Logic for capturing photos
        editingUI.SetActive(false);
        //check if the player is aiming the camera
        if(PlayerController.Instance.isAimingCamera)
        {
            //check if the player wants to zoom in or out
            zoomLevel = Mathf.Clamp(zoomLevel, minZoomLevel, maxZoomLevel) + -zoomInput.action.ReadValue<Vector2>().y * 5f;
            photoCamera.fieldOfView = zoomLevel;

        }

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

        if(nextPhotoInput.action.WasPressedThisFrame())
        {
            IncreaseCurrentPhotoIndex();
            GetPhotoToEdit();
        }

        if(prevPhotoInput.action.WasPressedThisFrame())
        {
            DecreaseCurrentPhotoIndex();
            GetPhotoToEdit();
        }

    }
       

    private void Deletion()
    {
        //Logic for deleting photos
        //Delete actionUI
        actionUI.SetActive(true);
        if (BackInput.action.WasPressedThisFrame())
        {
            cameraState = CameraState.Editing;

        }
        if(interactInput.action.WasPressedThisFrame())
        {
            //Delete the current photo
            if (photos != null && photos.Length > 0 && currentPhotoIndex < photos.Length)
            {
                File.Delete(photos[currentPhotoIndex]);
                Debug.Log("Deleted photo: " + photos[currentPhotoIndex]);
                // Refresh the photo list

                //decrease photo index so next photo shows up
                if (currentPhotoIndex > 0)
                {
                    currentPhotoIndex -= 1;
                }
                GetPhotoToEdit();
            }
        }
    }

    #endregion
}