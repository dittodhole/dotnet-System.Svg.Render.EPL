using System;
using System.Diagnostics;
using System.Linq;
using PInvoke;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable NonLocalizedString
// ReSharper disable ExceptionNotDocumented
// ReSharper disable ExceptionNotDocumentedOptional

namespace Svg.Contrib.Render.EPL.Demo
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var file = "assets/label.svg";
      var svgDocument = SvgDocument.Open(file);
      var bootstrapper = new CustomBootstrapper();
      var eplTransformer = bootstrapper.CreateEplTransformer();
      var eplRenderer = bootstrapper.CreateEplRenderer(eplTransformer);
      var viewMatrix = bootstrapper.CreateViewMatrix(90f,
                                                     203f,
                                                     ViewRotation.RotateBy270Degress);

      var stopwatch = Stopwatch.StartNew();
      var eplContainer = eplRenderer.GetTranslation(svgDocument,
                                                    viewMatrix);
      stopwatch.Stop();
      Console.WriteLine(stopwatch.Elapsed);

      var encoding = eplRenderer.GetEncoding();
      var array = eplContainer.ToByteStream(encoding)
                              .ToArray();
      var arraySegment = new ArraySegment<byte>(array);

      var classGuid = new Guid("{28d78fad-5a12-11d1-ae5b-0000f803a8c2}");
      using (var safeDeviceInfoSetHandle = SetupApi.SetupDiGetClassDevs(classGuid,
                                                                        null,
                                                                        IntPtr.Zero,
                                                                        SetupApi.GetClassDevsFlags.DIGCF_PRESENT | SetupApi.GetClassDevsFlags.DIGCF_DEVICEINTERFACE))
      {
        foreach (var deviceInterfaceData in SetupApi.SetupDiEnumDeviceInterfaces(safeDeviceInfoSetHandle,
                                                                                 IntPtr.Zero,
                                                                                 classGuid))
        {
          var deviceInterfaceDetail = SetupApi.SetupDiGetDeviceInterfaceDetail(safeDeviceInfoSetHandle,
                                                                               deviceInterfaceData,
                                                                               IntPtr.Zero);

          using (var safeObjectHandle = Kernel32.CreateFile(deviceInterfaceDetail,
                                                            Kernel32.FileAccess.FILE_GENERIC_WRITE,
                                                            Kernel32.FileShare.FILE_SHARE_WRITE,
                                                            IntPtr.Zero,
                                                            Kernel32.CreationDisposition.OPEN_EXISTING,
                                                            Kernel32.CreateFileFlags.FILE_ATTRIBUTE_NORMAL,
                                                            Kernel32.SafeObjectHandle.Null))
          {
            Kernel32.WriteFile(safeObjectHandle,
                               arraySegment);
          }
        }
      }
    }
  }
}