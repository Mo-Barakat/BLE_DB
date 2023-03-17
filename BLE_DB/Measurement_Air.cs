using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Sap.Data.SQLAnywhere;


namespace BLE_DB
{

    public class Measurement_Air
    {

        public Measurement_Air(){}

        #region Properties
        public string OID { get; set; }
        public string TireOID { get; set; }
        public double TireMileage { get; set; }
        public DateTime MeasurementDate { get; set; } = DateTime.Now;
        public double MeasurementTacho { get; set; }
        public double MSMT_Air_Pressure { get;  set; }
        public string Remarks { get; internal set; }
        public int Immutable { get; set; } = 0;
        public int NotValid { get; set; } = 0;
        public int NoFindings { get; set; } = 0;
        public DateTime LastModifiedOn { get; internal set; } = DateTime.Now;
        public string LastModifiedBy { get; internal set; } = Environment.UserName;
        public double CPC_Temp { get; set; }
        public double Air_Temp { get; set; } = -1;
        public double MeasurementTacho2 { get; set; } = -1;
        public double TireHours { get; set; } = -1;
        public double CPC_Mileage { get; set; }
        #endregion

    }
}
