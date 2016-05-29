using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgElementTranslatorBase<SvgImage>
  {
    public SvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                              [NotNull] ZplCommands zplCommands)
    {
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    [NotNull]
    [ItemNotNull]
    private IDictionary<string, string> ImageIdentifierToVariableNameMap { get; } = new Dictionary<string, string>();

    public override void Translate([NotNull] SvgImage svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] ZplContainer container)
    {
      float sourceAlignmentWidth;
      float sourceAlignmentHeight;
      int horizontalStart;
      int verticalStart;
      int sector;
      this.GetPosition(svgElement,
                       matrix,
                       out sourceAlignmentWidth,
                       out sourceAlignmentHeight,
                       out horizontalStart,
                       out verticalStart,
                       out sector);

      // TODO implement direct write!

      this.AddTranslationToContainer(svgElement,
                                     matrix,
                                     sourceAlignmentWidth,
                                     sourceAlignmentHeight,
                                     horizontalStart,
                                     verticalStart,
                                     sector,
                                     container);
    }

    protected virtual void GetPosition([NotNull] SvgImage svgElement,
                                       [NotNull] Matrix matrix,
                                       out float sourceAlignmentWidth,
                                       out float sourceAlignmentHeight,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int sector)
    {
      float startX;
      float startY;
      float endX;
      float endY;
      this.ZplTransformer.Transform(svgElement,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out sourceAlignmentWidth,
                                    out sourceAlignmentHeight);

      horizontalStart = (int) startX;
      verticalStart = (int) startY;
      sector = this.ZplTransformer.GetRotationSector(matrix);
    }

    protected virtual void AddTranslationToContainer([NotNull] SvgImage svgElement,
                                                     [NotNull] Matrix matrix,
                                                     float sourceAlignmentWidth,
                                                     float sourceAlignmentHeight,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     int sector,
                                                     [NotNull] ZplContainer container)
    {
      var forceDirectWrite = this.ForceDirectWrite(svgElement);
      if (forceDirectWrite)
      {
        this.GraphicField(svgElement,
                          matrix,
                          sourceAlignmentWidth,
                          sourceAlignmentHeight,
                          horizontalStart,
                          verticalStart,
                          container);
      }
      else
      {
        var variableName = this.StoreGraphics(svgElement,
                                              matrix,
                                              sourceAlignmentWidth,
                                              sourceAlignmentHeight,
                                              horizontalStart,
                                              verticalStart,
                                              container);
        if (variableName != null)
        {
          this.PrintGraphics(horizontalStart,
                             verticalStart,
                             variableName,
                             container);
        }
      }
    }

    protected virtual void GraphicField([NotNull] SvgImage svgElement,
                                        [NotNull] Matrix matrix,
                                        float sourceAlignmentWidth,
                                        float sourceAlignmentHeight,
                                        int horizontalStart,
                                        int verticalStart,
                                        [NotNull] ZplContainer container)
    {
      using (var bitmap = this.ZplTransformer.ConvertToBitmap(svgElement,
                                                              matrix,
                                                              (int) sourceAlignmentWidth,
                                                              (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        int numberOfBytesPerRow;
        var rawBinaryData = this.ZplTransformer.GetRawBinaryData(bitmap,
                                                                 false,
                                                                 out numberOfBytesPerRow);

        container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                         verticalStart));
        container.Body.Add(this.ZplCommands.GraphicField(rawBinaryData,
                                                         numberOfBytesPerRow));
      }
    }

    [CanBeNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual string StoreGraphics([NotNull] SvgImage svgElement,
                                           [NotNull] Matrix matrix,
                                           float sourceAlignmentWidth,
                                           float sourceAlignmentHeight,
                                           int horizontalStart,
                                           int verticalStart,
                                           [NotNull] ZplContainer container)
    {
      var imageIdentifier = string.Concat(svgElement.OwnerDocument.ID,
                                          "::",
                                          svgElement.ID);

      string variableName;
      if (!this.ImageIdentifierToVariableNameMap.TryGetValue(imageIdentifier,
                                                             out variableName))
      {
        variableName = this.CalculateVariableName(imageIdentifier);
        this.ImageIdentifierToVariableNameMap[imageIdentifier] = variableName;

        using (var bitmap = this.ZplTransformer.ConvertToBitmap(svgElement,
                                                                matrix,
                                                                (int) sourceAlignmentWidth,
                                                                (int) sourceAlignmentHeight))
        {
          if (bitmap == null)
          {
            return null;
          }

          int numberOfBytesPerRow;
          var rawBinaryData = this.ZplTransformer.GetRawBinaryData(bitmap,
                                                                   false,
                                                                   out numberOfBytesPerRow);

          container.Header.Add(this.ZplCommands.DownloadGraphics(variableName,
                                                                 rawBinaryData,
                                                                 numberOfBytesPerRow));
        }
      }

      return variableName;
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
                                         [NotNull] ZplContainer container)
    {
      container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                       verticalStart));
      container.Body.Add(this.ZplCommands.RecallGraphic(variableName));
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual bool ForceDirectWrite([NotNull] SvgImage svgImage) => false;
  }
}