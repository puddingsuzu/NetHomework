using System;
using System.Text;

namespace NetHomework2
{
    internal class HW2
    {
        private static byte[] m_PacketData;
        private static uint m_Pos;

        private static byte[] m_PacketDataPlusLeangh;
        private static int StringByteCount;
        private static string s = "Hello!";

        public static void Main(string[] args)
        {
            m_PacketDataPlusLeangh = new byte[1024];
            m_PacketData = new byte[1024];
            m_Pos = 0;

            Write(109);
            Write(109.99f);
            Write(s);
            Count();

            m_PacketDataPlusLeangh[0] = (byte)m_Pos;
            for (var i = 0; i < m_Pos; i++)
            {
                m_PacketDataPlusLeangh[i + 1] = m_PacketData[i];
            }

            Console.Write($"Output Byte array(length:{m_Pos}): ");
            Console.WriteLine();
            for (var i = 0; i < m_Pos + 1; i++)
            {
                Console.Write(m_PacketDataPlusLeangh[i] + ", ");
            }
            Console.WriteLine("\n");

            Console.WriteLine($"Output String array:");
            UnPacking(m_PacketDataPlusLeangh);
            Console.ReadKey();
        }

        // write an integer into a byte array
        private static bool Write(int i)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(i);
            _Write(bytes);
            return true;
        }

        // write a float into a byte array
        private static bool Write(float f)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(f);
            _Write(bytes);
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
                byteData.CopyTo(m_PacketDataPlusLeangh, m_Pos);
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
            int i = BitConverter.ToInt32(UnPackingData, 1);
            float f = BitConverter.ToSingle(UnPackingData, 5);
            string s = Encoding.Unicode.GetString(UnPackingData, 13, StringByteCount);
            Console.WriteLine(i);
            Console.WriteLine(f);
            Console.WriteLine(s);
        }
    }
}