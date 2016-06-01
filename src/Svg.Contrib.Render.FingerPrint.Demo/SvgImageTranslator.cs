using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  [PublicAPI]
  public class SvgImageTranslator : FingerPrint.SvgImageTranslator
  {
    // Q: why the fuck are barcodes implemented this way?
    // A: well, barcodes do not exist in the svg-spec, so the
    //    SvgImage is hijacked for this by setting the value
    //    in "data-barcode"-attribute. additional positioning
    //    and barcode-mode is done rather in the code, than
    //    further misuse the attribute, which might not be a
    //    reusable abstraction for multiple printer languages.
    //    in short: yes! you have to get your hands dirty...

    public SvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                              [NotNull] FingerPrintCommands fingerPrintCommands)
      : base(fingerPrintTransformer,
             fingerPrintCommands) {}

    [Pure]
    protected override void GetPosition([NotNull] SvgImage svgElement,
                                        [NotNull] Matrix matrix,
                                        out float sourceAlignmentWidth,
                                        out float sourceAlignmentHeight,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out int sector)
    {
      base.GetPosition(svgElement,
                       matrix,
                       out sourceAlignmentWidth,
                       out sourceAlignmentHeight,
                       out horizontalStart,
                       out verticalStart,
                       out sector);

      if (svgElement.HasNonEmptyCustomAttribute("data-barcode"))
      {
        if (sector % 2 == 0)
        {
          horizontalStart += (int) sourceAlignmentWidth;
        }
        else
        {
          horizontalStart -= (int) sourceAlignmentHeight;
          verticalStart += (int) sourceAlignmentHeight;
        }

        if (svgElement.ID == "RouteBc")
        {
          verticalStart -= 100;
        }
      }
    }

    protected override void AddTranslationToContainer([NotNull] SvgImage svgElement,
                                                      [NotNull] Matrix matrix,
                                                      float sourceAlignmentWidth,
                                                      float sourceAlignmentHeight,
                                                      int horizontalStart,
                                                      int verticalStart,
                                                      int sector,
                                                      [NotNull] FingerPrintContainer container)
    {
      if (svgElement.HasNonEmptyCustomAttribute("data-barcode"))
      {
        var barcode = svgElement.CustomAttributes["data-barcode"];
        var height = (int) sourceAlignmentHeight;
        var direction = this.FingerPrintTransformer.GetDirection(matrix);

        container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                             verticalStart));
        container.Body.Add(this.FingerPrintCommands.Direction(direction));
        container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
        container.Body.Add(this.FingerPrintCommands.BarCodeHeight(height));

        if (svgElement.ID == "CargoIdBc")
        {
          container.Body.Add(this.FingerPrintCommands.BarCodeMagnify(3));
          container.Body.Add(this.FingerPrintCommands.BarCodeType(BarCodeType.Code128));
        }
        else if (svgElement.ID == "RouteBc")
        {
          container.Body.Add(this.FingerPrintCommands.BarCodeMagnify(2));
          container.Body.Add(this.FingerPrintCommands.BarCodeType(BarCodeType.Code128));
        }
        else if (svgElement.ID == "ReceiverBc")
        {
          container.Body.Add(this.FingerPrintCommands.BarCodeMagnify(1));
          container.Body.Add(this.FingerPrintCommands.BarCodeType(BarCodeType.Code128));
        }
        else
        {
          throw new NotImplementedException();
        }
        container.Body.Add(this.FingerPrintCommands.PrintBarCode(barcode));
      }
      else
      {
        base.AddTranslationToContainer(svgElement,
                                       matrix,
                                       sourceAlignmentWidth,
                                       sourceAlignmentHeight,
                                       horizontalStart,
                                       verticalStart,
                                       sector,
                                       container);
      }
    }
  }
}