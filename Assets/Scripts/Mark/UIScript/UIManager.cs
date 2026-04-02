using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;

    public GameObject hitUI;
    public GameObject DeathUI;
    public GameObject PrUI;

    public TextMeshProUGUI AmmoT;
    public UnityEngine.UI.Image hpbar;
    public Gradient hpGradient;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        Instance = this;
    }
    public void HitUI()
    {
        Instantiate(hitUI, transform);
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnableDeathUI()
    {
        DeathUI.SetActive(true);
    }
    public void DisablePrUI()
    {
        PrUI.SetActive(false);
    }

    public void SetHpValue(float hp)
    {
        float floatHP = hp / 50;
        hpbar.color = hpGradient.Evaluate(floatHP);
        hpbar.fillAmount = floatHP;
    }

}
