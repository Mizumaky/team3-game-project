using UnityEngine;

[RequireComponent (typeof (Animator))]
public class ColliderBridge : MonoBehaviour {
    MeleeWeapon _listener;
    public void Initialize (MeleeWeapon l) {
        _listener = l;
    }
    void OnTriggerEnter (Collider other) {
        _listener.OnWeaponTriggerEnter (other);
    }
}