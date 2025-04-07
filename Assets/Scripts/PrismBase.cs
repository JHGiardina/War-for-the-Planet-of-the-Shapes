using UnityEngine;
using UnityEngine.SceneManagement;

public class PrismBase : MonoBehaviour
{
    [HideInInspector] public PrismUnitBehaviour prismBehavior;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prismBehavior = GetComponentInChildren<PrismUnitBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (prismBehavior.Health <= 0)
        {
            SceneManager.LoadScene("DefeatScene");
        }
    }
}
