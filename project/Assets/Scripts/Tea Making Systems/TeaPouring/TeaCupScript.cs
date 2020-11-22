using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaCupScript : MonoBehaviour
{
    public Transform teaFullPos;

    public GameObject teaObj;


    public void RaiseTeaLevel()
	{
		StartCoroutine(RaiseTea());
	}

    private IEnumerator RaiseTea()
	{
        //show tea
        teaObj.SetActive(true);

        float time = 0;
        float scale = 1f / 3f;

        Vector3 startPos = teaObj.transform.position;
        Vector3 startScale = teaObj.transform.localScale;
        //teaObjFull for end

        while (time < 3)
        {
            //lerp pos and scale
            teaObj.transform.position = Vector3.Lerp(startPos, teaFullPos.position, time * scale);
            teaObj.transform.localScale = Vector3.Lerp(startScale, teaFullPos.localScale, time * scale);


            time += Time.deltaTime;
            yield return null;
        }
    }
}