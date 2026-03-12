using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InstructionTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent instructionPopup;
    [SerializeField] GameObject instructionObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AskForInstruction()
    {
        instructionPopup?.Invoke();
    }


}
