using System;
using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using testMicroToolskit.Hardware.MotorDrivers;

namespace TestNetduinoStepper
{
    public class Program
    {
        public static void Main()
        {   // http://astrojbm.free.fr/bricolages/picastro/Mecanique/ITC_CNC_1_FR.pdf
            // N=200 pas - U=12V - C=200g/cm - EasyStepperMotor v4.4 
            var time = 2000; UInt16 delay = 5; UInt32 nbpas = 200;
            var stepper = new EasyStepperDriver(Pins.GPIO_PIN_D13, Pins.GPIO_PIN_D12, Pins.GPIO_PIN_D10, Pins.GPIO_PIN_D11);

            while (true)
            {              
                Debug.Print("Full Forward"); // 360° for ITC-VNC-1 motor
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Forward, delay, EasyStepperDriver.Mode.Full);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                Thread.Sleep(time);

                Debug.Print("Half Backward"); // 180° for ITC-VNC-1 motor
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Backward, delay, EasyStepperDriver.Mode.Half);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");              
                Thread.Sleep(time); 

                Debug.Print("Quater Forward"); // 90° for ITC-VNC-1 motor
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Forward, delay, EasyStepperDriver.Mode.Quarter);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                Thread.Sleep(time); 

                Debug.Print("OneEighth Backward"); // 45° for ITC-VNC-1 motor
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Backward, 1, EasyStepperDriver.Mode.OneEighth);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                Thread.Sleep(time);

                Debug.Print("Full Forward"); // 45° for ITC-VNC-1 motor
                stepper.StepMode = EasyStepperDriver.Mode.Full; stepper.StepDirection = EasyStepperDriver.Direction.Backward;
                stepper.Turn(25);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                Thread.Sleep(2 * time);
            }
        }

    }
}
