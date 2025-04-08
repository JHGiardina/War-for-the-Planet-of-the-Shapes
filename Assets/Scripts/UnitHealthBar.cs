using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    public float MaxHealth;
    public float MinHealth;
    public bool LookAtCamera = true;

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
        if(LookAtCamera)
        {
            transform.LookAt(Camera.main.transform.position);
        }
        
        float healthRatio = CurrentHealth / MaxHealth;
        healthSlider.value = healthRatio;
        fillImage.color = Color.Lerp(Color.red, Color.green, healthRatio);
    }
}
