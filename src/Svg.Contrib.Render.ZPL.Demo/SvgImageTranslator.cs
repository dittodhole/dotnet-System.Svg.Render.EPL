using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.ZPL.Demo
{
  [PublicAPI]
  public class SvgImageTranslator : ZPL.SvgImageTranslator
  {
    // Q: why the fuck are barcodes implemented this way?
    // A: well, barcodes do not exist in the svg-spec, so the
    //    SvgImage is hijacked for this by setting the value
    //    in "data-barcode"-attribute. additional positioning
    //    and barcode-mode is done rather in the code, than
    //    further misuse the attribute, which might not be a
    //    reusable abstraction for multiple printer languages.
    //    in short: yes! you have to get your hands dirty...

    public SvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                              [NotNull] ZplCommands zplCommands)
      : base(zplTransformer,
             zplCommands) {}

    [Pure]
    protected override void GetPosition([NotNull] SvgImage svgElement,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        out float sourceAlignmentWidth,
                                        out float sourceAlignmentHeight,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out int sector)
    {
      base.GetPosition(svgElement,
                       sourceMatrix,
                       viewMatrix,
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
          verticalStart -= (int) sourceAlignmentHeight;
        }
        else
        {
          horizontalStart += (int) sourceAlignmentHeight;
        }
      }

      if (svgElement.ID == "RouteBc")
      {
        verticalStart -= 30;
      }
    }

    protected override void AddTranslationToContainer([NotNull] SvgImage svgElement,
                                                      [NotNull] Matrix sourceMatrix,
                                                      [NotNull] Matrix viewMatrix,
                                                      float sourceAlignmentWidth,
                                                      float sourceAlignmentHeight,
                                                      int horizontalStart,
                                                      int verticalStart,
                                                      int sector,
                                                      [NotNull] ZplContainer container)
    {
      if (svgElement.HasNonEmptyCustomAttribute("data-barcode"))
      {
        var barcode = svgElement.CustomAttributes["data-barcode"];

        var height = (int) sourceAlignmentHeight;
        var fieldOrientation = this.ZplTransformer.GetFieldOrientation(sourceMatrix,
                                                                       viewMatrix);

        if (svgElement.ID == "CargoIdBc")
        {
          container.Body.Add(this.ZplCommands.BarCodeFieldDefaut(3,
                                                                 2,
                                                                 height));
          container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                           verticalStart));
          container.Body.Add(this.ZplCommands.Interleaved2Of5BarCode(fieldOrientation,
                                                                     height,
                                                                     barcode,
                                                                     PrintInterpretationLine.No));
        }
        else if (svgElement.ID == "RouteBc")
        {
          container.Body.Add(this.ZplCommands.BarCodeFieldDefaut(3,
                                                                 2,
                                                                 height));
          container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                           verticalStart));
          container.Body.Add(this.ZplCommands.Code128BarCode(fieldOrientation,
                                                             height,
                                                             barcode,
                                                             PrintInterpretationLine.No));
        }
        else if (svgElement.ID == "ReceiverBc")
        {
          container.Body.Add(this.ZplCommands.BarCodeFieldDefaut(2,
                                                                 2.0m,
                                                                 height));
          container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                           verticalStart));
          container.Body.Add(this.ZplCommands.Code128BarCode(fieldOrientation,
                                                             height,
                                                             barcode,
                                                             PrintInterpretationLine.No));
        }
        else
        {
          throw new NotImplementedException();
        }
      }
      else
      {
        base.AddTranslationToContainer(svgElement,
                                       sourceMatrix,
                                       viewMatrix,
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