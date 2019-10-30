using System.Collections.Generic;
using UnityEngine;

public enum VisionType
{
    White=0,Red=1,Green=2,Blue=4
};

public static class Constants
{
    public static Color Vision_Color(VisionType vType)
    {
        switch(vType)
        {
            case VisionType.White:  return Color.white;
            case VisionType.Red:    return Color.red;
            case VisionType.Green:  return Color.green;
            case VisionType.Blue:   return Color.blue;
        }
        return Color.black;
    }
}