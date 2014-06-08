using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Stepper_Test;

namespace Netduino_EasyStepperMot
{
    public class Program
    {
        public static void Main()
        {   // Programme de test d'un moteur pas à pas ITC-VNC-1 http://astrojbm.free.fr/bricolages/picastro/Mecanique/ITC_CNC_1_FR.pdf
            // avec une carte EasyStepperMotor
            // N=200 pas - U=12V - C=200g/cm
            var time = 2000;
            var stepper = new EasyStepperDriver(Pins.GPIO_PIN_D8, Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D10, Pins.GPIO_PIN_D11, Pins.GPIO_PIN_D12);

            while (true)
            {   // Exemples d'utilisation de la méthode Step() et des propriétés StepMode, StepDirection et StepDelay
                stepper.Sleep(false);
                Debug.Print("Full Forward");
                stepper.GoStep(200,EasyStepperDriver.Mode.Full,EasyStepperDriver.Direction.Forward); // 360° pour le moteur ITC-VNC-1
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " delay= " + stepper.StepDelay + "ms" + "\n");
                Thread.Sleep(time);

                Debug.Print("Half Backward");
                stepper.GoStep(200, EasyStepperDriver.Mode.Half,EasyStepperDriver.Direction.Backward); // 180° pour le moteur ITC-VNC-1
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " delay= " + stepper.StepDelay + "ms" + "\n");
                Thread.Sleep(time);

                Debug.Print("Quater Forward"); // 90° pour le moteur ITC-VNC-1
                stepper.GoStep(200, EasyStepperDriver.Mode.Quarter, EasyStepperDriver.Direction.Forward);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " delay= " + stepper.StepDelay + "ms" + "\n");
                Thread.Sleep(time);

                Debug.Print("OneEighth Backward"); // 45° pour le moteur ITC-VNC-1
                stepper.GoStep(200,EasyStepperDriver.Mode.OneEighth,EasyStepperDriver.Direction.Backward,1);
                Debug.Print("Pas= " + stepper.Steps + " Mode= " + stepper.StepMode + " Dir= " + stepper.StepDirection + " delay= " + stepper.StepDelay + "ms" + "\n");
                
                stepper.Sleep(true);
                Thread.Sleep(5*time);
            }
        }
    }
}
