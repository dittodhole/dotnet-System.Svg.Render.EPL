using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var file = "assets/label.svg";
      var svgDocument = SvgDocument.Open(file);
      var bootstrapper = new DefaultBootstrapper();
      var fingerPrintRenderer = bootstrapper.BuildUp(90f,
                                                     203f);

      var stopwatch = Stopwatch.StartNew();
      var fingerPrintContainer = fingerPrintRenderer.GetTranslation(svgDocument);
      stopwatch.Stop();
      Console.WriteLine(stopwatch.Elapsed);

      var encoding = Encoding.Default;
      var array = fingerPrintContainer.ToByteStream(encoding)
                                      .ToArray();

      using (var serialPort = new SerialPort("COM1",
                                             115200,
                                             Parity.None)
                              {
                                DataBits = 8,
                                StopBits = StopBits.Two,
                                Encoding = encoding
                              })
      {
        serialPort.Open();
        serialPort.Write(array,
                         0,
                         array.Count());
      }
    }
  }
}