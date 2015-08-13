using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3SplitterSimple {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Please drag a huge file into me!");
                Console.ReadKey();
                return;
            }
            foreach (var aaa in args) {
                new SimpleSplitter(aaa);            
            }
        }
    }
}
