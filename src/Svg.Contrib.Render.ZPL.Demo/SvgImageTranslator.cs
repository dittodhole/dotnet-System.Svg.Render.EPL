using System;
using System.Drawing.Drawing2D;
using Svg;
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

    public override void Translate([NotNull] SvgImage svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] Container<ZplStream> container)
    {
      if (svgElement.HasNonEmptyCustomAttribute("data-barcode"))
      {
        var barcode = svgElement.CustomAttributes["data-barcode"];

        float startX;
        float startY;
        float endX;
        float endY;
        float sourceAlignmentWidth;
        float sourceAlignmentHeight;
        this.ZplTransformer.Transform(svgElement,
                                      matrix,
                                      out startX,
                                      out startY,
                                      out endX,
                                      out endY,
                                      out sourceAlignmentWidth,
                                      out sourceAlignmentHeight);

        var horizontalStart = (int) endX;
        var verticalStart = (int) startY;
        var height = (int) sourceAlignmentHeight;
        var fieldOrientation = this.ZplTransformer.GetFieldOrientation(matrix);

        var sector = this.ZplTransformer.GetRotationSector(matrix);
        if (sector % 2 == 0)
        {
          verticalStart -= height;
        }

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
          verticalStart -= 30;
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
        base.Translate(svgElement,
                       matrix,
                       container);
      }
    }
  }
}