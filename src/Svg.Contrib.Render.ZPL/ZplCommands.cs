using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class ZplCommands
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplStream CreateZplStream() => new ZplStream();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string FieldOrigin(int horizontalStart,
                                      int verticalStart)
    {
      return $"^FO{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string FieldTypeset(int horizontalStart,
                                       int verticalStart)
    {
      return $"^FT{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string GraphicBox(int width,
                                     int height,
                                     int thickness,
                                     LineColor lineColor)
    {
      return $"^GB{width},{height},{thickness},{(char) lineColor}^FS";
    }

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //public virtual string GraphicDiagonalLine(int width,
    //                                          int height,
    //                                          int thickness,
    //                                          LineColor lineColor,
    //                                          Orientation orientation)
    //{
    //  return $"^GD{width},{height},{thickness},{(char) lineColor},{(char) orientation}^FS";
    //}

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Font([NotNull] string fontName,
                               FieldOrientation fieldOrientation,
                               int characterHeight,
                               int width,
                               string text)
    {
      return $"^A{fontName}{(char) fieldOrientation},{characterHeight},{width}^FD{text}^FS";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string StartFormat()
    {
      return "^XA";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string EndFormat()
    {
      return "^XZ";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintOrientation(PrintOrientation printOrientation)
    {
      return $"^PO{(char) printOrientation}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string LabelHome(int horizontalStart,
                                    int verticalStart)
    {
      return $"^LH{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string ChangeInternationalFont(CharacterSet characterSet)
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      return $"^CI{characterSet.ToString("D")}";
      // ReSharper restore ExceptionNotDocumentedOptional
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string DownloadGraphics([NotNull] string name,
                                           [NotNull] IEnumerable<byte> rawBinaryData,
                                           int numberOfBytesPerRow)
    {
      var binaryData = rawBinaryData.ToArray();
      var totalNumberOfBytes = binaryData.Count();
      var data = BitConverter.ToString(binaryData.ToArray())
                             .Replace("-",
                                      string.Empty);

      return $@"~DGR:{name},{totalNumberOfBytes},{numberOfBytesPerRow},{data}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string RecallGraphic([NotNull] string name)
    {
      return $"^XGR:{name},1,1^FS";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string BarCodeFieldDefaut(int moduleWidth,
                                             decimal wideBarToNarrowBarWidthRatio,
                                             int height)
    {
      return $"^BY{moduleWidth},{Math.Round(wideBarToNarrowBarWidthRatio, 2)},{height}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Code128BarCode(FieldOrientation fieldOrientation,
                                         int barCodeHeight,
                                         [NotNull] string content,
                                         PrintInterpretationLine printInterpretationLine = PrintInterpretationLine.Yes,
                                         PrintInterpretationLineAboveCode printInterpretationLineAboveCode = PrintInterpretationLineAboveCode.No,
                                         UccCheckDigit uccCheckDigit = UccCheckDigit.No,
                                         Mode mode = Mode.NoSelectedMode)
    {
      return $"^BC{(char) fieldOrientation},{barCodeHeight},{(char) printInterpretationLine},{(char) printInterpretationLineAboveCode},{(char) uccCheckDigit},{(char) mode}^FD{content}^FS";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Interleaved2Of5BarCode(FieldOrientation fieldOrientation,
                                                 int barCodeHeight,
                                                 [NotNull] string content,
                                                 PrintInterpretationLine printInterpretationLine = PrintInterpretationLine.Yes,
                                                 PrintInterpretationLineAboveCode printInterpretationLineAboveCode = PrintInterpretationLineAboveCode.No,
                                                 CalculateAndPrintMod10CheckDigit calculateAndPrintMod10CheckDigit = CalculateAndPrintMod10CheckDigit.No)
    {
      return $"^B2{(char) fieldOrientation},{barCodeHeight},{(char) printInterpretationLine},{(char) printInterpretationLineAboveCode},{(char) calculateAndPrintMod10CheckDigit}^FD{content}^FS";
    }
  }
}