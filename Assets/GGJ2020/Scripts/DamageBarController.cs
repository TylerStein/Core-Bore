using UnityEngine;
using UnityEngine.UI;

public class DamageBarController : MonoBehaviour
{
    public Image damageBarImage;

    public void SetHealth(float health) {
        damageBarImage.fillAmount = health;
    }

}
