using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

namespace MyoUnity
{
	public enum MyoPose
    {
		UNKNOWN = -1,
        REST = 0,
		FIST = 1,
		WAVE_IN = 2,
		WAVE_OUT = 3,
		FINGERS_SPREAD = 4,
		THUMB_TO_PINKY = 5
    };
	
	public enum MyoVibrateLength
	{
		SHORT,
		MEDIUM,
		LONG
	};
	
	
    public class MyoBinding
    {
        [DllImport("__Internal")]
        public static extern void _initialize();
		
		[DllImport("__Internal")]
        public static extern void _attachToAny();

		[DllImport("__Internal")]
		public static extern void _attachToAdjacent();

        [DllImport("__Internal")]
        public static extern bool _presentPairing();
		
        [DllImport("__Internal")]
        public static extern int _getDeviceCount();

		[DllImport("__Internal")]
        public static extern bool _vibrateForLength( int index, int option );
        
    }
}