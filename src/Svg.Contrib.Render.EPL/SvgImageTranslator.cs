using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgElementTranslatorBase<SvgImage>
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

    protected void StoreGraphics([NotNull] SvgImage svgElement,
                                 [NotNull] Matrix matrix,
                                 float sourceAlignmentWidth,
                                 float sourceAlignmentHeight,
                                 int horizontalStart,
                                 int verticalStart,
                                 [NotNull] EplContainer container,
                                 [CanBeNull] out string variableName)
    {
      var imageIdentifier = string.Concat(svgElement.OwnerDocument.ID,
                                          "::",
                                          svgElement.ID);

      if (!this.ImageIdentifierToVariableNameMap.TryGetValue(imageIdentifier,
                                                             out variableName))
      {
        variableName = this.CalculateVariableName(imageIdentifier);
        this.ImageIdentifierToVariableNameMap[imageIdentifier] = variableName;

        using (var bitmap = this.EplTransformer.ConvertToBitmap(svgElement,
                                                                matrix,
                                                                (int) sourceAlignmentWidth,
                                                                (int) sourceAlignmentHeight))
        {
          if (bitmap == null)
          {
            variableName = null;
            return;
          }

          var pcxByteArray = this.EplTransformer.ConvertToPcx(bitmap);

          container.Header.Add(this.EplCommands.DeleteGraphics(variableName));
          container.Header.Add(this.EplCommands.DeleteGraphics(variableName));
          container.Header.Add(this.EplCommands.StoreGraphics(variableName,
                                                              pcxByteArray.Length));
          container.Header.Add(pcxByteArray);
        }
      }
    }

    public override void Translate([NotNull] SvgImage svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] EplContainer container)

    {
      float startX;
      float startY;
      float endX;
      float endY;
      float sourceAlignmentWidth;
      float sourceAlignmentHeight;
      int horizontalStart;
      int verticalStart;
      int sector;
      this.GetPosition(svgElement,
                       matrix,
                       out startX,
                       out startY,
                       out endX,
                       out endY,
                       out sourceAlignmentWidth,
                       out sourceAlignmentHeight,
                       out horizontalStart,
                       out verticalStart,
                       out sector);

      this.AddTranslationToContainer(svgElement,
                                     matrix,
                                     sourceAlignmentWidth,
                                     sourceAlignmentHeight,
                                     horizontalStart,
                                     verticalStart,
                                     sector,
                                     container);
    }

    [Pure]
    protected virtual void GetPosition([NotNull] SvgImage svgElement,
                                       [NotNull] Matrix matrix,
                                       out float startX,
                                       out float startY,
                                       out float endX,
                                       out float endY,
                                       out float sourceAlignmentWidth,
                                       out float sourceAlignmentHeight,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int sector)
    {
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out sourceAlignmentWidth,
                                    out sourceAlignmentHeight);

      horizontalStart = (int) startX;
      verticalStart = (int) startY;
      sector = this.EplTransformer.GetRotationSector(matrix);
    }

    protected virtual void AddTranslationToContainer([NotNull] SvgImage svgElement,
                                                     [NotNull] Matrix matrix,
                                                     float sourceAlignmentWidth,
                                                     float sourceAlignmentHeight,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     int sector,
                                                     [NotNull] EplContainer container)
    {
      var forceDirectWrite = this.ForceDirectWrite(svgElement);
      if (forceDirectWrite)
      {
        this.GraphicDirectWrite(svgElement,
                                matrix,
                                sourceAlignmentWidth,
                                sourceAlignmentHeight,
                                horizontalStart,
                                verticalStart,
                                container);
      }
      else
      {
        string variableName;
        this.StoreGraphics(svgElement,
                           matrix,
                           sourceAlignmentWidth,
                           sourceAlignmentHeight,
                           horizontalStart,
                           verticalStart,
                           container,
                           out variableName);
        if (variableName != null)
        {
          this.PrintGraphics(horizontalStart,
                             verticalStart,
                             variableName,
                             container);
        }
      }
    }

    protected virtual void GraphicDirectWrite([NotNull] SvgImage svgElement,
                                              [NotNull] Matrix matrix,
                                              float sourceAlignmentWidth,
                                              float sourceAlignmentHeight,
                                              int horizontalStart,
                                              int verticalStart,
                                              [NotNull] EplContainer container)
    {
      using (var bitmap = this.EplTransformer.ConvertToBitmap(svgElement,
                                                              matrix,
                                                              (int) sourceAlignmentWidth,
                                                              (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        int numberOfBytesPerRow;
        var rawBinaryData = this.EplTransformer.GetRawBinaryData(bitmap,
                                                                 true,
                                                                 out numberOfBytesPerRow);
        var rows = bitmap.Height;

        container.Body.Add(this.EplCommands.GraphicDirectWrite(horizontalStart,
                                                               verticalStart,
                                                               numberOfBytesPerRow,
                                                               rows));
        container.Body.Add(rawBinaryData);
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual string CalculateVariableName([NotNull] string imageIdentifier)
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

    protected virtual void PrintGraphics(int horizontalStart,
                                         int verticalStart,
                                         [NotNull] string variableName,
                                         [NotNull] EplContainer container)
    {
      container.Body.Add(this.EplCommands.PrintGraphics(horizontalStart,
                                                        verticalStart,
                                                        variableName));
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual bool ForceDirectWrite([NotNull] SvgImage svgImage) => false;
  }
}