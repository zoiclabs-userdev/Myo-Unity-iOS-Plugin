//
//  TLMArmRecognizedEvent.h
//  MyoKit
//
//  Copyright (C) 2014 Thalmic Labs Inc.
//  Distributed under the Myo SDK license agreement. See LICENSE.txt.
//

#import <Foundation/Foundation.h>

@class TLMMyo;

@interface TLMArmRecognizedEvent : NSObject <NSCopying>

typedef NS_ENUM (NSInteger, TLMArm) {
    TLMArmRight, /**< Myo is on the right arm.*/
    TLMArmLeft  /**< Myo is on the left arm.*/
};

typedef NS_ENUM (NSInteger, TLMArmXDirection) {
    TLMArmXDirectionTowardWrist, /**< Myo's +x axis is pointing toward the user's wrist.*/
    TLMArmXDirectionTowardElbow  /**< Myo's +x axis is pointing toward the user's elbow.*/
};

/**
 The TLMMyo that recognized it is on an arm.
 */
@property (nonatomic, weak, readonly) TLMMyo *myo;

/**
 The arm that the Myo armband is on.
 */
@property (nonatomic, readonly) TLMArm arm;

/**
 The +x axis direction of the Myo armband relative to a user's arm.
 */
@property (nonatomic, readonly) TLMArmXDirection xDirection;

/**
 The timestamp associated with the arm recognized event.
 */
@property (nonatomic, strong, readonly) NSDate *timestamp;

@end
