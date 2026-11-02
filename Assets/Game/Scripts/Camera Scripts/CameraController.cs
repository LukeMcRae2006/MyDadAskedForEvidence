using System.IO;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera photoCamera;
    [SerializeField] private RenderTexture photoTexture;

    public void TakePhoto()
    {
        Texture2D photo = new Texture2D(
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

        Destroy(photo);

        Debug.Log("Photo saved to: " + path);
    }
}