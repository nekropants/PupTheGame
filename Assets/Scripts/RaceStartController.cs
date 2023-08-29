using System.Collections;
using UnityEngine;

public class RaceStartController : MonoBehaviour
{
    [SerializeField]  private StartingLightPole [] _poles;
    [SerializeField]  private SpringController [] _gates;
    [SerializeField]  private SpringConfiguration _openGate;
    private void Start()
    {
        foreach (var startingLightPole in _poles)
        {
            if (startingLightPole)
            {
                startingLightPole.red.SetOff();
                startingLightPole.amber.SetOff();
                startingLightPole.green.SetOff();
            }
        }

        StartCountDown();
    }
    
    public  void StartCountDown()
    {
        StartCoroutine(IERunCountDown());
    }

    private IEnumerator IERunCountDown()
    {        
        float interval = 2f;

        while (GameController.instance.dogs.Count < 1)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(5);


        foreach (var startingLightPole in _poles)
        {
            if (startingLightPole)
            {
                startingLightPole.red.SetOn();
            }
        }
        
        yield return new WaitForSeconds(interval);
        
        foreach (var startingLightPole in _poles)
        {
            if (startingLightPole)
            {
                startingLightPole.red.SetOff();
                startingLightPole.amber.SetOn();
            }
        }

        yield return new WaitForSeconds(interval);
        
        foreach (var startingLightPole in _poles)
        {
            if (startingLightPole)
            {
                startingLightPole.amber.SetOff();
                startingLightPole.green.SetOn();
                startingLightPole.startEffects.gameObject.SetActive(true);
            }
        }

        foreach (var gate in _gates)    
        {
            if (gate)
            {
                gate.SetConfig(_openGate);
            }
        }
    }
}