using GHI.Pins;
using Microsoft.SPOT;
using System;
using System.Threading;
using testMicroToolskit.Hardware.MotorDrivers;

namespace FezPanda
{
    public class Program
    {
        public static void Main()
        {   // http://astrojbm.free.fr/bricolages/picastro/Mecanique/ITC_CNC_1_FR.pdf
            // N=200 pas - U=12V - C=200g/cm - EasyStepperMotor v4.4 
            var time = 2000; UInt16 delay = 2; UInt32 nbpas = 200;
            var stepper = new EasyStepperDriver(FEZPandaIII.Gpio.D13, FEZPandaIII.Gpio.D12, FEZPandaIII.Gpio.D10, FEZPandaIII.Gpio.D11);

            while (true)
            {
                Debug.Print("Full Forward"); // 360° for ITC-VNC-1 motor               
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Forward, delay, EasyStepperDriver.Mode.Full);
                Thread.Sleep(time);

                Debug.Print("Half Backward"); // 180° for ITC-VNC-1 motor               
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Backward, delay, EasyStepperDriver.Mode.Half);
                Thread.Sleep(time);

                Debug.Print("Quater Forward"); // 90° for ITC-VNC-1 motor                
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Forward, delay, EasyStepperDriver.Mode.Quarter);
                Thread.Sleep(time);

                Debug.Print("OneEighth Backward"); // 45° for ITC-VNC-1 motor               
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Backward, 1, EasyStepperDriver.Mode.OneEighth);
                Thread.Sleep(time);

                Debug.Print("Full Forward"); // 45° for ITC-VNC-1 motor                
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.StepMode = EasyStepperDriver.Mode.Full; stepper.StepDirection = EasyStepperDriver.Direction.Backward;
                stepper.Turn(25);
                Thread.Sleep(2 * time);
            }
        }

    }
}
