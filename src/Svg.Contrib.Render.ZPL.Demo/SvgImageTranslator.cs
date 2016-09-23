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

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands"/> is <see langword="null" />.</exception>
    public SvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                              [NotNull] ZplCommands zplCommands)
      : base(zplTransformer,
             zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
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
          verticalStart -= (int) sourceAlignmentHeight;
        }
        else
        {
          horizontalStart += (int) sourceAlignmentHeight;
        }
      }

      if (svgImage.ID == "RouteBc")
      {
        verticalStart -= 30;
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
                                                      [NotNull] ZplContainer container)
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
        var fieldOrientation = this.ZplTransformer.GetFieldOrientation(sourceMatrix,
                                                                       viewMatrix);

        if (svgImage.ID == "CargoIdBc")
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
        else if (svgImage.ID == "RouteBc")
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
        else if (svgImage.ID == "ReceiverBc")
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