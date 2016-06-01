using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.EPL.Demo
{
  [PublicAPI]
  public class SvgImageTranslator : EPL.SvgImageTranslator
  {
    // Q: why the fuck are barcodes implemented this way?
    // A: well, barcodes do not exist in the svg-spec, so the
    //    SvgImage is hijacked for this by setting the value
    //    in "data-barcode"-attribute. additional positioning
    //    and barcode-mode is done rather in the code, than
    //    further misuse the attribute, which might not be a
    //    reusable abstraction for multiple printer languages.
    //    in short: yes! you have to get your hands dirty...

    public SvgImageTranslator([NotNull] EPL.EplTransformer eplTransformer,
                              [NotNull] EplCommands eplCommands)
      : base(eplTransformer,
             eplCommands) {}

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
        if (sector % 2 > 0)
        {
          horizontalStart += (int) sourceAlignmentHeight;
        }

        if (svgElement.ID == "CargoIdBc")
        {
          horizontalStart = horizontalStart - 30;
        }
        else if (svgElement.ID == "RouteBc")
        {
          verticalStart = verticalStart + 35;
        }
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
                                                      [NotNull] EplContainer container)
    {
      if (svgElement.HasNonEmptyCustomAttribute("data-barcode"))
      {
        var barcode = svgElement.CustomAttributes["data-barcode"];

        BarCodeSelection barCodeSelection;
        int narrowBarWidth;
        int wideBarWidth;
        PrintHumanReadable printHumanReadable;
        if (this.TryGetBarCodeSelection(svgElement,
                                        out barCodeSelection,
                                        out narrowBarWidth,
                                        out wideBarWidth,
                                        out printHumanReadable))
        {
          var height = (int) sourceAlignmentHeight;
          container.Body.Add(this.EplCommands.BarCode(horizontalStart,
                                                      verticalStart,
                                                      sector,
                                                      barCodeSelection,
                                                      narrowBarWidth,
                                                      wideBarWidth,
                                                      height,
                                                      printHumanReadable,
                                                      barcode));
          return;
        }
      }

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

    [Pure]
    [MustUseReturnValue]
    private bool TryGetBarCodeSelection([NotNull] SvgImage svgImage,
                                        out BarCodeSelection barCodeSelection,
                                        out int narrowBarWidth,
                                        out int wideBarWidth,
                                        out PrintHumanReadable printHumanReadable)
    {
      if (svgImage.ID == "CargoIdBc")
      {
        barCodeSelection = BarCodeSelection.Interleaved2Of5;
        narrowBarWidth = 4;
        wideBarWidth = 8;
        printHumanReadable = PrintHumanReadable.No;
        return true;
      }
      if (svgImage.ID == "RouteBc")
      {
        barCodeSelection = BarCodeSelection.Code128Auto;
        narrowBarWidth = 3;
        wideBarWidth = 4;
        printHumanReadable = PrintHumanReadable.No;
        return true;
      }
      if (svgImage.ID == "ReceiverBc")
      {
        barCodeSelection = BarCodeSelection.Code128Auto;
        narrowBarWidth = 1;
        wideBarWidth = 1;
        printHumanReadable = PrintHumanReadable.No;
        return true;
      }

      barCodeSelection = BarCodeSelection.Code128A;
      narrowBarWidth = 0;
      wideBarWidth = 0;
      printHumanReadable = PrintHumanReadable.No;
      return false;
    }
  }
}