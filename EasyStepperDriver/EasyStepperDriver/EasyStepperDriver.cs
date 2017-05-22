using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace testMicroToolskit
{
    namespace Hardware
    {
        namespace MotorDrivers
        {
            /// <summary>
            /// A class to manage the EasyDriver v4.4. An Open Source Hardware Stepper Motor Drive Project.
            /// </summary>
            /// <remarks>
            /// Thanks to Gus. GHI Electronics. You may have some additional information about this class on http://webge.github.io/EasyStepperDriver/
            /// </remarks>
            public class EasyStepperDriver
            {
                private OutputPort _SleepPin;
                private OutputPort _EnablePin;
                private OutputPort _DirectionPin;
                private OutputPort _StepPin;
                private OutputPort _StepModePinOne;
                private OutputPort _StepModePinTwo;
                private int _StepDelay = 2;
                private UInt32 _Steps = 0;
                private Mode _StepMode = Mode.Full;
                private Direction _StepDirection = Direction.Forward;

                /// <summary>
                /// Directions are Forward or Backward
                /// </summary>
                public enum Direction
                {
                    Forward,
                    Backward
                }
                /// <summary>
                /// Modes are Full, Half, Quarter, OneEighth
                /// </summary> 
                public enum Mode
                {
                    Full,
                    Half,
                    Quarter,
                    OneEighth
                }
                /// <summary>
                /// Get Steps
                /// </summary>
                public UInt32 Steps
                {
                    get
                    {
                        return _Steps;
                    }
                }

                /// <summary>
                /// Get or Set direction
                /// </summary>      
                public Direction StepDirection
                {
                    get
                    {
                        return _StepDirection;
                    }
                    set
                    {
                        _StepDirection = value;
                    }
                }

                /// <summary>
                /// Get or Set Step Mode
                /// </summary>
                public Mode StepMode
                {
                    get
                    {
                        return _StepMode;
                    }
                    set
                    {
                        _StepMode = value;
                    }
                }

                /// <summary>
                /// Get time between two step
                /// </summary>
                public int StepDelay
                {
                    get
                    {
                        return _StepDelay;
                    }

                }

                /// <summary>
                /// Get if Sleep or not
                /// </summary>
                public bool IsDriverSleep
                {
                    get
                    {
                        return !_SleepPin.Read();
                    }
                }

                /// <summary>
                /// Get if Enable or Disable
                /// </summary>
                public bool IsOutputsEnable
                {
                    get
                    {
                        return !_EnablePin.Read();
                    }
                }

                /// <summary>
                /// Creates an instance of the driver, that only lets you move and choose direction.
                /// </summary>
                /// <param name="DirectionPin">(DIR) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver up that way) digital signal.
                /// The level if this signal (high/low) is sampled on each rising edge of STEP to determine which direction to take the step (or microstep).</param>
                /// <param name="StepPin">(STEP) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver that way) digital signal. 
                /// Each rising edge of this signal will cause one step (or microstep) to be taken.</param>
                public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin)
                {
                    _DirectionPin = new OutputPort(DirectionPin, true);  // Forward
                    _StepPin = new OutputPort(StepPin, false);
                }

                /// <summary>
                /// Creates an instance of the driver, that only lets you move, choose direction and put the controller to sleep
                /// </summary>
                /// <param name="DirectionPin">(DIR) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver up that way) digital signal.
                /// The level if this signal (high/low) is sampled on each rising edge of STEP to determine which direction to take the step (or microstep).</param>
                /// <param name="StepPin">(STEP) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver that way) digital signal. 
                /// Each rising edge of this signal will cause one step (or microstep) to be taken.</param>
                /// <param name="SleepPin">(SLP) This normally high input signal will minimize power consumption by disabling 
                /// internal circuitry and the output drivers when pulled low.(disabled by default)</param>
                public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin SleepPin)
                {
                    _DirectionPin = new OutputPort(DirectionPin, false); // Forward
                    _StepPin = new OutputPort(StepPin, false);
                    _SleepPin = new OutputPort(SleepPin, false); // Sleep activé
                }

                /// <summary>
                /// Creates an instance of the driver, that only lets you move, choose direction and select step mode.
                /// </summary>
                /// <param name="DirectionPin">(DIR) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver up that way) digital signal.
                /// The level if this signal (high/low) is sampled on each rising edge of STEP to determine which direction to take the step (or microstep).</param>
                /// <param name="StepPin">(STEP) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver that way) digital signal. 
                /// Each rising edge of this signal will cause one step (or microstep) to be taken.</param>
                /// /// <param name="StepModePinOne">(MS1) These digital inputs control the microstepping mode. Possible settings are (MS1/MS2) : full step (0,0), half step (1,0), 1/4 step (0,1), and 1/8 step (1,1 : default)</param>
                /// <param name="StepModePinTwo">(MS2) These digital inputs control the microstepping mode. Possible settings are (MS1/MS2) : full step (0,0), half step (1,0), 1/4 step (0,1), and 1/8 step (1,1 : default)</param>
                public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin StepModePinOne, Cpu.Pin StepModePinTwo)
                {
                    _DirectionPin = new OutputPort(DirectionPin, true);  // Forward
                    _StepPin = new OutputPort(StepPin, false);
                    _StepModePinOne = new OutputPort(StepModePinOne, true); // Huitième de pas
                    _StepModePinTwo = new OutputPort(StepModePinTwo, true);
                }

                /// <summary>
                /// Creates an instance of the driver, that only lets you move, choose direction, sleep, and select step mode.
                /// </summary>
                /// <param name="DirectionPin">(DIR) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver up that way) digital signal.
                /// The level if this signal (high/low) is sampled on each rising edge of STEP to determine which direction to take the step (or microstep).</param>
                /// <param name="StepPin">(STEP) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver that way) digital signal. 
                /// Each rising edge of this signal will cause one step (or microstep) to be taken.</param>
                /// <param name="StepModePinOne">(MS1) These digital inputs control the microstepping mode. Possible settings are (MS1/MS2) : full step (0,0), half step (1,0), 1/4 step (0,1), and 1/8 step (1,1 : default)</param>
                /// <param name="StepModePinTwo">(MS2) These digital inputs control the microstepping mode. Possible settings are (MS1/MS2) : full step (0,0), half step (1,0), 1/4 step (0,1), and 1/8 step (1,1 : default)</param>
                /// <param name="SleepPin">(SLP) This normally high input signal will minimize power consumption by disabling 
                /// internal circuitry and the output drivers when pulled low.(disabled by default)</param>
                public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin StepModePinOne, Cpu.Pin StepModePinTwo, Cpu.Pin SleepPin)
                {
                    _DirectionPin = new OutputPort(DirectionPin, false); // Forward
                    _StepPin = new OutputPort(StepPin, false);
                    _StepModePinOne = new OutputPort(StepModePinOne, true); // Huitième de pas
                    _StepModePinTwo = new OutputPort(StepModePinTwo, true);
                    _SleepPin = new OutputPort(SleepPin, false); // Sleep acivé
                }

                /// <summary>
                /// Creates an instance of the driver, that only lets you move, choose direction, sleep, select step mode and enable / disable card
                /// </summary>
                /// <param name="DirectionPin">(DIR) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver up that way) digital signal.
                /// The level if this signal (high/low) is sampled on each rising edge of STEP to determine which direction to take the step (or microstep).</param>
                /// <param name="StepPin">(STEP) This needs to be a 0V to 5V (or 0V to 3.3V if you've set your Easy Driver that way) digital signal. 
                /// Each rising edge of this signal will cause one step (or microstep) to be taken.</param>
                /// <param name="StepModePinOne">(MS1) These digital inputs control the microstepping mode. Possible settings are (MS1/MS2) : full step (0,0), half step (1,0), 1/4 step (0,1), and 1/8 step (1,1 : default)</param>
                /// <param name="StepModePinTwo">(MS2) These digital inputs control the microstepping mode. Possible settings are (MS1/MS2) : full step (0,0), half step (1,0), 1/4 step (0,1), and 1/8 step (1,1 : default)</param>
                /// <param name="SleepPin">(SLP) This normally high input signal will minimize power consumption by disabling 
                /// internal circuitry and the output drivers when pulled low.(disabled by default)</param>
                /// <param name="EnablePin">(EN) This normally low input signal will disable all outputs when pulled high.</param>
                public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin StepModePinOne, Cpu.Pin StepModePinTwo, Cpu.Pin SleepPin, Cpu.Pin EnablePin)
                {
                    _DirectionPin = new OutputPort(DirectionPin, false); // Forward
                    _StepPin = new OutputPort(StepPin, false);
                    _StepModePinOne = new OutputPort(StepModePinOne, true); // Huitième de pas
                    _StepModePinTwo = new OutputPort(StepModePinTwo, true);
                    _SleepPin = new OutputPort(SleepPin, false); // Sleep activé
                    _EnablePin = new OutputPort(EnablePin, false); // Activation des sorties
                }

                /// <summary>
                /// Put the stepper driver to sleep mode
                /// </summary>
                /// <returns>Boolean : True if sleep</returns>
                public bool Sleep()
                {
                    if (_SleepPin != null)
                    {
                        _SleepPin.Write(false);
                        return true;
                    }
                    else
                        return false;
                }

                /// <summary>
                /// Wake up the stepper driver
                /// </summary>
                /// <returns>Boolean : True if WakeUp </returns>
                public bool WakeUp()
                {
                    if (_SleepPin != null)
                    {
                        _SleepPin.Write(true);
                        return true;
                    }
                    else
                        return false;
                }

                /// <summary>
                /// Enable the stepper driver outputs
                /// </summary>
                /// <returns>Boolean : True if Outputs are enable </returns>
                public bool EnableOutputs()
                {
                    if (_EnablePin != null)
                    {
                        _EnablePin.Write(false);
                        return true;
                    }
                    else
                        return false;
                }

                /// <summary>
                /// Disable the stepper driver outputs
                /// </summary>
                /// <returns>Boolean : True if Outputs are disable</returns>
                public bool DisableOutputs()
                {
                    if (_EnablePin != null)
                    {
                        _EnablePin.Write(true);
                        return true;
                    }
                    else
                        return false;
                }

                /// <summary>
                /// Moves the stepper motor
                /// </summary>
                /// <param name="steps">Indicate the amount of steps that need to be moved</param>
                /// <param name="stepdelay">Duration between steps</param>
                public void Turn(UInt32 steps, int stepdelay = 2)
                {
                    _Steps = steps; _StepDelay = stepdelay;
                    ChangeStepMode(_StepMode);
                    ChangeDirection(_StepDirection);
                    for (UInt32 i = 0; i < _Steps; i++)
                    {
                        _StepPin.Write(true);
                        Thread.Sleep(_StepDelay);
                        _StepPin.Write(false);
                    }
                }

                /// <summary>
                /// Moves the stepper motor
                /// </summary>
                /// <param name="steps">Indicate the amount of steps that need to be moved</param>
                /// <param name="direction">Indicates the direction of rotation</param>
                /// <param name="stepdelay">Duration between steps</param>
                /// <param name="mode">Full, Half, Quarter, or OneEighth step</param>                               
                public void Turn(UInt32 steps, Direction direction, int stepdelay = 2, Mode mode = Mode.OneEighth)
                {
                    _StepMode = mode; _StepDirection = direction; _StepDelay = stepdelay; _Steps = steps;
                    ChangeStepMode(mode);
                    ChangeDirection(direction);
                    for (UInt32 i = 0; i < _Steps; i++)
                    {
                        _StepPin.Write(true);
                        Thread.Sleep(_StepDelay);
                        _StepPin.Write(false);
                    }
                }

                /// <summary>
                /// Set Direction pin
                /// </summary>
                /// <param name="direction">Indicates the direction of rotation</param>
                private void ChangeDirection(Direction direction)
                {
                    switch (direction)
                    {
                        case Direction.Forward:
                            if (_DirectionPin != null)
                                _DirectionPin.Write(true);
                            break;
                        case Direction.Backward:
                            if (_DirectionPin != null)
                                _DirectionPin.Write(false);
                            break;
                    }
                }
                /// <summary>
                /// Set pins MS1 and MS2 
                /// </summary>
                /// <param name="mode">Full, Half, Quarter, or OneEighth step</param>
                private void ChangeStepMode(Mode mode)
                {
                    if (_StepModePinOne != null & _StepModePinTwo != null)
                    {
                        switch (mode)
                        {
                            case Mode.Full:
                                _StepModePinOne.Write(false);
                                _StepModePinTwo.Write(false);
                                break;
                            case Mode.Half:
                                _StepModePinOne.Write(true);
                                _StepModePinTwo.Write(false);
                                break;
                            case Mode.Quarter:
                                _StepModePinOne.Write(false);
                                _StepModePinTwo.Write(true);
                                break;
                            case Mode.OneEighth:
                                _StepModePinOne.Write(true);
                                _StepModePinTwo.Write(true);
                                break;
                        }
                    }
                }
            }
        }
    }
}
