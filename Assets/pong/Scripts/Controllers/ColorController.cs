using UnityEngine;
using System.Collections;
using System;
public class ColorController : MonoBehaviour {

    public tk2dUIScrollbar redBar;
    public tk2dUIScrollbar greenBar;
    public tk2dUIScrollbar blueBar;
    public tk2dSlicedSprite sprite;

    void Awake()
    {
        string color=PlayerPrefs.GetString("RightPlayerColor");
        if (color=="")
        {
            redBar.Value = 1;
            greenBar.Value = 1;
            blueBar.Value = 1;
        }
        else
        {
            if (name == "RightPlayerColor")
            {
                string[] rgb=color.Split('@');
                redBar.Value = Convert.ToSingle(rgb[0]);
                greenBar.Value = Convert.ToSingle(rgb[1]);
                blueBar.Value = Convert.ToSingle(rgb[2]);
            }
            else
            {
                color = PlayerPrefs.GetString("LeftPlayerColor");
                string[] rgb = color.Split('@');
                redBar.Value = Convert.ToSingle(rgb[0]);
                greenBar.Value = Convert.ToSingle(rgb[1]);
                blueBar.Value = Convert.ToSingle(rgb[2]);
            }
        }
    }
    void OnScroll()
    {
        sprite.color =new Color(redBar.Value, greenBar.Value, blueBar.Value);
    }
}
