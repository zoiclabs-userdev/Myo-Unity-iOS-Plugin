using UnityEngine;
using System.Collections;
using MyoUnity;

public class MyoPluginDemo : MonoBehaviour 
{
	public Transform objectToRotate;

	private Quaternion myoRotation;
	private MyoPose myoPose = MyoPose.UNKNOWN;


	// Use this for initialization
	void Start () 
	{
		MyoManager.Initialize();

		MyoManager.PoseEvent += OnPoseEvent;
	}

	void OnPoseEvent( int index, MyoPose pose )
	{
		myoPose = pose;
	}

	void Update()
	{
		if (MyoManager.IsEnabled)
		{
			myoRotation = MyoManager.GetQuaternion();

			objectToRotate.rotation = myoRotation;
		}
	}
	
	void OnGUI()
	{
		GUI.BeginGroup( new Rect( 10, 10, 300, 500 ) );

		if (GUILayout.Button ( "Present Pairing" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.PresentPairing();
		}

		if (GUILayout.Button ( "Attach to Adjacent" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.AttachToAdjacent();
		}

		if (GUILayout.Button ( "Vibrate Short" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.VibrateForLength( MyoVibrateLength.SHORT );
		}

		if (GUILayout.Button ( "Vibrate Medium" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.VibrateForLength( MyoVibrateLength.MEDIUM );
		}

		if (GUILayout.Button ( "Vibrate Long" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.VibrateForLength( MyoVibrateLength.LONG );
		}

		GUILayout.Label ( "Myo Quaternion: " + myoRotation.ToString(), GUILayout.MinWidth(300), GUILayout.MinHeight(30) );

		GUILayout.Label ( "Myo Pose: " + myoPose.ToString(), GUILayout.MinWidth(300), GUILayout.MinHeight(30) );

		GUI.EndGroup();
	}
}
