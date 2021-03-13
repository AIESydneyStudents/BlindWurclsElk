﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingMenuScript : MonoBehaviour
{
    public RectTransform[] images;
    public Image blackFader;

    public float speed;

    int index = 0;

    public Vector3 firstStart;


    void Start()
    {
        StartCoroutine(Scroll(true));
    }

    private IEnumerator Scroll(bool first = false)
    {
        //show image and move it to start of canvas
        images[index].gameObject.SetActive(true);
        images[index].localPosition = new Vector3(images[index].sizeDelta.x * 0.5f, images[index].localPosition.y, 0);

        Vector3 start, end;

        if (!first)
		{
            start = images[index].localPosition;
            end = start + new Vector3(-images[index].sizeDelta.x, 0, 0);
        }
        else
		{
            start = firstStart;
            end = -firstStart;
		}
        

        for (float t = 0f; t < 1f; t += Time.deltaTime * speed)
        {
            //fade in
            if (t < speed * 2)
            {
                blackFader.color -= new Color(0, 0, 0, Time.deltaTime * 0.5f);
            }


            //scroll background
            images[index].localPosition = Vector3.Lerp(start, end, t);


            //fade out
            if (t > 1 - speed * 2)
            {
                blackFader.color += new Color(0, 0, 0, Time.deltaTime * 0.5f);
            }

            yield return null;
        }


        //hide image and increment index
        images[index].gameObject.SetActive(false);
        index++;
        if (index >= images.Length)
        { index = 0; }

        StartCoroutine(Scroll());
    }
}