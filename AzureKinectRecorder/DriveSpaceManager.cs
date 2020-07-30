using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKinectRecorder
{
    class DriveSpaceManager
    {
        string StrDriveID;
        string RootDirectory;
        double Space_B_WarningBeforeRecording;
        double Space_B_WarningWhenRecording;
        double Space_B_Min;
        DriveInfo drive;


        double DATARATE_MB_PER_KINECT_PER_SECOND = 7.46;
        double WARNING_THRESHOLD_TIME_SECOND_BEFORE_RECORDING = 3600 * 1.5;
        double WARNING_THRESHOLD_TIME_SECOND_WHEN_RECORDING = 100;
        double RECONDING_FORBIDDEN_TIME_SECOND = 10;

        private static DriveSpaceManager singleton;

        private DriveSpaceManager() {
            Init();
        }

        private void Init() {
            RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            StrDriveID = RootDirectory.Substring(0, 1);
            drive = new DriveInfo(StrDriveID);
            numOfKinect = 2;
        }

        public static DriveSpaceManager GetInstance() {
            if (singleton == null) {
                singleton = new DriveSpaceManager();
            }
            return singleton;
        }

        int numOfKinect;
        public int NumOfkinect {
            set {
                numOfKinect = value;
                Space_B_Min = DATARATE_MB_PER_KINECT_PER_SECOND * numOfKinect * RECONDING_FORBIDDEN_TIME_SECOND * 1024 * 1024;
                Space_B_WarningWhenRecording = DATARATE_MB_PER_KINECT_PER_SECOND * numOfKinect * WARNING_THRESHOLD_TIME_SECOND_WHEN_RECORDING * 1024 * 1024;
                Space_B_WarningBeforeRecording = DATARATE_MB_PER_KINECT_PER_SECOND * numOfKinect * WARNING_THRESHOLD_TIME_SECOND_BEFORE_RECORDING * 1024 * 1024;
            }
        }

        public bool ShouldStopRecording
        {
            get
            {
                return drive.AvailableFreeSpace < Space_B_Min;
            }
        }

        public bool ShouldWarnWhenRecording {
            get {
                return drive.AvailableFreeSpace < Space_B_WarningWhenRecording;
            }
        }

        public bool ShouldWarnBeforeRecording {
            get {
                return drive.AvailableFreeSpace < Space_B_WarningBeforeRecording;
            }
        }

        public int GetMinutesByAvailabeSpace() {
            return (int)(drive.AvailableFreeSpace/1024/1024 / (DATARATE_MB_PER_KINECT_PER_SECOND * numOfKinect) / 60);
        }
    }
}
