using UnityEngine;

public class TVScript : MonoBehaviour, IInteractable
{
    private bool isTvOn = false;
    [SerializeField] private Material offMaterial, staticMaterial;
    [SerializeField] private MeshRenderer screenRender;
    [SerializeField] private Vector2 speed = new Vector2(0.5f, 0f);
    [SerializeField] private Vector2 maxOffset = new Vector2(1f, 0f);
    [SerializeField] private AudioSource staticAudio;
    [SerializeField] private GameObject tvLight;
    public void Interact()
    {
        isTvOn = !isTvOn;
        
    }

    void Update()
    {
        if (isTvOn == true)
        {
            float offsetX = maxOffset.x > 0 ? Mathf.PingPong(Time.time * speed.x, maxOffset.x) : 0f;
            float offsetY = maxOffset.y > 0 ? Mathf.PingPong(Time.time * speed.y, maxOffset.y) : 0f;

            Vector2 currentOffset = new Vector2(offsetX, offsetY);
            screenRender.material = staticMaterial;
            staticMaterial.mainTextureOffset = currentOffset;
            if (staticAudio.isPlaying == false)
            {
                staticAudio.Play();
            }
            tvLight.SetActive(true);
        }
        else
        {
            screenRender.material = offMaterial;
            staticAudio.Stop(); 
            tvLight.SetActive(false);
        }
    }
}
