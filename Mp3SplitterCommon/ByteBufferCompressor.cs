using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mp3SplitterCommon {
    public class ByteBufferCompressor
    {
        private byte[] buffer;
        private double? speedChange;
        public ByteBufferCompressor(int length, double? speedChange) {
            buffer = null;
            this.speedChange = speedChange;
            if (speedChange != null)
                buffer = new byte[(int)(length / speedChange)];
        }

        public int Transform(byte[] raw, int size) {
            if (speedChange == null) {
                buffer = raw;
                return size;
            }
            var newLength = (int)(size / speedChange);
            var randy = new Random();
            for (int i = 0; i < newLength; i++) {
                var approximation = (int)(i * speedChange);
                if (approximation > buffer.Length - 1)
                    approximation = buffer.Length - 1;
                buffer[i] = raw[approximation];
                //buffer[i] = 0;
                //if (i % 4 == 0)
                //    buffer[i] = (byte)(randy.Next(150));
            }
            //buffer[i]
            return newLength;
        }
        public byte[] GetNewBuffer() {
            return buffer;
        }
    }
}
