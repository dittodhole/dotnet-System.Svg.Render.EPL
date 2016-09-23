using System;
using System.Diagnostics;
using System.Linq;
using PInvoke;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable NonLocalizedString
// ReSharper disable ExceptionNotDocumented
// ReSharper disable ExceptionNotDocumentedOptional

namespace Svg.Contrib.Render.ZPL.Demo
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var label = new label();
      var svgDocument = label.SvgDocument;
      var bootstrapper = new CustomBootstrapper();
      var zplTransformer = bootstrapper.CreateZplTransformer();
      var zplRenderer = bootstrapper.CreateZplRenderer(zplTransformer);
      var viewMatrix = bootstrapper.CreateViewMatrix(zplTransformer,
                                                     90f,
                                                     203f,
                                                     ViewRotation.RotateBy270Degress);
      var stopwatch = Stopwatch.StartNew();
      var zplContainer = zplRenderer.GetTranslation(svgDocument,
                                                    viewMatrix);
      stopwatch.Stop();
      Console.WriteLine(stopwatch.Elapsed);

      var encoding = zplRenderer.GetEncoding();
      var array = zplContainer.ToByteStream(encoding)
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
