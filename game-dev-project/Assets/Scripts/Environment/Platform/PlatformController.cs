using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Platform[] platforms;
    
    private void Start() 
    {
        platforms = GetComponentsInChildren<Platform>(true);
    }

    public void MovePlatforms(Lever.State state, Platform.PlatformColor color)
    {
        IEnumerable<Platform> sameColorPlatform = platforms.Where(x => x.Color == color);

        bool inShiftState = state == Lever.State.On;
        
        foreach (var platform in sameColorPlatform)
        {
            if (inShiftState)
            {
                platform.StartShifting();
            }
            else 
            {
                platform.StopShifting();
            }
        }
    }
}
