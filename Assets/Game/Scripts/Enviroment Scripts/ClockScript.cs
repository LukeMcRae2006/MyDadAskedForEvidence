using UnityEngine;
using TMPro;

public class ClockScript : MonoBehaviour
{
    [SerializeField] private TMP_Text clockTime;
    public void SetClockTime(string time)
    {
        clockTime.text = time;
    }
}
