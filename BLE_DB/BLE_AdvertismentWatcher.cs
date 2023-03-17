using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace BLE_DB
{
    public partial class BLE_AdvertismentWatcher
    {

        #region Public Properties

        /// <summary>
        /// Indicates if this watcher is listening for advertisements
        /// </summary>
        public bool Listening => BLE_Watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started;

        #endregion


        #region Private Members

        private BluetoothLEManufacturerData manufacturerData = new BluetoothLEManufacturerData();

        /// <summary>
        /// class Members 
        /// </summary>
        private readonly BluetoothLEAdvertisementWatcher BLE_Watcher;

        /// <summary>
        /// A thread lock object for this class 
        /// </summary>
        private readonly object mThreadLock = new object();

        #endregion



        #region Public Events

        /// <summary>
        /// Fired when the bluetooth watcher stops listening
        /// </summary>
        public event Action StoppedListening = () => { };

        /// <summary>
        /// Fired when the bluetooth watcher starts listening
        /// </summary>
        public event Action StartedListening = () => { };


        /// <summary>
        /// Fired when a device is discovered
        /// </summary>
        public event Action<BLE_Advertisment> Advertisment_Received = (advertisment) => { };


        #endregion


        /// <summary>
        /// constructor 
        /// </summary>
        public BLE_AdvertismentWatcher()
        {
            BLE_Watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };

            manufacturerData.CompanyId = 0x4B;
            BLE_Watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);

            // Set the in-range threshold to -70dBm. This means advertisements with RSSI >= -70dBm 
            // will start to be considered "in-range" (callbacks will start in this range).
            BLE_Watcher.SignalStrengthFilter.InRangeThresholdInDBm = 0;

            // Set the out-of-range threshold to -75dBm (give some buffer). Used in conjunction 
            // with OutOfRangeTimeout to determine when an advertisement is no longer 
            // considered "in-range".
            BLE_Watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -100;

            // Set the out-of-range timeout to be 2 seconds. Used in conjunction with 
            // OutOfRangeThresholdInDBm to determine when an advertisement is no longer 
            // considered "in-range"
            BLE_Watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(8000);

            BLE_Watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(4);


            BLE_Watcher.Received += BLE_Watcher_Received;


            // Listen out for when the watcher stops listening
            BLE_Watcher.Stopped += (watcher, e) =>
            {
                // Inform listeners
                StoppedListening();
            };

        }

        private void BLE_Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {

            //// Cleanup Timeouts
            //CleanupTimeouts();


            /// edit*******
            /// 
            var manufacturerSections = args.Advertisement.ManufacturerData;

            if (manufacturerSections.Count > 0)
            {
                var manufacturerData = manufacturerSections[0];
                var input = new byte[manufacturerData.Data.Length];
                using (var reader = DataReader.FromBuffer(manufacturerData.Data))
                {
                    reader.ReadBytes(input);
                }

                BLE_Advertisment BLE_Advertisment_Received = new BLE_Advertisment(args.BluetoothAddress, args.Advertisement.LocalName, args.RawSignalStrengthInDBm, args.Timestamp, input);

                // Null guard
                if (BLE_Advertisment_Received == null)
                    return;



                Advertisment_Received(BLE_Advertisment_Received);
            }
            else
            {
                return;
            }
        }

        #region Public Methods start/end

        /// <summary>
        /// Starts listening for advertisements
        /// </summary>
        public void StartListening()
        {
            lock (mThreadLock)
            {
                // If already listening...
                if (Listening)
                    // Do nothing more
                    return;

                // Start the underlying watcher
                BLE_Watcher.Start();
            }

            // Inform listeners
            StartedListening();
        }

        /// <summary>
        /// Stops listening for advertisements
        /// </summary>
        public void StopListening()
        {
            lock (mThreadLock)
            {
                // If we are no currently listening...
                //if (!Listening)
                //    // Do nothing more
                //    return;

                // Stop listening
                BLE_Watcher.Stop();
                Console.WriteLine("Stopped");

                //// Clear any devices
                //mDiscoveredDevices.Clear();
            }
        }
        #endregion
    }
}
