using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
   [SerializeField] private Material material;
   private LaserBeam beam;

   private void Update()
   {
      Destroy(GameObject.Find("Laser Beam"));
      beam = new LaserBeam(transform.position, transform.up, material);
   }
}
