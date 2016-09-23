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

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands"/> is <see langword="null" />.</exception>
    public SvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                              [NotNull] FingerPrintCommands fingerPrintCommands)
      : base(fingerPrintTransformer,
             fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    [Pure]
    protected override void GetPosition([NotNull] SvgImage svgImage,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        out float sourceAlignmentWidth,
                                        out float sourceAlignmentHeight,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out int sector)
    {
      if (svgImage == null)
      {
        throw new ArgumentNullException(nameof(svgImage));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      base.GetPosition(svgImage,
                       sourceMatrix,
                       viewMatrix,
                       out sourceAlignmentWidth,
                       out sourceAlignmentHeight,
                       out horizontalStart,
                       out verticalStart,
                       out sector);

      if (svgImage.HasNonEmptyCustomAttribute("data-barcode"))
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

        if (svgImage.ID == "RouteBc")
        {
          verticalStart -= 100;
        }
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void AddTranslationToContainer([NotNull] SvgImage svgImage,
                                                      [NotNull] Matrix sourceMatrix,
                                                      [NotNull] Matrix viewMatrix,
                                                      float sourceAlignmentWidth,
                                                      float sourceAlignmentHeight,
                                                      int horizontalStart,
                                                      int verticalStart,
                                                      int sector,
                                                      [NotNull] FingerPrintContainer container)
    {
      if (svgImage == null)
      {
        throw new ArgumentNullException(nameof(svgImage));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      if (svgImage.HasNonEmptyCustomAttribute("data-barcode"))
      {
        var barcode = svgImage.CustomAttributes["data-barcode"];
        var height = (int) sourceAlignmentHeight;
        var direction = this.FingerPrintTransformer.GetDirection(sourceMatrix,
                                                                 viewMatrix);

        container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                             verticalStart));
        container.Body.Add(this.FingerPrintCommands.Direction(direction));
        container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
        container.Body.Add(this.FingerPrintCommands.BarCodeHeight(height));

        if (svgImage.ID == "CargoIdBc")
        {
          container.Body.Add(this.FingerPrintCommands.BarCodeMagnify(3));
          container.Body.Add(this.FingerPrintCommands.BarCodeType(BarCodeType.Code128));
        }
        else if (svgImage.ID == "RouteBc")
        {
          container.Body.Add(this.FingerPrintCommands.BarCodeMagnify(2));
          container.Body.Add(this.FingerPrintCommands.BarCodeType(BarCodeType.Code128));
        }
        else if (svgImage.ID == "ReceiverBc")
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
        base.AddTranslationToContainer(svgImage,
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