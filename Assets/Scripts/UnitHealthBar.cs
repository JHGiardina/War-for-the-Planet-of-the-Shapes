using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    public float MaxHealth;
    public float MinHealth;
    [HideInInspector] public float CurrentHealth;
    public Slider healthSlider;
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 2f, 0);

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position);

        float healthRatio = CurrentHealth / MaxHealth;
        healthSlider.value = healthRatio;
        Debug.Log(healthRatio);
        fillImage.color = Color.Lerp(Color.red, Color.green, healthRatio);
    }
}
