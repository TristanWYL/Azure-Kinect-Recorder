using System;
using System.Drawing;

namespace CameraCaptureUIExample
{
    class Program
    {
        static void MethodA() {
            using (Test test = new Test("Test A")) { 
                test.Write();
            }
        }

        static void MethodB()
        {
            Test test = new Test("Test B");
            test.Write();
        }

        static void Main(string[] args)
        {
            // Using Windows.Media.Capture.CameraCaptureUI API to capture a photo
            //CameraCaptureUI dialog = new CameraCaptureUI();
            //Size aspectRatio = new Size(16, 9);
            //dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

            //StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);

            MethodA();
            GC.Collect();
            MethodB();
            
        }
    }

    class Test: IDisposable
    {
        String label;
        public Test(String label) { this.label = label; }
        public void Write() {
            Console.WriteLine($"{label} is writing Something!");
        }
        public void Dispose() { 
            
        }
        ~Test() { Console.WriteLine($"{label} is being destroyed."); }
    }
}
