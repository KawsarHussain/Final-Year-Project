using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Sprite Config")]
    [SerializeField] private Sprite throwSprite;
    [SerializeField] private Sprite teleportSprite;
    [SerializeField] private Sprite swapSprite;
    [SerializeField] private Sprite returnSprite;

    [Header("Icons")]
    [SerializeField] private Image LMBIcon;
    [SerializeField] private TextMeshPro LMBText;
    [SerializeField] private Image RMBIcon;
    [SerializeField] private TextMeshPro RMBText;
    [SerializeField] private Image SwapIcon;
    [SerializeField] private Image ReloadIcon;

    [Header("Player")]
    [SerializeField] private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Robot Kyle").GetComponent<PlayerController>();
        SwapIcon.enabled = false;
        ReloadIcon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.bands[0].GetThrown()) LMBIcon.sprite = teleportSprite;
        else LMBIcon.sprite = throwSprite;
        if (player.bands[1].GetThrown()) RMBIcon.sprite = teleportSprite;
        else RMBIcon.sprite = throwSprite;
        if (player.player.GetAmountOfBands() > 0) ReloadIcon.enabled = true;
        else ReloadIcon.enabled = false;

        if (player.swappable) SwapIcon.enabled = true;
        else SwapIcon.enabled = false;

    }
}
