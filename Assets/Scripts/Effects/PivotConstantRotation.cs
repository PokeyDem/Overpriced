using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotConstantRotation : MonoBehaviour
{
   [SerializeField] private float _rotationSpeed;
   private float step;

   private void Update()
   {
      transform.RotateAround(transform.parent.position, Vector3.up, _rotationSpeed * Time.deltaTime);
   }
}
