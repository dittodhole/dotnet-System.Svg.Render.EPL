using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class SvgImageTranslatorBase<TContainer> : SvgElementTranslatorBase<TContainer, SvgImage>
    where TContainer : Container
  {
    /// <exception cref="ArgumentNullException"><paramref name="genericTransformer" /> is <see langword="null" />.</exception>
    protected SvgImageTranslatorBase([NotNull] GenericTransformer genericTransformer)
    {
      this.GenericTransformer = genericTransformer ?? throw new ArgumentNullException(nameof(genericTransformer));
    }

    [NotNull]
    private GenericTransformer GenericTransformer { get; }

    [NotNull]
    private IDictionary<string, string> ImageIdentifierToVariableNameMap { get; } = new Dictionary<string, string>();

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    protected virtual void StoreGraphics([NotNull] SvgImage svgImage,
                                         [NotNull] Matrix sourceMatrix,
                                         [NotNull] Matrix viewMatrix,
                                         float sourceAlignmentWidth,
                                         float sourceAlignmentHeight,
                                         int horizontalStart,
                                         int verticalStart,
                                         [NotNull] TContainer container,
                                         [CanBeNull] out string variableName)
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

      var rotationSector = this.GenericTransformer.GetRotationSector(sourceMatrix,
                                                                     viewMatrix);

      var imageIdentifier = string.Concat(rotationSector,
                                          "::",
                                          svgImage.ID,
                                          "::",
                                          svgImage.OwnerDocument.ID);

      if (!this.ImageIdentifierToVariableNameMap.TryGetValue(imageIdentifier,
                                                             out variableName))
      {
        variableName = this.CalculateVariableName(imageIdentifier);
        this.ImageIdentifierToVariableNameMap[imageIdentifier] = variableName;

        var bitmap = this.GenericTransformer.ConvertToBitmap(svgImage,
                                                             sourceMatrix,
                                                             viewMatrix,
                                                             (int) sourceAlignmentWidth,
                                                             (int) sourceAlignmentHeight);
        if (bitmap == null)
        {
          variableName = null;
        }
        else
        {
          using (bitmap)
          {
            this.StoreGraphics(svgImage,
                               variableName,
                               bitmap,
                               container);
          }
        }
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variableName" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmap" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    protected abstract void StoreGraphics([NotNull] SvgImage svgImage,
                                          [NotNull] string variableName,
                                          [NotNull] Bitmap bitmap,
                                          [NotNull] TContainer container);

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    public override void Translate(SvgImage svgImage,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   TContainer container)

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

      this.GetPosition(svgImage,
                       sourceMatrix,
                       viewMatrix,
                       out var sourceAlignmentWidth,
                       out var sourceAlignmentHeight,
                       out var horizontalStart,
                       out var verticalStart,
                       out var sector);

      this.AddTranslationToContainer(svgImage,
                                     sourceMatrix,
                                     viewMatrix,
                                     sourceAlignmentWidth,
                                     sourceAlignmentHeight,
                                     horizontalStart,
                                     verticalStart,
                                     sector,
                                     container);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetPosition([NotNull] SvgImage svgImage,
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

      this.GenericTransformer.Transform(svgImage,
                                        sourceMatrix,
                                        viewMatrix,
                                        out var startX,
                                        out var startY,
                                        out var endX,
                                        out var endY,
                                        out sourceAlignmentWidth,
                                        out sourceAlignmentHeight);

      horizontalStart = (int) startX;
      verticalStart = (int) startY;
      sector = this.GenericTransformer.GetRotationSector(sourceMatrix,
                                                         viewMatrix);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    protected virtual void AddTranslationToContainer([NotNull] SvgImage svgImage,
                                                     [NotNull] Matrix sourceMatrix,
                                                     [NotNull] Matrix viewMatrix,
                                                     float sourceAlignmentWidth,
                                                     float sourceAlignmentHeight,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     int sector,
                                                     [NotNull] TContainer container)
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

      var forceDirectWrite = this.ForceDirectWrite(svgImage);
      if (forceDirectWrite)
      {
        this.GraphicDirectWrite(svgImage,
                                sourceMatrix,
                                viewMatrix,
                                sourceAlignmentWidth,
                                sourceAlignmentHeight,
                                horizontalStart,
                                verticalStart,
                                container);
      }
      else
      {
        this.StoreGraphics(svgImage,
                           sourceMatrix,
                           viewMatrix,
                           sourceAlignmentWidth,
                           sourceAlignmentHeight,
                           horizontalStart,
                           verticalStart,
                           container,
                           out var variableName);
        if (variableName != null)
        {
          this.PrintGraphics(svgImage,
                             sourceMatrix,
                             viewMatrix,
                             horizontalStart,
                             verticalStart,
                             sector,
                             variableName,
                             container);
        }
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    protected abstract void GraphicDirectWrite([NotNull] SvgImage svgImage,
                                               [NotNull] Matrix sourceMatrix,
                                               [NotNull] Matrix viewMatrix,
                                               float sourceAlignmentWidth,
                                               float sourceAlignmentHeight,
                                               int horizontalStart,
                                               int verticalStart,
                                               [NotNull] TContainer container);

    /// <exception cref="ArgumentNullException"><paramref name="imageIdentifier" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual string CalculateVariableName([NotNull] string imageIdentifier)
    {
      if (imageIdentifier == null)
      {
        throw new ArgumentNullException(nameof(imageIdentifier));
      }

      string result;
      using (var md5 = MD5.Create())
      {
        var buffer = Encoding.UTF8.GetBytes(imageIdentifier);
        var hash = md5.ComputeHash(buffer);
        result = BitConverter.ToString(hash)
                             .Replace("-", string.Empty)
                             .ToUpperInvariant();
      }

      if (result.Length > 8)
      {
        result = result.Substring(0, 8);
      }

      return result;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variableName" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    protected abstract void PrintGraphics([NotNull] SvgImage svgImage,
                                          [NotNull] Matrix sourceMatrix,
                                          [NotNull] Matrix viewMatrix,
                                          int horizontalStart,
                                          int verticalStart,
                                          int sector,
                                          [NotNull] string variableName,
                                          [NotNull] TContainer container);

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    [Pure]
    protected virtual bool ForceDirectWrite([NotNull] SvgImage svgImage)
    {
      if (svgImage == null)
      {
        throw new ArgumentNullException(nameof(svgImage));
      }

      return false;
    }
  }
}
