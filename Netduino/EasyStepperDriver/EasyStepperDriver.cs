using System;
using System.Threading;
using Microsoft.SPOT.Hardware;


namespace ToolBoxes
{
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


      // Properties
      // ------------------------------------------------------
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
        /// Get direction
        /// </summary>      
        public Direction StepDirection 
        {
            get 
            { 
                return _StepDirection; 
            }
        }

        /// <summary>
        /// Get Mode
        /// </summary>
        public Mode StepMode 
        {
            get 
            { 
               return _StepMode;
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


     // Constructors
     // ------------------------------------------------------
        /// <summary>
        /// Creates an instance of the driver, that only lets you move and choose direction.
        /// </summary>
        /// <param name="DirectionPin">Control EasyDriver DIR: a digital pin used for direction</param>
        /// <param name="StepPin">Control EasyDriver STEP: a digital pin used for steps</param>
        public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin)
        {
            _DirectionPin = new OutputPort(DirectionPin, true);  // Forward
            _StepPin = new OutputPort(StepPin, false);
        }

        /// <summary>
        /// Creates an instance of the driver, that only lets you move, choose direction and put the controller to sleep
        /// </summary>
        /// <param name="DirectionPin">Control EasyDriver DIR: a digital pin used for direction</param>
        /// <param name="StepPin">Control EasyDriver STEP: a digital pin used for steps</param>
        /// <param name="SleepPin">Control EasyDriver SLP: a digital pin used for sleep function (disable par default)</param>
        public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin SleepPin)
        {
            _DirectionPin = new OutputPort(DirectionPin, false); // Forward
            _StepPin = new OutputPort(StepPin, false);
            _SleepPin = new OutputPort(SleepPin, false); // Sleep activé
        }

        /// <summary>
        /// Creates an instance of the driver, that only lets you move, choose direction, sleep, and select step mode
        /// </summary>
        /// <param name="DirectionPin">Control EasyDriver DIR PIN: a digital pin used for direction</param>
        /// <param name="StepPin">Control EasyDriver STEP: a digital pin used for steps</param>
        /// <param name="SleepPin">Control EasyDriver SLP: a digital pin used for sleep function (disable par default)</param>
        /// <param name="StepModePinOne">Control EasyDriver MS1: a digital pin used for control mode</param>
        /// <param name="StepModePinTwo">Control EasyDriver MS2: a digital pin used for control mode</param>
        public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin SleepPin, Cpu.Pin StepModePinOne, Cpu.Pin StepModePinTwo)
        {
            _DirectionPin = new OutputPort(DirectionPin, false); // Forward
            _StepPin = new OutputPort(StepPin, false);
            _SleepPin = new OutputPort(SleepPin, false); // Sleep acivé
            _StepModePinOne = new OutputPort(StepModePinOne, true); // Huitième de pas
            _StepModePinTwo = new OutputPort(StepModePinTwo, true);
        }

        /// <summary>
        /// Creates an instance of the driver, that only lets you move, choose direction, sleep, select step mode and enable / disable card
        /// </summary>
        /// <param name="DirectionPin">Control EasyDriver DIR PIN: a digital pin used for direction</param>
        /// <param name="StepPin">Control EasyDriver STEP: a digital pin used for steps</param>
        /// <param name="SleepPin">Control EasyDriver SLP: a digital pin used for sleep function (disable par default)</param>
        /// <param name="StepModePinOne">Control EasyDriver MS1: a digital pin used for control mode</param>
        /// <param name="StepModePinTwo">Control EasyDriver MS2: a digital pin used for control mode</param>
        /// <param name="EnablePin">Control EasyDriver ENABLE: a digital pin used for Enable function</param>
        public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin SleepPin, Cpu.Pin StepModePinOne, Cpu.Pin StepModePinTwo, Cpu.Pin EnablePin)
        {
            _DirectionPin = new OutputPort(DirectionPin, false); // Forward
            _StepPin = new OutputPort(StepPin, false);
            _SleepPin = new OutputPort(SleepPin, false); // Sleep activé
            _StepModePinOne = new OutputPort(StepModePinOne, true); // Huitième de pas
            _StepModePinTwo = new OutputPort(StepModePinTwo, true);
            _EnablePin = new OutputPort(EnablePin, false); // Activation des sorties
        }


     // Public methodes
     // ------------------------------------------------------
        /// <summary>
        /// Put the stepper driver to sleep mode
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <param name="Steps">indicate the amount of steps that need to be moved</param>
        /// <param name="Delay">duration between steps</param>
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

    // Private methodes
    // ------------------------------------------------------
        /// <summary>
         /// Set Direction pin
         /// </summary>
         /// <param name="dir"></param>
        private void ChangeDirection(Direction dir)
        {
            switch (dir)
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
        /// <param name="mode"></param>
        private void ChangeStepMode(Mode mode)
        {
            if (_StepModePinOne != null & _StepModePinTwo != null)
            {
                switch (mode)
                {
                    case Mode.Full :
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

    // Enumerations
    // ------------------------------------------------------
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
    }
}