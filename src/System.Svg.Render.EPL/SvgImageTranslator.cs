using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.EPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgElementToInternalMemoryTranslator<SvgImage>
  {
    public SvgImageTranslator([NotNull] EplTransformer eplTransformer,
                              [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    [NotNull]
    [ItemNotNull]
    private IDictionary<string, string> ImageIdentifierToVariableNameMap { get; } = new Dictionary<string, string>();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual string GetImageIdentifier([NotNull] SvgImage svgImage)
    {
      var result = string.Concat(svgImage.OwnerDocument.ID,
                                 "::",
                                 svgImage.ID);

      return result;
    }

    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    protected virtual void StoreVariableNameForImageIdentifier([NotNull] string imageIdentifier,
                                                               [NotNull] string variableName)
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      this.ImageIdentifierToVariableNameMap[imageIdentifier] = variableName;
      // ReSharper restore ExceptionNotDocumentedOptional
    }

    public override void Translate([NotNull] SvgImage svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] EplStream container)

    {
      float startX;
      float startY;
      float endX;
      float endY;
      float sourceAlignmentWidth;
      float sourceAlignmentHeight;
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out sourceAlignmentWidth,
                                    out sourceAlignmentHeight);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;

      var imageIdentifier = this.GetImageIdentifier(svgElement);
      var forceDirectWrite = this.ForceDirectWrite(svgElement);

      string variableName;
      if (!forceDirectWrite
          && this.AssumeStoredInInternalMemory)
      {
        variableName = this.GetVariableName(imageIdentifier);

        container.Add(this.EplCommands.PrintGraphics(horizontalStart,
                                                     verticalStart,
                                                     variableName));
      }
      // ReSharper disable ExceptionNotDocumentedOptional
      else if (!forceDirectWrite
               && this.ImageIdentifierToVariableNameMap.TryGetValue(imageIdentifier,
                                                                    out variableName))
      // ReSharper restore ExceptionNotDocumentedOptional
      {
        container.Add(this.EplCommands.PrintGraphics(horizontalStart,
                                                     verticalStart,
                                                     variableName));
      }
      else
      {
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

          eplStream = this.EplCommands.GraphicDirectWrite(bitmap,
                                                          horizontalStart,
                                                          verticalStart);
        }
        // ReSharper disable ExceptionNotDocumentedOptional
        if (eplStream.Any())
        // ReSharper restore ExceptionNotDocumentedOptional
        {
          container.Add(eplStream);
        }
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
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
        // ReSharper disable ExceptionNotDocumentedOptional
        variableName = variableName.Substring(0,
                                              8);
        // ReSharper restore ExceptionNotDocumentedOptional
      }

      return variableName;
    }

    public override void TranslateForStoring([NotNull] SvgImage svgElement,
                                             [NotNull] Matrix matrix,
                                             [NotNull] EplStream container)
    {
      if (this.ForceDirectWrite(svgElement))
      {
        return;
      }

      float startX;
      float startY;
      float endX;
      float endY;
      float sourceAlignmentWidth;
      float sourceAlignmentHeight;
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
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
      // ReSharper disable ExceptionNotDocumentedOptional
      if (eplStream.Any())
      // ReSharper restore ExceptionNotDocumentedOptional
      {
        container.Add(this.EplCommands.DeleteGraphics(variableName));
        container.Add(this.EplCommands.DeleteGraphics(variableName));
        container.Add(eplStream);
      }
    }

    [CanBeNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Bitmap ConvertToBitmap([NotNull] SvgImage svgElement,
                                             [NotNull] Matrix matrix,
                                             int sourceAlignmentWidth,
                                             int sourceAlignmentHeight)
    {
      var stretchImage = this.StretchImage(svgElement);

      using (var image = svgElement.GetImage() as Image)
      {
        if (image == null)
        {
          return null;
        }

        var rotationTranslation = this.EplTransformer.GetRotation(matrix);

        Bitmap bitmap;
        if (stretchImage)
        {
          bitmap = new Bitmap(image,
                              sourceAlignmentWidth,
                              sourceAlignmentHeight);
        }
        else
        {
          var sourceRatio = (float) sourceAlignmentWidth / sourceAlignmentHeight;
          var destinationRatio = (float) image.Width / image.Height;

          // TODO find a good TOLERANCE
          if (Math.Abs(sourceRatio - destinationRatio) < 0.5f)
          {
            bitmap = new Bitmap(image,
                                sourceAlignmentWidth,
                                sourceAlignmentHeight);
          }
          else
          {
            int destinationWidth;
            int destinationHeight;

            if (sourceRatio < destinationRatio)
            {
              destinationWidth = sourceAlignmentWidth;
              destinationHeight = (int) (sourceAlignmentWidth / destinationRatio);
            }
            else
            {
              destinationWidth = (int) (sourceAlignmentHeight * destinationRatio);
              destinationHeight = sourceAlignmentHeight;
            }

            var x = (sourceAlignmentWidth - destinationWidth) / 2;
            var y = (sourceAlignmentHeight - destinationHeight) / 2;

            bitmap = new Bitmap(sourceAlignmentWidth,
                                sourceAlignmentHeight);
            using (var graphics = Graphics.FromImage(bitmap))
            {
              var rect = new Rectangle(x,
                                       y,
                                       destinationWidth,
                                       destinationHeight);
              graphics.DrawImage(image,
                                 rect);
            }
          }
        }

        var rotateFlipType = (RotateFlipType) rotationTranslation;
        bitmap.RotateFlip(rotateFlipType);

        return bitmap;
      }
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual bool ForceDirectWrite([NotNull] SvgImage svgImage)
    {
      return false;
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual bool StretchImage([NotNull] SvgImage svgImage)
    {
      return false;
    }
  }
}