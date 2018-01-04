using System;
using System.Threading;
using Microsoft.SPOT;
using Microtoolskit.Hardware.MotorDrivers;
using BrainpadLibrary;

namespace Brainpad
{
    public class Program
    {
        static bool toggle = false;

        public static void Main()
        {
            // Motor : SY57STH41-1006A (SYN=200 pas - U=5V - C=210g/cm) - Driver : STEPPER click 
            UInt16 delay = 5; UInt32 nbpas = 200;

            // STEPPER click on mikroBUS connector
            var stepper = new EasyStepperDriver(BrainPad.Expansion.Gpio.PC3, BrainPad.Expansion.Gpio.PA3);

            // Title
            BrainPad.Display.DrawLargeText(10, 10, "BrainPad", BrainPad.Color.Yellow);
            BrainPad.Display.DrawLargeText(30, 40, "STEPPER", BrainPad.Color.Yellow);
            BrainPad.Display.DrawLargeText(60, 60, "click", BrainPad.Color.Yellow);
            BrainPad.Display.DrawText(10, 120, "Left>Start   Right>Stop", BrainPad.Color.Yellow);
            BrainPad.Button.ButtonPressed += Button_ButtonPressed;

            while (true)
            {
                // 45° for SY57STH41-1006A motor
                if (toggle)
                {
                    stepper.StepMode = EasyStepperDriver.Mode.Full; stepper.StepDirection = EasyStepperDriver.Direction.Backward;
                    stepper.Turn(25, delay);
                    Thread.Sleep(1000);

                    // 360° for SY57STH41-1006A motor
                    stepper.Turn(nbpas, EasyStepperDriver.Direction.Forward, delay, EasyStepperDriver.Mode.Full);
                    Thread.Sleep(1000);
                }
                Thread.Sleep(10);
            }

        }

        private static void Button_ButtonPressed(BrainPad.Button.DPad button, BrainPad.Button.State state)
        {
            if (BrainPad.Button.IsRightPressed())
            {
                toggle = false;
            }
            else if (BrainPad.Button.IsLeftPressed())
            {
                toggle = true;
            }
        }
    }
}
