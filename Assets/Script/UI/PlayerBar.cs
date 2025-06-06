using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Image healthImage;
    [SerializeField] Image blockImage;

    float lerpSpeed;
    void Start()
    {
        healthImage.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        lerpSpeed = 5f * Time.deltaTime;

        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, (float)player.getCurrentHealth() / (float)player.getMaxHealth(), lerpSpeed);
        blockImage.fillAmount = Mathf.Lerp(blockImage.fillAmount, player.getCurrentBlock() / player.getMaxBlock(), lerpSpeed);

    }
}
