using UnityEngine;
using System.Collections;
using MyoUnity;
using System;
using System.Collections.Generic;

public class MyoManager : MonoBehaviour 
{
	/// <summary>
	/// The instance of this singleton
	/// </summary>
	public static MyoManager instance;

	/// <summary>
	/// This determines whether or not Myo Enabled setting is shown and enable-able
	/// </summary>
	public bool permitMyoEnabled = true;
	
	/// <summary>
	/// Subscribe to this event to recieve Pose event notifications
	/// </summary>
	public static event Action<int,MyoPose> PoseEvent;

	/// <summary>
	/// Subscribe to these events to learn when various connection events occur.
	/// </summary>
	public static event Action AttachEvent, DetachEvent, ConnectEvent, DisconnectEvent;

	private static List<Quaternion> myoRotations;

	private static bool isAttaching = false;
	
	/// <summary>
	/// Enabled or Disabled, changed with Initialize/Uninitialize
	/// </summary>
	private bool isEnabled = false;

	/// <summary>
	/// Property for enabled setting, will Initialize/Unititialize appropriately
	/// </summary>
	/// <value><c>true</c> if is enabled; otherwise, <c>false</c>.</value>
	public static bool IsEnabled
	{
		get {
			if (instance != null) 
				return instance.isEnabled; 
			return false;
		}
		set 
		{
			if (instance != null)
			{
				if (value)
					Initialize();
				else 
					Uninitialize(); 
			}
		}
	}

	public static int DeviceCount
	{
		get
		{
#if UNITY_IOS && !UNITY_EDITOR
			return MyoBinding._getDeviceCount();
#endif
			return 0;
		}
	}

	void Awake()
    {
    	//Persist through level changes, only one singleton instance
    	if (instance == null)
    	{
    		instance = this;
    		DontDestroyOnLoad(this);
    	}
    	else
    	{
    		DestroyImmediate(this.gameObject);
    		return;
    	}

    	//Enable compass to get Relative Quaternion
		Input.compass.enabled = true;
    }

    void OnApplicationQuit()
    {
        Uninitialize();
    }

	/// <summary>
	/// This method is called on this object by the XCode plugin when a pose event occurs. Don't have any methods "OnPose(string)" on this game object to avoid issues.
	/// </summary>
	/// <param name="poseName">Name of pose that occurred</param>
	public void OnPose( string indexAndPoseName )
	{
		if (isEnabled)
		{
			//split string back into index and pose
			string[] indexPose = indexAndPoseName.Split(',');
			int index = 0;
			int.TryParse( indexPose[0], out index );
			MyoPose pose = (MyoPose)Enum.Parse( typeof(MyoPose), indexPose[1] );
			if (PoseEvent != null)
				PoseEvent( index, pose );
		}
	}

	public void OnRotation( string indexAndQuaternion )
	{
		if (isEnabled)
		{
			//split string into index,x,y,z,w
			string[] tokens = indexAndQuaternion.Split(',');
			int index = 0;
			int.TryParse( tokens[0], out index );
			float x=0, y=0, z=0, w=0;
			float.TryParse( tokens[1], out x );
			float.TryParse( tokens[2], out y );
			float.TryParse( tokens[3], out z );
			float.TryParse( tokens[4], out w );

			if (index >= myoRotations.Count)
			{
				for ( int i=myoRotations.Count; i<index+1; ++i)
					myoRotations.Add( Quaternion.identity );
			}
			myoRotations[index] = new Quaternion( x, y, z, w );
		}
	}

	public void OnAttachDevice( string ignore )
	{
		if (AttachEvent != null)
			AttachEvent();

		isAttaching = false;
	}

	public void OnDetachDevice( string ignore )
	{
		if (DetachEvent != null)
			DetachEvent();
	}

	public void OnConnectDevice( string ignore )
	{
		if (ConnectEvent != null)
			ConnectEvent();

		isAttaching = false;
	}

	public void OnDisconnectDevice( string ignore )
	{
		if (DisconnectEvent != null)
			DisconnectEvent();
	}
	


	#region MyoPluginAPI
	
	/// <summary>
	/// Initializes and enables Myo plugin
	/// </summary>
	public static void Initialize()
	{
		Debug.Log("Initializing Myo");
#if UNITY_IPHONE && !UNITY_EDITOR
		MyoBinding._initialize();
#endif
		instance.isEnabled = true;

		myoRotations = new List<Quaternion>();
	}

	/// <summary>
	/// Uninitialize and disables Myo plugin
	/// </summary>
	public static void Uninitialize()
	{
		instance.isEnabled = false;
	}

	/// <summary>
	/// Automatically pairs with any Myo device available. Use this when you only expect there to be one Myo in range. 
	/// </summary>
	public static void AttachToAny()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		isAttaching = true;
		MyoBinding._attachToAny();
#endif
	}

	/// <summary>
	/// Start pairing procedure where the myo must tap the device in order to connect. 
	/// </summary>
	public static void AttachToAdjacent()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		isAttaching = true;
		MyoBinding._attachToAdjacent();
#endif
	}

	/// <summary>
	/// Presents Pairing UI that allows user to scan for Myo devices, and pair/unpair Myos at will. 
	/// </summary>
	/// <returns><c>true</c>, if pairing ui was presented, <c>false</c> if pairing ui failed to be presented, usually because Unity view isn't fully initialized yet.</returns>
	public static bool PresentPairing()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		isAttaching = true;
		return MyoBinding._presentPairing();
#endif
		return false;
	}

	/// <summary>
	/// Gets the rotation of the Myo device, converted into Unity's coordinate system (See MyoToUnity).
	/// </summary>
	/// <returns>The quaternion representing the Myo rotation in world space.</returns>
	public static Quaternion GetQuaternion( int index = 0 )
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		if (index < myoRotations.Count)
			return MyoToUnity( myoRotations[index] );
		return Quaternion.identity;
#endif
		//To test in editor, return based on Mouse position, this can be disabled and just return Quaternion.identity if desired
		return Quaternion.Euler( 90 - Mathf.Clamp01(Input.mousePosition.y / Screen.height) * 180, Mathf.Clamp01(Input.mousePosition.x / Screen.width) * 360 - 180, 0 );
	}

	/// <summary>
	/// Converts a Quaternion from Myo coordinate system to Unity coordinate system. 
	/// Myo X = Unity -Z
	/// Myo Y = Unity X
	/// Myo Z = Unity Y
	/// </summary>
	/// <returns>The Quaternion rotation in Unity's coordinate system</returns>
	/// <param name="q">The Quaternion in Myo coordinate system</param>
	public static Quaternion MyoToUnity( Quaternion q )
	{
		return new Quaternion( q.y, q.z, -q.x, -q.w );
	}

	/// <summary>
	/// Gets the rotation of the Myo device, converted to Unity, and offset by Compass's heading. Due to Compass inaccuracy on iOS, this causes jittery behavior and may be undesirable.
	/// </summary>
	/// <returns>The rotation of the Myo device relative to the iOS device</returns>
	public static Quaternion GetRelativeQuaternion()
	{
		Quaternion myo = GetQuaternion();
		
		return Quaternion.AngleAxis( -Input.compass.magneticHeading, Vector3.up ) * myo;
	}

	/// <summary>
	/// Vibrates the Myo device for the specified length, Short, Medium, or Long.
	/// </summary>
	/// <returns><c>true</c>, if vibrated, <c>false</c> otherwise.</returns>
	/// <param name="length">Length.</param>
	public static bool VibrateForLength( MyoVibrateLength length, int index=0 )
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		return MyoBinding._vibrateForLength( index, (int)length );
#else
		return false;
#endif
	}

	#endregion
}
