using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorController : SingletonBehaviour<PlayerColorController>
{
    [SerializeField] private Material _materialToReplace;
    [SerializeField] private Material[] _colors;
    [SerializeField] private List<Material> _remainingColors;
    
    // Start is called before the first frame update

    public override void Awake()
    {
        _remainingColors = new List<Material>(_colors);
    }

    // Update is called once per frame
    public void AssignRandomColor(DogController dog)
    {

        if (dog == null)
        {
            Debug.Log("Dog is null");
            return;
        }
        
        Material material;
        
        Debug.Log("AssignRandomColor " + dog, dog);
        if (_remainingColors.Count > 0)
        {
            material = _remainingColors[0];
            _remainingColors.RemoveAt(0);

            dog.color = material;
            MeshRenderer[] componentsInChildren = dog.GetComponentsInChildren<MeshRenderer>(true);

            foreach (MeshRenderer meshRenderer in componentsInChildren)
            {
                if (meshRenderer.sharedMaterial == _materialToReplace)
                {
                    meshRenderer.material = material;
                }
            }
        }
    }
}
