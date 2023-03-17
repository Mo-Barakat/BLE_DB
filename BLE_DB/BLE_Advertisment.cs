using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLE_DB
{


    public class BLE_Advertisment
    {


        /// <summary>
        /// BLE Frames device Attributes 
        /// </summary>
        public DateTimeOffset BroadcastTime { get; }
        public ulong Address { get; set; }
        public string Name { get; }
        public short SignalStrengthInDB { get; }

        /// <summary>
        /// ADV Frame Attributes 
        /// </summary>
        public  string TTM_generation { get; set; }
        public  string Loose_detection { get; set; }
        public  String WFC_ID { get; set; }
        public  string Battery_status { get; set; }
        public  string Flat_tire { get; set; }
        public  double MSMT_Air_Pressure { get; set; }
        public double CPC_Temp { get; set; }
        public double Acceleration { get; set; }
        public double CPC_Mileage { get; set; }
        public double Battery_percentage { get; set; }

        public byte[] ADV_Byte { get; set; }

        public IDictionary<String, object> ADV_Interpreted { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="address"></param>
        /// <param name="name"></param>
        /// <param name="rssi"></param>
        /// <param name="broadcastTime"></param>
        /// <param name="ADV"></param>
        /// <param name="mileage"></param>
        /// <param name="error_status"></param>
        /// <param name="avg_radius"></param>
        /// <param name="tds"></param>
        /// <param name="tds_filter"></param>
        public BLE_Advertisment(ulong address, string name, short rssi, DateTimeOffset broadcastTime, byte[] adv)
        {

            Address = address;
            Name = name;
            SignalStrengthInDB = rssi;
            BroadcastTime = broadcastTime;
            ADV_Byte= adv;
            

            ADV_Interpreted = BLE_Frames_Interpreture.Interpret(ADV_Byte, "ADV");

            TTM_generation = (String)ADV_Interpreted[nameof(TTM_generation)];
            Loose_detection = (String)ADV_Interpreted[nameof(Loose_detection)];
            WFC_ID = (String)ADV_Interpreted[nameof(WFC_ID)];
            Battery_status = (String)ADV_Interpreted[nameof(Battery_status)];
            Flat_tire = (String)ADV_Interpreted[nameof(Flat_tire)];
            MSMT_Air_Pressure = (double)ADV_Interpreted[nameof(MSMT_Air_Pressure)];
            CPC_Temp = (double)ADV_Interpreted[nameof(CPC_Temp)];
            Acceleration = (double)ADV_Interpreted[nameof(Acceleration)];
            CPC_Mileage = (double)ADV_Interpreted[nameof(CPC_Mileage)];
            Battery_percentage = (double)ADV_Interpreted[nameof(Battery_percentage)];

        }

    }


}
