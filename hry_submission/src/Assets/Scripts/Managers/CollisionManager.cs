using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour {

  [SerializeField] private LayerMask collisionMask;

  public LayerMask GetMask () {
    return collisionMask;
  }

}