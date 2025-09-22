using UnityEngine;


public class SoundTest : MonoBehaviour
{
    // public Button button;
    public AK.Wwise.Event sound;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sound.Post(gameObject);
        }
    }
}
