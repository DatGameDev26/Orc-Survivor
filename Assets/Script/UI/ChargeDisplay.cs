using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ChargeDisplay : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject chargeBarGO;
    [SerializeField] Image chargeImage;

    Animator animator;
    Color orangeColor;
    void Start()
    {
        animator = GetComponent<Animator>();
        orangeColor = new Color(1f, 0.647f, 0f);
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = (Vector2)player.transform.position;
            if (player.getCurrentCharge() > 0)
            {
                chargeBarGO.SetActive(true);
                float chargePercent = (float)player.getCurrentCharge() / player.getChargeRequire();
                chargeImage.fillAmount = chargePercent;
                chargeImage.color = chargePercent == 1 ? orangeColor : Color.white;
                animator.SetBool("charge_finish", chargePercent == 1);
            }
            else
            {
                chargeBarGO.SetActive(false);
            }
        }
        else chargeBarGO.SetActive(false);
    }

}
