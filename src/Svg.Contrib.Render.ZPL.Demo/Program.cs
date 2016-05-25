using System;
using System.Diagnostics;
using System.Svg;
using PInvoke;

// ReSharper disable UnusedParameter.Local
// ReSharper disable NonLocalizedString
// ReSharper disable ExceptionNotDocumented
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable ClassNeverInstantiated.Global

namespace Svg.Contrib.Render.ZPL.Demo
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var shouldWriteInternalMemory = false;
      var file = "assets/label.svg";
      var svgDocument = SvgDocument.Open(file);
      var bootstrapper = new CustomBootstrapper();
      var zplRenderer = bootstrapper.BuildUp(90f,
                                             203f,
                                             CharacterSet.ZebraCodePage850,
                                             ViewRotation.RotateBy270Degress);

      var encoding = zplRenderer.GetEncoding();

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

          if (shouldWriteInternalMemory)
          {
            //var zplStreams = zplRenderer.GetInternalMemoryTranslation(svgDocument);
            //foreach (var zplStream in zplStreams)
            //{
            //  var array = zplStream.ToByteArray(encoding);
            //  using (var safeObjectHandle = Kernel32.CreateFile(deviceInterfaceDetail,
            //                                                    Kernel32.FileAccess.FILE_GENERIC_WRITE,
            //                                                    Kernel32.FileShare.FILE_SHARE_WRITE,
            //                                                    IntPtr.Zero,
            //                                                    Kernel32.CreationDisposition.OPEN_EXISTING,
            //                                                    Kernel32.CreateFileFlags.FILE_ATTRIBUTE_NORMAL,
            //                                                    Kernel32.SafeObjectHandle.Null))
            //  {
            //    var arraySegment = new ArraySegment<byte>(array);
            //    Kernel32.WriteFile(safeObjectHandle,
            //                       arraySegment);
            //  }
            //}
          }

          {
            var stopwatch = Stopwatch.StartNew();
            var zplStream = zplRenderer.GetTranslation(svgDocument);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            var array = zplStream.ToByteArray(encoding);
            var arraySegment = new ArraySegment<byte>(array);
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
}