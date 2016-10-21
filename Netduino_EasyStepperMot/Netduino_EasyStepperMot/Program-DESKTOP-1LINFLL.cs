using System;
using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using ToolBoxes;

namespace TestNetduinoStepper
{ // Description de la classe EasyStepperDriver : http://webge.github.io/EasyDriverStepperMotor/
    public class Program
    {
        public static void Main()
        {   // Programme de test d'un moteur pas à pas ITC-VNC-2
            // N=200 pas - U=12V - C=200g/cmavec une carte EasyStepperMotor 
            var time = 2000; UInt16 delay = 2; UInt32 nbpas = 200;
            var stepper = new EasyStepperDriver(Pins.GPIO_PIN_D13, Pins.GPIO_PIN_D12, Pins.GPIO_PIN_D2,Pins.GPIO_PIN_D10,Pins.GPIO_PIN_D11,Pins.GPIO_PIN_D3);

            stepper.WakeUp(); 
            while (true)
            {   // Exemples d'utilisation de la méthode Step() et des propriétés StepMode, StepDirection et Steptime
                stepper.EnableOutputs(); // Activation des sorties
                Debug.Print("Sleep= " + stepper.IsDriverSleep + " Enable= " + stepper.IsOutputsEnable);
                Debug.Print("Full Forward"); // 360° pour le moteur ITC-VNC-1
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Forward, delay,EasyStepperDriver.Mode.Full); 
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.DisableOutputs(); Thread.Sleep(time); // Désactivation des sorties pendant la temporisation

                Debug.Print("Half Backward"); stepper.EnableOutputs(); // 180° pour le moteur ITC-VNC-1
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Backward, delay, EasyStepperDriver.Mode.Half);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.DisableOutputs(); Thread.Sleep(time); // Désactivation des sorties pendant la temporisation

                Debug.Print("Quater Forward"); stepper.EnableOutputs();  // 90° pour le moteur ITC-VNC-1
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Forward, delay, EasyStepperDriver.Mode.Quarter);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.DisableOutputs(); Thread.Sleep(time); // Désactivation des sorties pendant la temporisation

                Debug.Print("OneEighth Backward"); stepper.EnableOutputs(); // 45° pour le moteur ITC-VNC-1
                stepper.Turn(nbpas, EasyStepperDriver.Direction.Backward, 1, EasyStepperDriver.Mode.OneEighth);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " time= " + stepper.StepDelay + "ms" + "\n");
                stepper.DisableOutputs(); Thread.Sleep(time); // Désactivation des sorties pendant la temporisation
                Thread.Sleep(2 * time);
            }
        }
    }
}
