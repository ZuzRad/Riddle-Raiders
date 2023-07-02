using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateController : MonoBehaviour
{
   [SerializeField] private Image[] hearts;

   public void DecreaseHeartsAmount()
   {
      Image lastEnabledImage = hearts.FirstOrDefault(image => image.enabled);

      if (lastEnabledImage) {
         lastEnabledImage.enabled = false;
      }
   }
}
