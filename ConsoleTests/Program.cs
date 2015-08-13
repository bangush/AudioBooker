using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AudioBooker.classes;
using Mp3SplitterCommon;

namespace ConsoleTests {
    class ConsoleTests {
        static void Main(string[] args) {
            new ConsoleTests();
        }

        public ConsoleTests() {

            //ABTests.WriteFrankerBook(
            //    @"C:\Users\mtemkine\Documents\mikhailshit\lingodome\casanova\casa_evasion.wav.xml",
            //    @"C:\Users\mtemkine\Documents\mikhailshit\lingodome\casanova\casa_evasion-test.wav"
            //);

            ABTests.WriteFrankerBook(
                @"C:\WS\jmerde\trunk\_VisualStudio\AudioBooker\IlyaFranker\bin\Debug\sd.wav.xml",
                @"../../shooooooooooooot.wav",
                1,
                1
            );

            //new TestMix();
            //new TestSpeed();

            Console.WriteLine("Done.");
        }
    }
}
