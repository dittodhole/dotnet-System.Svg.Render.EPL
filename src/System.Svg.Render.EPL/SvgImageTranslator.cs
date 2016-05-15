using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgImageTranslator : SvgElementToInternalMemoryTranslator<SvgImage>
  {
    public SvgImageTranslator([NotNull] EplTransformer eplTransformer,
                              [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
      this.ImageIdentifierToVariableNameMap = new Dictionary<string, string>();
    }

    [NotNull]
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    [NotNull]
    private IDictionary<string, string> ImageIdentifierToVariableNameMap { get; }

    [NotNull]
    private string GetImageIdentifier([NotNull] SvgImage svgImage)
    {
      var result = string.Concat(svgImage.OwnerDocument.ID,
                                 "::",
                                 svgImage.ID);

      return result;
    }

    public override IEnumerable<byte> Translate([NotNull] SvgImage svgElement,
                                                [NotNull] Matrix matrix)
    {
      float startX;
      float startY;
      float sourceAlignmentWidth;
      float sourceAlignmentHeight;
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out sourceAlignmentWidth,
                                    out sourceAlignmentHeight);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;

      IEnumerable<byte> result;

      var imageIdentifier = this.GetImageIdentifier(svgElement);

      string variableName;
      if (this.AssumeStoredInInternalMemory)
      {
        variableName = this.GetVariableName(imageIdentifier);

        result = this.EplCommands.PrintGraphics(horizontalStart,
                                                verticalStart,
                                                variableName);
      }
      else if (this.ImageIdentifierToVariableNameMap.TryGetValue(imageIdentifier,
                                                                 out variableName))
      {
        result = this.EplCommands.PrintGraphics(horizontalStart,
                                                verticalStart,
                                                variableName);
      }
      else
      {
        result = this.TranslateGeneric(svgElement,
                                       matrix,
                                       (int) sourceAlignmentWidth,
                                       (int) sourceAlignmentHeight,
                                       bitmap => this.EplCommands.GraphicDirectWrite(bitmap,
                                                                                     horizontalStart,
                                                                                     verticalStart)
                                                     .ToArray());
      }

      return result;
    }

    private string GetVariableName([NotNull] string imageIdentifier)
    {
      // TODO this is magic
      // on purpose: the imageIdentifier should be hashed to 8 chars
      // long, and should always be the same for the same imageIdentifier
      // thus going for this pile of shit ...
      var variableName = Math.Abs(imageIdentifier.GetHashCode())
                             .ToString();
      if (variableName.Length > 8)
      {
        variableName = variableName.Substring(0,
                                              8);
      }

      return variableName;
    }

    public override IEnumerable<byte> TranslateForStoring([NotNull] SvgImage svgElement,
                                                          [NotNull] Matrix matrix)
    {
      float startX;
      float startY;
      float sourceAlignmentWidth;
      float sourceAlignmentHeight;
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out sourceAlignmentWidth,
                                    out sourceAlignmentHeight);

      var imageIdentifier = this.GetImageIdentifier(svgElement);

      var variableName = this.GetVariableName(imageIdentifier);

      this.ImageIdentifierToVariableNameMap[imageIdentifier] = variableName;

      var result = this.TranslateGeneric(svgElement,
                                         matrix,
                                         (int) sourceAlignmentWidth,
                                         (int) sourceAlignmentHeight,
                                         bitmap => this.EplCommands.StoreGraphics(bitmap,
                                                                                  variableName)
                                                       .ToArray());

      return result;
    }

    private IEnumerable<byte> TranslateGeneric([NotNull] SvgImage svgElement,
                                               [NotNull] Matrix matrix,
                                               int sourceAlignmentWidth,
                                               int sourceAlignmentHeight,
                                               [NotNull] Func<Bitmap, byte[]> translationFn)
    {
      using (var image = svgElement.GetImage() as Image)
      {
        if (image == null)
        {
          return null;
        }

        var rotationTranslation = this.EplTransformer.GetRotation(matrix);

        using (var bitmap = new Bitmap(image,
                                       sourceAlignmentWidth,
                                       sourceAlignmentHeight))
        {
          var rotateFlipType = (RotateFlipType) rotationTranslation;
          bitmap.RotateFlip(rotateFlipType);

          var result = translationFn.Invoke(bitmap);

          return result;
        }
      }
    }
  }
}