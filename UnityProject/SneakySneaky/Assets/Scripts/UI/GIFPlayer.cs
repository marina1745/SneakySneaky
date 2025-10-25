using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GIFPlayer : MonoBehaviour
{
    public List<Sprite> sprites;
    public float changeAfter = 0.8f;
    private Image image;
    private int currentImage = 0;
    private void OnEnable()
    {
        image = GetComponent<Image>();
        InvokeRepeating("ChangeSprite", changeAfter, changeAfter);
    }
    private void ChangeSprite()
    {
        image.sprite = sprites[(++currentImage) % sprites.Count];
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
