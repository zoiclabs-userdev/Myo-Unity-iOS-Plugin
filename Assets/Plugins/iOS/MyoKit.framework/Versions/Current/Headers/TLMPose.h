//
//  TLMPose.h
//  MyoKit
//
//  Copyright (C) 2014 Thalmic Labs Inc.
//  Distributed under the Myo SDK license agreement. See LICENSE.txt.
//

#import <Foundation/Foundation.h>

@class TLMMyo;

//---------
// TLMPose
//---------

/** Represents a hand pose detected by a TLMMyo. */
@interface TLMPose : NSObject <NSCopying>

/**
   Represents different hand poses.
 */
typedef NS_ENUM (NSInteger, TLMPoseType) {
    TLMPoseTypeRest          = 0, /**< Rest pose.*/
    TLMPoseTypeFist          = 1, /**< User is making a fist.*/
    TLMPoseTypeWaveIn        = 2, /**< User has an open palm rotated towards the posterior of their wrist.*/
    TLMPoseTypeWaveOut       = 3, /**< User has an open palm rotated towards the anterior of their wrist.*/
    TLMPoseTypeFingersSpread = 4, /**< User has an open palm with their fingers spread away from each other.*/
    TLMPoseTypeThumbToPinky  = 6, /**< User is touching the tip of their thumb to the tip of their pinky.*/
    TLMPoseTypeUnknown       = 0xffff /**< Unknown pose.*/
};

/** The TLMMyo posting the pose. */
@property (nonatomic, weak, readonly) TLMMyo *myo;

/** The pose being recognized. */
@property (nonatomic, readonly) TLMPoseType type;

/** The time the pose was recognized. */
@property (nonatomic, strong, readonly) NSDate *timestamp;

@end
