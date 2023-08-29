using UnityEngine;

public class StartingLightPole : MonoBehaviour
{
    [SerializeField]  private StartingLight _red;
    [SerializeField]  private StartingLight _amber;
    [SerializeField]  private StartingLight _green;
    [SerializeField]  private GameObject _startEffects;

    public StartingLight red => _red;

    public StartingLight amber => _amber;

    public StartingLight green => _green;

    public GameObject startEffects => _startEffects;
}