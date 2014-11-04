# Myo-Unity-iOS-km
================

This is the readme for the Myo iOS Plugin for Unity3D. 

## Installation

Simply add the files in the Assets folder to your Unity project Assets folder. 

```
\Assets\MyoPlugin
    +---Demo
        +---Scenes
        |       MyoPluginDemo.unity
        +---Scripts
        |       MyoPluginDemo.cs
    +---Prefabs
    |       MyoManager.prefab
    +---Scripts
    |       MyoBinding.cs
    |       MyoManager.cs
\Assets\Plugins
    +---iOS
    |       MyoUnity.h
    |       MyoUnity.mm
    |       MyoKit.framework
```

## Building the Demo

1. Switch your Build Settings to iOS.
2. Make sure your Target iOS Version is 7.0 or greater (Limitation of Myo iOS SDK).
3. Open the scene "MyoPlugin/Demo/Scenes/MyoPluginDemo.unity" and add the scene to the Scenes in Build Settings.
4. Build the Unity project.
5. In XCode, "MyoKit.framework" sometimes doesn't automatically transfer. Copy paste it into the Libraries folder and add it to the project.
6. In Build Phases, add "MyoKit.framework", "CoreBluetooth.framework", and make sure "SystemConfiguration.framework" is already added.
7. Add "-ObjC" to Other Linker Flags in the Build Settings tab.
8. Build and deploy to your iOS device!

## Using the Demo

1. First you must pair your Myo device. Press the "Present Pairing" button to do so. Note: You can also use the API's "AttachToAny" or "AttachToAdjacent" method if you wish.
    Once paired, you should notice the Cube should rotate based on the Myo orientation.
2. You should immediately gain control of the Cube's rotation. Poses and Orientation will be displayed as well.

## The API

Here's a trimmed down version of the MyoManager.cs class, which should be your only interface into the Myo iOS Plugin. This script needs to be added onto a GameObject in the scene. The GameObject needs to be named "MyoManager". To do this easily, just drag in the provided prefab "MyoPlugin/Prefabs/MyoManager" into your scene and you should be good to go.

```C#

/// Manager class for Myo iOS Plugin. Use only this public API to interface with Myo inside of Unity. 
public class MyoManager : MonoBehaviour 
{
    /// Subscribe to this event to recieve Pose event notifications
    public static event Action<MyoPose> PoseEvent;

    /// Property for enabled setting, will Initialize/Unititialize appropriately
    public static bool IsEnabled;

    /// Initializes and enables Myo plugin
    public static void Initialize();

    /// Uninitialize and disables Myo plugin
    public static void Uninitialize();

    /// Automatically pairs with any Myo device available. Use this when you only expect there to be one Myo in range. 
    public static void AttachToAny();

    /// Automatically pairs with the first Myo device that touches the iOS device. 
    public static void AttachToAdjacent();

    /// Presents Pairing UI that allows user to scan for Myo devices, and pair/unpair Myos at will. 
    public static bool PresentPairing();

    /// Gets the rotation of the Myo device, converted into Unity's coordinate system (See MyoToUnity).
    public static Quaternion GetQuaternion()

    /// Converts a Quaternion from Myo coordinate system to Unity coordinate system. 
    /// Myo X = Unity -Z
    /// Myo Y = Unity X
    /// Myo Z = Unity Y
    private static Quaternion MyoToUnity( Quaternion q );

    /// Gets the rotation of the Myo device, converted to Unity, and offset by Compass's heading. Due to Compass inaccuracy on iOS, this causes jittery behavior and may be undesirable.
    public static Quaternion GetRelativeQuaternion();

    //Vibrates the Myo device for the specified length, Short, Medium, or Long.
    public static bool VibrateForLength( MyoVibrateLength length );
}

```

## Support

If you have feedback for this plugin contact Katlan Merrill (kmerrill@zoicstudios.com) or discuss the plugin in the [forum feature thread](https://developer.thalmic.com/forums/topic/282/?page=1#post-1659).

This is intended to be a community project, so support directly from the developer may be minimal. 






