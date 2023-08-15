using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeChildren : MonoBehaviour {

    public List<Transform> ignore;

	// Use this for initialization
	void Awake ()
    {
     List<int> ignoreIndexes = new List<int>();


        foreach (var item in ignore)
        {
            ignoreIndexes.Add(item.GetSiblingIndex());
        }
        List<Transform> children = new List<Transform>(transform.GetChildren());
        children.Shuffle();


        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);

        }


        for (int i = 0; i < ignore.Count; i++)
        {
            ignore[i].SetSiblingIndex(ignoreIndexes[i]);
        }

    }

// Update is called once per frame
void Update () {
		
	}
}

