using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    public UnitHealth targetHealth;
    public Slider healthSlider;
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 2f, 0);

    void Start()
    {
        if (targetHealth != null && healthSlider != null)
        {
            healthSlider.maxValue = targetHealth.maxHealth;
            healthSlider.value = targetHealth.maxHealth;
        }
    }

    void Update()
    {
        if (targetHealth == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = targetHealth.transform.position + offset;

        transform.rotation = Quaternion.identity;

        float currentHealth = targetHealth.GetCurrentHealth();
        healthSlider.value = currentHealth;

        float healthRatio = currentHealth / healthSlider.maxValue;
        fillImage.color = Color.Lerp(Color.red, Color.green, healthRatio);
    }
}
