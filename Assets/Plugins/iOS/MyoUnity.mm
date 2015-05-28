//
//  MyoUnity.m
//  Unity-iPhone
//
//  Created by Katlan
//
//

#import "MyoUnity.h"

UIViewController* UnityGetGLViewController();
UIWindow* UnityGetMainWindow();

static MyoUnity* _MyoUnity;

@implementation MyoUnity

+ (MyoUnity*)Instance
{
	if(!_MyoUnity)
    {
		_MyoUnity = [[MyoUnity alloc] init];
    }
	return _MyoUnity;
}


+ (void)load
{
    [[NSNotificationCenter defaultCenter] addObserver:[MyoUnity Instance]
                                             selector:@selector(didReceivePoseChange:)
                                                 name:TLMMyoDidReceivePoseChangedNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:[MyoUnity Instance]
                                             selector:@selector(didReceiveOrientationEvent:)
                                                 name:TLMMyoDidReceiveOrientationEventNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:[MyoUnity Instance]
                                             selector:@selector(didAttachDevice:)
                                                 name:TLMHubDidAttachDeviceNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:[MyoUnity Instance]
                                             selector:@selector(didDetachDevice:)
                                                 name:TLMHubDidDetachDeviceNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:[MyoUnity Instance]
                                             selector:@selector(didConnectDevice:)
                                                 name:TLMHubDidConnectDeviceNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:[MyoUnity Instance]
                                             selector:@selector(didDisconnectDevice:)
                                                 name:TLMHubDidDisconnectDeviceNotification
                                               object:nil];
}

- (void)didAttachDevice:(NSNotification*)notification {
    
    UnitySendMessage("MyoManager", "OnAttachDevice", "" );
}

- (void)didDetachDevice:(NSNotification*)notification {
    
    UnitySendMessage("MyoManager", "OnDetachDevice", "" );
}

- (void)didConnectDevice:(NSNotification*)notification {
    
    UnitySendMessage("MyoManager", "OnConnectDevice", "" );
}

- (void)didDisconnectDevice:(NSNotification*)notification {
    
    UnitySendMessage("MyoManager", "OnDisconnectDevice", "" );
}

- (void)didReceivePoseChange:(NSNotification*)notification {
    TLMPose *pose = notification.userInfo[kTLMKeyPose];
    
    if (pose != nil)
    {
        //make pose to string converter
        NSString* const poseToString[] = {
            [TLMPoseTypeUnknown] = @"UNKNOWN",
            [TLMPoseTypeRest] = @"REST", /**< Default pose type when no pose is being made.*/
            [TLMPoseTypeFist] = @"FIST", /**< Clenching fingers together to make a fist.*/
            [TLMPoseTypeWaveIn] = @"WAVE_IN", /**< Turning your palm towards yourself.*/
            [TLMPoseTypeWaveOut] = @"WAVE_OUT", /**< Turning your palm away from yourself.*/
            [TLMPoseTypeFingersSpread] = @"FINGERS_SPREAD", /**< Spreading your fingers and extending your palm.*/
            [TLMPoseTypeDoubleTap] = @"DOUBLE_TAP" /**< Twist your wrist in towards yourself.*/
        };
        
        //send message to unity with pose as string
        NSString* poseStr = poseToString[pose.type];
        if (poseStr != nil)
        {
            int index = [self getMyoIndex:pose.myo];
            if (index >= 0)
            {
                NSString* args = [NSString stringWithFormat:@"%d,%@", index, poseStr];
                
                //GameObject in Unity scene must be named "MyoManager" with function "OnPose" to recieve pose events.
                UnitySendMessage("MyoManager", "OnPose", [args UTF8String] );
            }
        }
    }
}

- (void)didReceiveOrientationEvent:(NSNotification*)notification {
    TLMOrientationEvent* event = notification.userInfo[kTLMKeyOrientationEvent];
    
    if (event != nil)
    {
        int index = [self getMyoIndex:event.myo];
        
        if (index >= 0)
        {
            NSString* args = [NSString stringWithFormat:@"%d,%f,%f,%f,%f", index, event.quaternion.x, event.quaternion.y, event.quaternion.z, event.quaternion.w ];
            
            UnitySendMessage("MyoManager", "OnRotation", [args UTF8String] );
        }
    }
}

//- (void) attachToAny
//{
//    [[TLMHub sharedHub] attachToAny];
//}

- (bool) presentPairing
{
    UINavigationController *controller = [TLMSettingsViewController settingsInNavigationController];
    
    UIViewController* viewController = UnityGetGLViewController();
    if (viewController.isViewLoaded && viewController.view.window)
    {
        [ viewController presentViewController:controller animated:YES completion:nil];
        return TRUE;
    }
    return FALSE;
}

- (void) attachToAdjacent
{
    [[TLMHub sharedHub] attachToAdjacent];
}

- (int) getDeviceCount
{
    return [[[TLMHub sharedHub] myoDevices] count];
}

- (bool) vibrateForLength:(int)index option:(int)option
{
    if (index >= [self getDeviceCount])
        return NO;
    
    [[[[TLMHub sharedHub] myoDevices] objectAtIndex:index] vibrateWithLength:(TLMVibrationLength)option];
    
    return TRUE;
}

- (int) getMyoIndex:(TLMMyo*)myo
{
    for (int i=0; i<[[TLMHub sharedHub] myoDevices].count; i++)
    {
        if (myo == [[[TLMHub sharedHub] myoDevices] objectAtIndex:i])
            return i;
    }
    return -1;
}

extern "C" {

    void _initialize()
    {
        [TLMHub sharedHub];
        
        [MyoUnity Instance];
    }

    void _uninitialize()
    {
        //todo
    }

//    void _attachToAny()
//    {
//        [[MyoUnity Instance] attachToAny];
//    }

    bool _presentPairing()
    {
        return [[MyoUnity Instance] presentPairing];
    }

    void _attachToAdjacent()
    {
        [[MyoUnity Instance] attachToAdjacent];
    }

    int _getDeviceCount()
    {
        return [[MyoUnity Instance] getDeviceCount];
    }

    bool _vibrateForLength( int index, int option )
    {
        return [[MyoUnity Instance] vibrateForLength:index option:option];
    }
}


@end
