using System;
using UnityEngine;

namespace WiimoteApi
{
    public class BalanceBoardData : WiimoteData
    {
        /// <summary> The 4 sensors on the Balance Board (short values) </summary>
        [Serializable]
        public struct BalanceBoardSensors
        {
            public BalanceBoardSensors(byte[] data)
            {
                TopRight = 0;
                TopLeft = 0;
                BottomRight = 0;
                BottomLeft = 0;
                ReadData(data);
            }

            private BalanceBoardSensors(short topRight, short topLeft, short bottomRight, short bottomLeft)
            { TopRight = topRight; TopLeft = topLeft; BottomRight = bottomRight; BottomLeft = bottomLeft; }

            public short TopRight;
            public short TopLeft;
            public short BottomRight;
            public short BottomLeft;

            public static BalanceBoardSensors operator-(BalanceBoardSensors lhs, BalanceBoardSensors rhs)
            {
                return new BalanceBoardSensors(
                    (short)(lhs.TopRight    - rhs.TopRight),
                    (short)(lhs.TopLeft     - rhs.TopLeft),
                    (short)(lhs.BottomRight - rhs.BottomRight),
                    (short)(lhs.BottomLeft  - rhs.BottomLeft));
            }

            public void ResetData()
            {
                TopRight = 0;
                TopLeft = 0;
                BottomRight = 0;
                BottomLeft = 0;
            }

            public void ReadData(byte[] data)
            {
                if (data == null || data.Length < 8)
                {
                    Debug.LogError("Error reading Sensor Data! ");
                    return;
                }

                // The Wii Balance Board sends big endian data 
                // We need to convert if the system is little endian 
                // Byte documentation can be found here: https://wiibrew.org/wiki/Wii_Balance_Board#Data_Format 

                byte[] topRightBytes = { data[0], data[1] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(topRightBytes);
                TopRight = BitConverter.ToInt16(topRightBytes, 0);

                byte[] bottomRightBytes = { data[2], data[3] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bottomRightBytes);
                BottomRight = BitConverter.ToInt16(bottomRightBytes, 0);

                byte[] topLeftBytes = { data[4], data[5] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(topLeftBytes);
                TopLeft = BitConverter.ToInt16(topLeftBytes, 0);

                byte[] bottomLeftBytes = { data[6], data[7] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bottomLeftBytes);
                BottomLeft = BitConverter.ToInt16(bottomLeftBytes, 0);
            }
        }

        /// <summary> The 4 sensors on the Balance Board (float values) </summary>
        [Serializable]
        public struct BalanceBoardSensorsF
        {
            public float TopRight;
            public float TopLeft;
            public float BottomRight;
            public float BottomLeft;

            public void ResetData()
            {
                TopRight = 0;
                TopLeft = 0;
                BottomRight = 0;
                BottomLeft = 0;
            }
        }

        /// <summary> Calabration Info (for 0kg, 17kg, and 34kg) </summary>
        [Serializable]
        public struct BalanceBoardCalabrationData
        {
            public BalanceBoardCalabrationData(byte[] data)
            {
                if (data == null || data.Length != 24) // there is 24 bytes of calabration data 
                {
                    Debug.LogError("Error reading Calabration Data! ");
                    kg0 = new BalanceBoardSensors();
                    kg17 = new BalanceBoardSensors();
                    kg34 = new BalanceBoardSensors();
                    return;
                }

                byte[] kg0Data = new byte[8]; // There are 8 bytes per weight 
                for (int i = 0; i < kg0Data.Length; i++)
                    kg0Data[i] = data[i];
                kg0 = new BalanceBoardSensors(kg0Data);

                byte[] kg17Data = new byte[8]; // There are 8 bytes per weight 
                for (int i = 0; i < kg17Data.Length; i++)
                    kg17Data[i] = data[i + 8]; // offset of 8 since we already read kg0 
                kg17 = new BalanceBoardSensors(kg17Data);

                byte[] kg34Data = new byte[8]; // There are 8 bytes per weight 
                for (int i = 0; i < kg34Data.Length; i++)
                    kg34Data[i] = data[i + 16];// offset of 8 since we already read kg0 and kg17 
                kg34 = new BalanceBoardSensors(kg34Data);
            }

            public BalanceBoardSensors kg0;
            public BalanceBoardSensors kg17;
            public BalanceBoardSensors kg34;
        }

        public BalanceBoardSensors RawSensorData { get { return _rawSensorData; } }
        private BalanceBoardSensors _rawSensorData;

        //public BalanceBoardCalabrationData CalabrationData { get { return _calabrationData; } }
        //private BalanceBoardCalabrationData _calabrationData;

        public BalanceBoardData(Wiimote owner/*, BalanceBoardCalabrationData calabrationData*/) : base(owner)
        {
            //_calabrationData = calabrationData;
        }

        public override bool InterpretData(byte[] data)
        {
            if (data == null || data.Length < 8)
            {
                // Zero out data here! 
                _rawSensorData.ResetData();

                return false;
            }

            /* ---=== Read in Raw Data ===--- */
            _rawSensorData.ReadData(data);

            return true;
        }
    }
}
