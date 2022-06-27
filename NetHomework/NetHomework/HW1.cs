using System;
using System.Text;

namespace NetHomework1
{
    internal class HW1
    {
        private static byte[] m_NoReversePacketData;
        private static byte[] m_PacketData;
        private static uint m_Pos;

        private static int DataLeangh;
        private static int StringByteCount;
        private static string s = "Hello!";

        public static void Main(string[] args)
        {
            m_NoReversePacketData = new byte[1024];
            m_PacketData = new byte[1024];
            m_Pos = 0;
            DataLeangh = 0;

            Write(109);
            Write(109.99f);
            Write(s);
            Count();

            Console.Write($"Output Byte array(length:{m_Pos}): ");
            Console.WriteLine();
            for (var i = 0; i < m_Pos; i++)
            {
                Console.Write(m_PacketData[i] + ", ");
            }
            Console.WriteLine("\n");

            Console.WriteLine($"Output String array(length:{DataLeangh}):");
            UnPacking(m_NoReversePacketData);
            Console.ReadKey(); //中斷點
        }

        // write an integer into a byte array
        private static bool Write(int i)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(i);
            _Write(bytes);
            DataLeangh++;
            return true;
        }

        // write a float into a byte array
        private static bool Write(float f)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(f);
            _Write(bytes);
            DataLeangh++;
            return true;
        }

        // write a string into a byte array
        private static bool Write(string s)
        {
            // convert string to byte array
            var bytes = Encoding.Unicode.GetBytes(s);

            // write byte array length to packet's byte array
            if (Write(bytes.Length) == false)
            {
                return false;
            }
            _Write(bytes);
            return true;
        }

        // write a byte array into packet's byte array
        private static void _Write(byte[] byteData)
        {
            // converter little-endian to network's big-endian
            if (BitConverter.IsLittleEndian)
            {
                byteData.CopyTo(m_NoReversePacketData, m_Pos);
                Array.Reverse(byteData);
            }

            byteData.CopyTo(m_PacketData, m_Pos);
            m_Pos += (uint)byteData.Length;
        }

        private static int Count()
        {
            StringByteCount = s.Length * 2;
            return StringByteCount;
        }

        private static void UnPacking(byte[] UnPackingData)
        {
            int i = BitConverter.ToInt32(m_NoReversePacketData, 0);
            float f = BitConverter.ToSingle(m_NoReversePacketData, 4);
            string s = Encoding.Unicode.GetString(m_NoReversePacketData, 12, StringByteCount);
            Console.WriteLine(i);
            Console.WriteLine(f);
            Console.WriteLine(s);
        }
    }
}