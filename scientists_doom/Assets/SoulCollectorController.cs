using UnityEngine;

public class SoulCollectorController : MonoBehaviour
{
    public ParticleSystem soulFireOverload;
    private Animator animator;
    private Inventory playerInventory;

    void Start(){
        animator = gameObject.GetComponent<Animator>();
        //playerInventory = CharacterManager.activeCharacterObject.GetComponent<Inventory>();
    }

    public void BurstSouls(){
        soulFireOverload.Play();
    }

    public void AcquireSoul(){
        if(animator){
            animator.SetTrigger("pulseTrigger");
        }else{
            Debug.LogWarning("SoulCollector could not get animator component!");
        }

        CharacterManager.activeCharacterObject.GetComponent<Inventory>().AddSouls(1);
    }
}
