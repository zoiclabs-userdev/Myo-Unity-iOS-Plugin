//
//  MyoUnity.h
//  Unity-iPhone
//
//  Created by Katlan
//
//


#import <MyoKit/MyoKit.h>

@interface MyoUnity : NSObject

+ (MyoUnity*)Instance;

+ (void) load;

- (void) didReceivePoseChange:(NSNotification*)notification;

- (void) didReceiveOrientationEvent:(NSNotification*)notification;

//- (void) attachToAny;

- (bool) presentPairing;

- (void) attachToAdjacent;

- (int) getDeviceCount;

- (bool) vibrateForLength:(int)index option:(int)option;

- (int) getMyoIndex:(TLMMyo*)myo;

extern "C"
{
    void _initialize();

    void _uninitialize();

//    void _attachToAny();

    bool _presentPairing();

    void _attachToAdjacent();

    int _getDeviceCount();

    bool _vibrateForLength( int index, int option );
}

@end
