using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var label = new label();
      var svgDocument = label.SvgDocument;
      var bootstrapper = new CustomBootstrapper();
      var fingerPrintTransformer = bootstrapper.CreateFingerPrintTransformer();
      var fingerPrintRenderer = bootstrapper.CreateFingerPrintRenderer(fingerPrintTransformer);
      var viewMatrix = bootstrapper.CreateViewMatrix(fingerPrintTransformer,
                                                     90f,
                                                     203f,
                                                     ViewRotation.RotateBy90Degrees);
      var stopwatch = Stopwatch.StartNew();
      var fingerPrintContainer = fingerPrintRenderer.GetTranslation(svgDocument,
                                                                    viewMatrix);
      stopwatch.Stop();
      Console.WriteLine(stopwatch.Elapsed);

      var encoding = fingerPrintRenderer.GetEncoding();
      var array = fingerPrintContainer.ToByteStream(encoding)
                                      .ToArray();

      using (var serialPort = new SerialPort("COM1",
                                             115200,
                                             Parity.None,
                                             8,
                                             StopBits.Two)
                              {
                                Encoding = encoding,
                                NewLine = Environment.NewLine
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
