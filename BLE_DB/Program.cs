using BLE_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLE_DB
{
    internal class Program
    {
         static void Main(string[] args)
        {
            //List<Measurement_Air> measurements = new List<Measurement_Air>();
            //Measurement_Air measurement_Air = new Measurement_Air();
            //measurement_Air.MSMT_Air_Pressure = 9;
            //measurement_Air.Air_Temp = 22;
            //measurement_Air.CPC_Mileage = 123456;
            //measurement_Air.CPC_Temp = 46.2;
            //measurements.Add(measurement_Air);
            //Measurement_Air measurement_Air2 = new Measurement_Air();
            //measurement_Air2.MSMT_Air_Pressure = 8.4;
            //measurement_Air2.Air_Temp = 21.5;
            //measurement_Air2.CPC_Mileage = 237440;
            //measurement_Air2.CPC_Temp = 22.2;
            //measurements.Add(measurement_Air2);
            //Measurement_Air measurement_Air3 = new Measurement_Air();
            //measurement_Air3.MSMT_Air_Pressure = 2.4;
            //measurement_Air3.Air_Temp = -5.5;
            //measurement_Air3.CPC_Mileage = 654321;
            //measurement_Air3.CPC_Temp = 6.2;
            //measurements.Add(measurement_Air3);
            //Measurement_Air measurement_Air4 = new Measurement_Air();
            //measurement_Air4.MSMT_Air_Pressure = 5.4;
            //measurement_Air4.Air_Temp = 0.5;
            //measurement_Air4.CPC_Mileage = 555111;
            //measurement_Air4.CPC_Temp = 22.2;
            //measurements.Add(measurement_Air4);
            //SACommands sACommands = new SACommands();
            //sACommands.SaveMeaurements(measurements);


            BLE_AdvertismentWatcher watcher = new BLE_AdvertismentWatcher();
            // Hook into events
            watcher.StartedListening += () =>
            {
                Console.WriteLine("Started listening");
            };
            watcher.Advertisment_Received += (adv) =>
            {
                Console.WriteLine("New device Advertisment:" + Environment.NewLine + string.Join(" ", adv.ADV_Interpreted));
                List<Measurement_Air> measurements = new List<Measurement_Air>();
                Measurement_Air measurement_Air = new Measurement_Air();
                measurement_Air.MSMT_Air_Pressure = adv.MSMT_Air_Pressure;
                measurement_Air.Air_Temp = adv.CPC_Temp;
                measurement_Air.CPC_Mileage = adv.CPC_Mileage;
                measurement_Air.CPC_Temp = adv.CPC_Temp;

                measurements.Add(measurement_Air);


                //this.BeginInvoke((MethodInvoker)delegate
                //{
                    SACommands sACommands = new SACommands();
                    sACommands.SaveMeaurements(measurements);
                //});
               

            };

            // Start listening
            watcher.StartListening();
            while (true)
            {

            }
        }
    }
}
