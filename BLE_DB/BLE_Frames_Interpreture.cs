using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;


namespace BLE_DB
{
    public class BLE_Frames_Interpreture
    {
        /// <summary>
        /// TPMS Attributes 
        /// </summary>
        public static string TTM_generation = "";
        public static string Loose_detection = "";
        public static string WFC_ID = "";
        public static string Battery_status = "";
        public static string Flat_tire = "";
        public static double MSMT_Air_Pressure = 0;
        public static double CPC_Temp = 0;
        public static double Acceleration = 0;
        public static double CPC_Mileage = 0;
        public static double Battery_percentage = 0;

        public static int RSSI_InDevice = 0;

        /// <summary>
        /// IDS_filter
        /// </summary>
        public static string TDS_active = "";
        public static string TDS_direction = "";
        public static string TDS_raw = "";
        public static string TDS_test_mode = "";

        public static double TDS_excution_rate = 0;
        public static double Footprint_weight = 0;
        public static double Gradient_weight = 0;
        public static double Wheel_frequency_reference = 0;
        public static double Filter_Attenuation = 0;
        public static double Filter_cutoff_frequency = 0;



        public static double AVG_Radius = 0;



        /// <summary>
        /// constructor 
        /// </summary>
        public BLE_Frames_Interpreture()
        {

        }


        public static IDictionary<String, object> Interpret(byte[] frame, String frame_type)
        {


            switch (frame_type)
            {

                #region Advertisment

                case "ADV":
                    StringBuilder WFC_ID_SB_ADV = new System.Text.StringBuilder();

                    for (int i = 0; i < frame.Length; i++)
                    {
                        if (i == 1)
                        {
                            byte Byte_1 = Convert.ToByte(frame[i]);
                            if (IsBitSet(Byte_1, 1))
                            {
                                TTM_generation = "Gen 2";
                            }
                            else
                            {
                                TTM_generation = "Gen1";
                            }
                            if (IsBitSet(Byte_1, 0))
                            {
                                Loose_detection = "module loose";
                            }
                            else
                            {
                                Loose_detection = "No error";
                            }
                        }
                        if (i < 6 && i > 1)
                        {
                            WFC_ID_SB_ADV.Append(frame[i].ToString("X2"));
                        }
                        if (i == 6)
                        {
                            byte Byte_6 = Convert.ToByte(frame[i]);
                            if (IsBitSet(Byte_6, 2))
                            {
                                Flat_tire = "tire is flat";

                            }
                            else
                            {
                                Flat_tire = "tire is filled";

                            }
                            if (IsBitSet(Byte_6, 3))
                            {
                                Battery_status = "battery low";

                            }
                            else
                            {
                                Battery_status = "battery is OK";
                            }
                        }
                        if (i == 7)
                        {
                            MSMT_Air_Pressure = (frame[i] * 4.706) / 100;
                        }
                        if (i == 8)
                        {
                            CPC_Temp = (frame[i] - 32) / 1.8;
                        }

                        if (i == 9)
                        {
                            if (frame[i] == 0x01)
                            {
                                Acceleration = 0.5;
                            }
                            else if (frame[i] == 0x02)
                            {
                                Acceleration = 1.25;
                            }
                            else if (frame[i] > 0x02 && frame[i] < 0xFD)
                            {
                                Acceleration = 2;
                            }
                            else if (frame[i] >= 0xFD)
                            {
                                Acceleration = -1;
                            }
                            else
                            {
                                Acceleration = 0;
                            }

                        }
                        if (i == 10)
                        {
                            CPC_Mileage = frame[i];
                        }
                        if (i == 12)
                        {
                            Battery_percentage = frame[i] * 0.5;
                        }

                        WFC_ID = WFC_ID_SB_ADV.ToString();
                    }

                    String Advertisment = "TPMS_FRAME  | Timestamp:" + DateTime.Now + System.Environment.NewLine + " | " + "Name: " + WFC_ID.ToString() + " | " + " G:" + TTM_generation + " | " + " Loose detection:" + Loose_detection
                        + " | " + " Pressure: " + MSMT_Air_Pressure + " | " + " Flat tire detect:" + Flat_tire + " | " + " Temp: " + CPC_Temp.ToString() + " | " + " Acceleration: " + Acceleration + " | " + " Mileage: " + CPC_Mileage
                        + " | " + " Battery: " + Battery_percentage + " | " + " Battery status:" + Battery_status;
                    IDictionary<String, object> Advertismentx = new Dictionary<String, object>();
                    //Advertismentx.Add("Timestamp", DateTime.Now.ToString());
                    //Advertismentx.Add(nameof(WFC_ID), WFC_ID.ToString());

                    Advertismentx.Add(nameof(WFC_ID), WFC_ID);
                    Advertismentx.Add(nameof(TTM_generation), TTM_generation);
                    Advertismentx.Add(nameof(Loose_detection), Loose_detection);
                    Advertismentx.Add(nameof(Flat_tire), Flat_tire);
                    Advertismentx.Add(nameof(MSMT_Air_Pressure), MSMT_Air_Pressure);
                    Advertismentx.Add(nameof(CPC_Temp), CPC_Temp);
                    Advertismentx.Add(nameof(Acceleration), Acceleration);
                    Advertismentx.Add(nameof(CPC_Mileage), CPC_Mileage);
                    Advertismentx.Add(nameof(Battery_percentage), Battery_percentage);
                    Advertismentx.Add(nameof(Battery_status), Battery_status);

                    return Advertismentx;


                    break;

                #endregion

                #region TPMS
                case "TPMS":

                    StringBuilder WFC_ID_SB_TPMS = new System.Text.StringBuilder();
                    for (int i = 0; i < frame.Length; i++)
                    {
                        if (i == 0)
                        {
                            byte Byte_1 = Convert.ToByte(frame[i]);
                            if (IsBitSet(Byte_1, 1))
                            {
                                TTM_generation = "Gen 2";
                            }
                            else
                            {
                                TTM_generation = "Gen1";
                            }
                            if (IsBitSet(Byte_1, 0))
                            {
                                Loose_detection = "module loose";
                            }
                            else
                            {
                                Loose_detection = "No error";
                            }
                        }
                        if (i < 5 && i > 0)
                        {
                            WFC_ID_SB_TPMS.Append(frame[i].ToString("X2"));
                        }
                        if (i == 5)
                        {
                            byte Byte_6 = Convert.ToByte(frame[i]);
                            if (IsBitSet(Byte_6, 2))
                            {
                                Flat_tire = "tire is flat";

                            }
                            else
                            {
                                Flat_tire = "tire is filled";

                            }
                            if (IsBitSet(Byte_6, 3))
                            {
                                Battery_status = "battery low";

                            }
                            else
                            {
                                Battery_status = "battery is OK;";
                            }
                        }
                        if (i == 6)
                        {
                            MSMT_Air_Pressure = (frame[i] * 4.706) / 100;
                        }
                        if (i == 7)
                        {
                            CPC_Temp = (frame[i] - 32) / 1.8;
                        }

                        if (i == 8)
                        {
                            if (frame[i] == 0x01)
                            {
                                Acceleration = 0.5;
                            }
                            else if (frame[i] == 0x02)
                            {
                                Acceleration = 1.25;
                            }
                            else if (frame[i] > 0x02 && frame[i] < 0xFD)
                            {
                                Acceleration = 2;
                            }
                            else if (frame[i] >= 0xFD)
                            {
                                Acceleration = -1;
                            }
                            else
                            {
                                Acceleration = 0;
                            }

                        }
                        if (i == 9)
                        {
                            CPC_Mileage = frame[i];
                        }
                        if (i == 11)
                        {
                            Battery_percentage = frame[i] * 0.5;
                        }
                    }
                    WFC_ID = WFC_ID_SB_TPMS.ToString();
                    String TPMS_FRAMEs = "TPMS_FRAME  | Timestamp:" + DateTime.Now + System.Environment.NewLine + " | " + "Name: " + WFC_ID.ToString() + " | " + " G:" + TTM_generation + " | " + " Loose detection:" + Loose_detection
                        + " | " + " Pressure: " + MSMT_Air_Pressure + " | " + " Flat tire detect:" + Flat_tire + " | " + " Temp: " + CPC_Temp.ToString() + " | " + " Acceleration: " + Acceleration + " | " + " Mileage: " + CPC_Mileage
                        + " | " + " Battery: " + Battery_percentage + " | " + " Battery status:" + Battery_status;

                    IDictionary<String, object> TPMS_FRAME = new Dictionary<String, object>();
                    //TPMS_FRAME.Add("Timestamp", DateTime.Now.ToString());
                    TPMS_FRAME.Add(nameof(WFC_ID), WFC_ID);
                    TPMS_FRAME.Add(nameof(TTM_generation), TTM_generation);
                    TPMS_FRAME.Add(nameof(Loose_detection), Loose_detection);
                    TPMS_FRAME.Add(nameof(MSMT_Air_Pressure), MSMT_Air_Pressure);
                    TPMS_FRAME.Add(nameof(Flat_tire), Flat_tire);
                    TPMS_FRAME.Add(nameof(CPC_Temp), CPC_Temp);
                    TPMS_FRAME.Add(nameof(Acceleration), Acceleration);
                    TPMS_FRAME.Add(nameof(CPC_Mileage), CPC_Mileage);
                    TPMS_FRAME.Add(nameof(Battery_percentage), Battery_percentage);
                    TPMS_FRAME.Add(nameof(Battery_status), Battery_status);



                    return TPMS_FRAME;
                    //return null;

                    break;

                #endregion

                case "Mileage":

                    return null;
                    break;

                case "Vehicle_Data":


                    return null;

                    break;

                case "RSSI":



                    string RSSI_InDevice_temp = Convert.ToString(frame[0], 2);
                    // Convert the binary number to an integer
                    int binaryInt = Convert.ToInt32(RSSI_InDevice_temp, 2);
                    int decimalNumber = 0;
                    // Check if the number is negative
                    if ((binaryInt & (1 << (RSSI_InDevice_temp.Length - 1))) != 0)
                    {
                        // Invert all the bits
                        int invertedInt = ~binaryInt & 0x000000FF;

                        // Add 1 to the result
                        decimalNumber = -(invertedInt + 1);

                        // Print the decimal equivalent of the binary number
                        Console.WriteLine("The decimal equivalent of " + RSSI_InDevice_temp + " is " + decimalNumber);
                    }


                    IDictionary<String, object> RSSI_InDevice = new Dictionary<String, object>();
                    RSSI_InDevice.Add(nameof(decimalNumber), decimalNumber);
                    return RSSI_InDevice;

                    break;
                case "AVG_Radius":

                    byte[] Byte_R_12 = new byte[2];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        if (i == 0)
                        {
                            byte Byte_1 = frame[i];
                            Byte_R_12[i] = Byte_1;


                        }
                        if (i == 1)
                        {

                            byte Byte_2 = frame[i];
                            Byte_R_12[i] = Byte_2;


                        }
                       
                    }
                    ushort combined_R = (ushort)((Byte_R_12[0] << 8) | Byte_R_12[1]);
                    AVG_Radius = (double)combined_R*(double)0.1 ;



                    IDictionary<String, object> AVG_Radius_Frame = new Dictionary<String, object>();
                    //TPMS_FRAME.Add("Timestamp", DateTime.Now.ToString());

                    AVG_Radius_Frame.Add(nameof(AVG_Radius), AVG_Radius);
                 


                    return AVG_Radius_Frame;

                    return null;

                    break;
                case "TDS":


                   
                    for (int i = 0; i < frame.Length; i++)
                    {
                        if (i == 0)
                        {
                            byte Byte_1 = frame[i];
                           
                            if (IsBitSet(Byte_1, 7))
                            {

                                TDS_active = "Yes";
                            }
                            else
                            {
                                TDS_active = "No";
                            }
                            if (IsBitSet(Byte_1, 6))
                            {
                                TDS_direction = "BLE";

                                double iRate = (byte)((Byte_1) & 0b111);
                                TDS_excution_rate = Math.Pow(2, iRate) * 4;
                            }
                            else
                            {
                                TDS_direction = "RF";
                                double iRate = (byte)((Byte_1) & 0b111);
                                TDS_excution_rate = (double)Math.Pow(2, iRate) * ((double)128 / (double)60);


                            }
                            if (IsBitSet(Byte_1, 5))
                            {
                                TDS_raw = "Enabled";
                            }
                            else
                            {
                                TDS_raw = "Disabled";
                            }
                            if (IsBitSet(Byte_1, 3))
                            {
                                TDS_test_mode = "Yes";
                            }
                            else
                            {
                                TDS_test_mode = "NO";
                            }

                        }
                        if (i == 1)
                        {

                            byte Byte_2 = frame[i];
                            
                            double fpw = (byte)((Byte_2 >> 3) & 0b11);
                            Footprint_weight = 8 * Math.Pow(2, fpw);
                            double grw = (byte)((Byte_2 >> 5) & 0b11);
                            Gradient_weight = 8 * Math.Pow(2, grw);

                        }
                        
                    }
                   



                    IDictionary<String, object> TDS_FRAME = new Dictionary<String, object>();
                    //TPMS_FRAME.Add("Timestamp", DateTime.Now.ToString());
                    TDS_FRAME.Add(nameof(TDS_active), TDS_active);
                    TDS_FRAME.Add(nameof(TDS_direction), TDS_direction);
                    TDS_FRAME.Add(nameof(TDS_excution_rate), TDS_excution_rate);
                    TDS_FRAME.Add(nameof(TDS_raw), TDS_raw);
                    TDS_FRAME.Add(nameof(TDS_test_mode), TDS_test_mode);
                    TDS_FRAME.Add(nameof(Footprint_weight), Footprint_weight);
                    TDS_FRAME.Add(nameof(Gradient_weight), Gradient_weight);
                                        
                    


                    return TDS_FRAME;

                    

                    break;
                case "TDS_Filter":
                    byte[] Byte_12 = new byte[2];
                    for (int i = 0; i < frame.Length; i++)
                    {
                        if (i == 0)
                        {
                            byte Byte_1 = frame[i];
                            Byte_12[i] = Byte_1;
                           

                        }
                        if (i == 1)
                        {

                            byte Byte_2 = frame[i];
                            Byte_12[i] = Byte_2;
                            

                        }
                        if (i == 2)
                        {
                            byte Byte_3 = frame[i];

                            double att = (byte)((Byte_3 >> 3) & 0b1111);
                            Filter_Attenuation = 80 + att * 80;
                            double fc0 = (byte)((Byte_3) & 0b111);
                            Filter_cutoff_frequency = 300 + fc0 * 50;

                        }
                    }
                    ushort combined = (ushort)((Byte_12[0] << 8) | Byte_12[1]);
                    Wheel_frequency_reference = (double)combined / (double)100;



                    IDictionary<String, object> TDS_Filter_FRAME = new Dictionary<String, object>();
                    //TPMS_FRAME.Add("Timestamp", DateTime.Now.ToString());
                    
                    TDS_Filter_FRAME.Add(nameof(Wheel_frequency_reference), Wheel_frequency_reference);
                    TDS_Filter_FRAME.Add(nameof(Filter_Attenuation), Filter_Attenuation);
                    TDS_Filter_FRAME.Add(nameof(Filter_cutoff_frequency), Filter_cutoff_frequency);


                    return TDS_Filter_FRAME;

                    break;

                default:
                    return null;
                    // code block
                    break;
            }


        }
        public static bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

    }
}
