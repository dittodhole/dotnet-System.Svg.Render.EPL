using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

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
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    [NotNull]
    private IDictionary<string, string> ImageIdentifierToVariableNameMap { get; }

    [NotNull]
    protected virtual string GetImageIdentifier([NotNull] SvgImage svgImage)
    {
      var result = string.Concat(svgImage.OwnerDocument.ID,
                                 "::",
                                 svgImage.ID);

      return result;
    }

    protected virtual void StoreVariableNameForImageIdentifier(string imageIdentifier,
                                                               string variableName)
    {
      this.ImageIdentifierToVariableNameMap[imageIdentifier] = variableName;
    }

    public override void Translate([NotNull] SvgImage svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] EplStream container)

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

      var imageIdentifier = this.GetImageIdentifier(svgElement);

      EplStream eplStream;

      string variableName;
      if (this.AssumeStoredInInternalMemory)
      {
        variableName = this.GetVariableName(imageIdentifier);

        eplStream = this.EplCommands.PrintGraphics(horizontalStart,
                                                   verticalStart,
                                                   variableName);
      }
      else if (this.ImageIdentifierToVariableNameMap.TryGetValue(imageIdentifier,
                                                                 out variableName))
      {
        eplStream = this.EplCommands.PrintGraphics(horizontalStart,
                                                   verticalStart,
                                                   variableName);
      }
      else
      {
        using (var bitmap = this.ConvertToBitmap(svgElement,
                                                 matrix,
                                                 (int) sourceAlignmentWidth,
                                                 (int) sourceAlignmentHeight))
        {
          if (bitmap == null)
          {
            return;
          }

          eplStream = this.EplCommands.GraphicDirectWrite(bitmap,
                                                          horizontalStart,
                                                          verticalStart);
        }
      }
      if (!eplStream.IsEmpty)
      {
        container.Add(eplStream);
      }
    }

    protected virtual string GetVariableName([NotNull] string imageIdentifier)
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

    public override void TranslateForStoring([NotNull] SvgImage svgElement,
                                             [NotNull] Matrix matrix,
                                             [NotNull] EplStream container)
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

      this.StoreVariableNameForImageIdentifier(imageIdentifier,
                                               variableName);

      EplStream eplStream;
      using (var bitmap = this.ConvertToBitmap(svgElement,
                                               matrix,
                                               (int) sourceAlignmentWidth,
                                               (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        eplStream = this.EplCommands.StoreGraphics(bitmap,
                                                   variableName);
      }

      if (!eplStream.IsEmpty)
      {
        container.Add(this.EplCommands.DeleteGraphics(variableName));
        container.Add(this.EplCommands.DeleteGraphics(variableName));
        container.Add(eplStream);
      }
    }

    protected virtual Bitmap ConvertToBitmap([NotNull] SvgImage svgElement,
                                             [NotNull] Matrix matrix,
                                             int sourceAlignmentWidth,
                                             int sourceAlignmentHeight)
    {
      using (var image = svgElement.GetImage() as Image)
      {
        if (image == null)
        {
          return null;
        }

        var rotationTranslation = this.EplTransformer.GetRotation(matrix);

        var bitmap = new Bitmap(image,
                                sourceAlignmentWidth,
                                sourceAlignmentHeight);
        var rotateFlipType = (RotateFlipType) rotationTranslation;
        bitmap.RotateFlip(rotateFlipType);

        return bitmap;
      }
    }
  }
}