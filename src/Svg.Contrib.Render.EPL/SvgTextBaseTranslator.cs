using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<EplContainer, T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    public SvgTextBaseTranslator([NotNull] EplTransformer eplTransformer,
                                 [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] EplContainer container)
    {
      if (svgElement.Text == null)
      {
        return;
      }

      var text = this.RemoveIllegalCharacters(svgElement.Text);
      if (string.IsNullOrEmpty(text))
      {
        return;
      }

      float fontSize;
      int horizontalStart;
      int verticalStart;
      int sector;
      this.GetPosition(svgElement,
                       sourceMatrix,
                       viewMatrix,
                       out horizontalStart,
                       out verticalStart,
                       out sector,
                       out fontSize);

      int fontSelection;
      int horizontalMultiplier;
      int verticalMultiplier;
      this.EplTransformer.GetFontSelection(svgElement,
                                           fontSize,
                                           out fontSelection,
                                           out horizontalMultiplier,
                                           out verticalMultiplier);

      this.AddTranslationToContainer(svgElement,
                                     horizontalStart,
                                     verticalStart,
                                     sector,
                                     fontSelection,
                                     horizontalMultiplier,
                                     verticalMultiplier,
                                     text,
                                     container);
    }

    [Pure]
    protected virtual void GetPosition([NotNull] T svgElement,
                                       [NotNull] Matrix sourceMatrix,
                                       [NotNull] Matrix viewMatrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int sector,
                                       out float fontSize)
    {
      float x;
      float y;
      this.EplTransformer.Transform(svgElement,
                                    sourceMatrix,
                                    viewMatrix,
                                    out x,
                                    out y,
                                    out fontSize);

      horizontalStart = (int) x;
      verticalStart = (int) y;
      sector = this.EplTransformer.GetRotationSector(sourceMatrix,
                                                     viewMatrix);
    }

    [NotNull]
    [Pure]
    protected virtual string RemoveIllegalCharacters([NotNull] string text)
    {
      // TODO add regex for removing illegal characters ...

      // ReSharper disable ExceptionNotDocumentedOptional
      return text.Replace("\"",
                          "'");
      // ReSharper restore ExceptionNotDocumentedOptional
    }

    protected virtual void AddTranslationToContainer([NotNull] T svgElement,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     int sector,
                                                     int fontSelection,
                                                     int horizontalMultiplier,
                                                     int verticalMultiplier,
                                                     [NotNull] string text,
                                                     [NotNull] EplContainer container)
    {
      ReverseImage reverseImage;
      if ((svgElement.Fill as SvgColourServer)?.Colour == Color.White)
      {
        reverseImage = ReverseImage.Reverse;
      }
      else
      {
        reverseImage = ReverseImage.Normal;
      }

      container.Body.Add(this.EplCommands.AsciiText(horizontalStart,
                                                    verticalStart,
                                                    sector,
                                                    fontSelection,
                                                    horizontalMultiplier,
                                                    verticalMultiplier,
                                                    reverseImage,
                                                    text));
    }
  }
}