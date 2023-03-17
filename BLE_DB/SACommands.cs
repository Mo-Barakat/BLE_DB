using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data;
using System.Security.Cryptography;
using Sap.Data.SQLAnywhere;
using System.Runtime.InteropServices;

namespace BLE_DB
{
    public class SACommands
    {
        private static string FiTS_Sybase17 = "Data Source=FiTS Sybase 17";
        private SAConnection conn =new SAConnection(FiTS_Sybase17);
        private SACommand commitCmd;
        private static readonly Func<String> Log_Prefix = () => $"CPCWrapper::{Thread.CurrentThread.ManagedThreadId}::{DateTime.Now}:: ";
        public static readonly TraceListener fileListener = new TextWriterTraceListener("CPC_BLE.log");

        public SACommands()
        {
            Trace.Listeners.Add(fileListener);
            Trace.AutoFlush = true;
            Trace.WriteLine($"{Log_Prefix()}Lib v1 initialized. Writing to logfile CPC_BLE.log");
            Trace.Flush();
            commitCmd = new SACommand("commit;", conn);
        }

        public void SaveMeaurements(List<Measurement_Air> measurements)
        {
            conn.Open();
            foreach (Measurement_Air mAir in measurements)
            {
                mAir.OID = OIDGenerator();
                ExecuteNonQuery(mAir, false);
            }
            conn.Close();
        }
        public void UpdateMeaurements(List<Measurement_Air> measurements)
        {
            conn.Open();
            foreach (Measurement_Air mAir in measurements)
            {
                ExecuteNonQuery(mAir, true);
            }
            conn.Close();
        }

        private void ExecuteNonQuery(Measurement_Air mAir, bool update)
        {
            SADataAdapter da;
            DataSet ds = new DataSet();
            if (!(conn.State== ConnectionState.Open)) {conn.Open(); }
            

            if (update)
            {
                try
                {
                    da = new SADataAdapter("SELECT * FROM MEASUREMENT_AIR WHERE OID ='" + mAir.OID + ";", conn);
                    SACommandBuilder sqlCommandBuilder = new SACommandBuilder(da);
                    da.UpdateCommand = sqlCommandBuilder.GetUpdateCommand(true);
                    da.Fill(ds, "MEASUREMENT_AIR");
                    DataTable dt = ds.Tables["MEASUREMENT_AIR"];
                    DataRow rw = dt.Rows[0];
                    rw["tireMileage"] = mAir.TireMileage;
                    rw["measurementDate"] = mAir.MeasurementDate;
                    rw["measurementTacho"] = mAir.MeasurementTacho;
                    rw["MSMT_AIR_PRESSURE"] = mAir.MSMT_Air_Pressure;
                    rw["lastModifiedBy"] = mAir.LastModifiedBy;
                    rw["lastModifiedOn"] = mAir.LastModifiedOn;
                    rw["CPC_Temp"] = mAir.CPC_Temp;
                    rw["Air_Temp"] = mAir.Air_Temp;
                    rw["measurementTacho2"] = mAir.MeasurementTacho2;
                    rw["tireHours"] = mAir.TireHours;
                    rw["cpc_mileage"] = mAir.CPC_Mileage;
                    da.Update(dt);
                    Trace.WriteLine($"{Log_Prefix()} updating measurement for OID {mAir.OID}"); 
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"{Log_Prefix} Error updating measurement for OID {mAir.OID} {ex}"); ;
                }
            }
            else
                try
                {
                    da = new SADataAdapter("SELECT * FROM MEASUREMENT_AIR WHERE OID Is Null;", conn);
                    SACommandBuilder sqlCommandBuilder = new SACommandBuilder(da);
                    da.InsertCommand = sqlCommandBuilder.GetInsertCommand();
                    da.Fill(ds, "MEASUREMENT_AIR");
                    DataTable dt = ds.Tables["MEASUREMENT_AIR"];
                    DataRow rw = dt.Rows.Add();
                    rw["OID"] = mAir.OID;
                    rw["TireOID"] = mAir.TireOID;
                    rw["tireMileage"] = mAir.TireMileage;
                    rw["measurementDate"] = mAir.MeasurementDate;
                    rw["measurementTacho"] = mAir.MeasurementTacho;
                    rw["MSMT_AIR_PRESSURE"] = mAir.MSMT_Air_Pressure;
                    rw["remarks"] = mAir.Remarks;
                    rw["immutable"] = mAir.Immutable;
                    rw["notValid"] = mAir.NotValid;
                    rw["noFindings"] = mAir.NoFindings;
                    rw["lastModifiedBy"] = mAir.LastModifiedBy;
                    rw["lastModifiedOn"] = mAir.LastModifiedOn;
                    rw["CPC_Temp"] = mAir.CPC_Temp;
                    rw["Air_Temp"] = mAir.Air_Temp;
                    rw["measurementTacho2"] = mAir.MeasurementTacho2;
                    rw["tireHours"] = mAir.TireHours;
                    rw["cpc_mileage"] = mAir.CPC_Mileage;
                    da.Update(dt);        // Anzahl eingefügter Zeilen
                    Trace.WriteLine($"{Log_Prefix()} updating measurement for TireOID {mAir.TireOID}");
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"{Log_Prefix} Error updating measurement for tireOID {mAir.TireOID} {ex}"); ;
                }
            commitCmd.ExecuteNonQuery();
        }

        #region Functions
        private static int lastI = 0;
        private static string lastOID;
        public static string OIDGenerator()
        {          
            StringBuilder sb = new StringBuilder();
            String user = Environment.UserName;
            if (user.Length > 10){user = user.Substring(0, 10);}
            else {user = user.PadRight(10, 'x'); }
            
            sb.Append(user);
            DateTime sysTime = DateTime.Now;
            string date = sysTime.Year.ToString() + sysTime.Month.ToString().PadLeft(2,'0') + sysTime.Day.ToString().PadLeft(2, '0');
            string hexDate = Convert.ToInt64(date).ToString("X");
            string time = sysTime.Hour.ToString() + sysTime.Minute.ToString().PadLeft(2, '0') + sysTime.Second.ToString().PadLeft(2, '0') + Environment.TickCount.ToString().PadLeft(6, '0') + String.Format(lastI.ToString(), "000");
            string hexTime = Convert.ToInt64(time).ToString("X");
            sb.Append(hexDate);
            sb.Append(hexTime);
            string oid = sb.ToString().ToLower().Substring(0,25);
            if (oid.Equals(lastOID))
            {
                oid = oid.Replace('x', 'z').Replace('a', 'u').Replace('b', 'v').Replace('0', 'w').Replace('1', 'y');
                
            }
            lastOID = oid;
            lastI += 1;
            return oid;
        }
        #endregion
    }
}
