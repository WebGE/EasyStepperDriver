using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
 
namespace Stepper_Test
{
    public class EasyStepperDriver
    {
        private OutputPort _SleepPin;
        private OutputPort _DirectionPin;
        private OutputPort _StepPin;
        private OutputPort _StepModePinOne;
        private OutputPort _StepModePinTwo;
        private int _StepDelay = 2;
        private UInt32 _Steps = 0;
        private Mode _StepMode = Mode.Full;
        private Direction _StepDirection = Direction.Forward;

        public UInt32 Steps
        {
            get
            {
                return _Steps;
            }
        }
        /// <summary>
        /// Get or set direction
        /// </summary>      
        public Direction StepDirection 
        {
            get 
            { 
                return _StepDirection; 
            }
        }
        /// <summary>
        /// Get or set Mode
        /// </summary>
        public Mode StepMode 
        {
            get 
            { 
               return _StepMode;
            }
        }
        /// <summary>
        /// Get or set time between two step
        /// </summary>
        public int StepDelay
        {
            get
            {
                return _StepDelay;
            }
        }

        /// <summary>
        /// Creates an instance of the driver, that only lets you move and choose direction.
        /// </summary>
        /// <param name="DirectionPin">a digital pint that is used for direction</param>
        /// <param name="StepPin">a digital pint that is used for steps</param>
        public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin)
        {
            _DirectionPin = new OutputPort(DirectionPin, true);
            _StepPin = new OutputPort(StepPin, false);
        }
        /// <summary>
        /// Creates an instance of the driver, that only lets you move, choose direction and put the controller to sleep
        /// </summary>
        /// <param name="DirectionPin">a digital pint that is used for direction</param>
        /// <param name="StepPin">a digital pint that is used for steps</param>
        /// <param name="SleepPin">a digital pint that is used for sleep function</param>
        public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin SleepPin)
        {
            _DirectionPin = new OutputPort(DirectionPin, false);
            _StepPin = new OutputPort(StepPin, false);
            _SleepPin = new OutputPort(SleepPin, false);
        }
        /// <summary>
        /// Creates an instance of the driver, that only lets you move, choose direction, sleep, and select step mode
        /// </summary>
        /// <param name="DirectionPin">a digital pint that is used for direction</param>
        /// <param name="StepPin">a digital pint that is used for steps</param>
        /// <param name="SleepPin">a digital pint that is used for sleep function</param>
        /// <param name="StepModePinOne">pin one used to change step mode</param>
        /// <param name="StepModePinTwo">pin two used to change step mode</param>
        public EasyStepperDriver(Cpu.Pin DirectionPin, Cpu.Pin StepPin, Cpu.Pin SleepPin, Cpu.Pin StepModePinOne, Cpu.Pin StepModePinTwo)
        {
            _DirectionPin = new OutputPort(DirectionPin, false);
            _StepPin = new OutputPort(StepPin, false);
            _SleepPin = new OutputPort(SleepPin, false);
            _StepModePinOne = new OutputPort(StepModePinOne, true);
            _StepModePinTwo = new OutputPort(StepModePinTwo, true);
        }

        /// <summary>
        /// Put the stepper driver to sleep
        /// </summary>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public bool Sleep(bool sleep)
        {
            if (_SleepPin != null)
            {
                _SleepPin.Write(!sleep);
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
        public void GoStep(UInt32 Steps, Mode mode, Direction direction, int Delay=2)
        {            
            _StepMode = mode; _StepDirection = direction; _StepDelay = Delay; _Steps = Steps;
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