﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgImageTranslator : SvgElementTranslatorBase<SvgImage>
  {
    public SvgImageTranslator([NotNull] EplTransformer eplTransformer,
                              [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
      this.IdToVariableNameMap = new Dictionary<string, string>();
    }

    [NotNull]
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    [NotNull]
    private IDictionary<string, string> IdToVariableNameMap { get; }

    public override IEnumerable<byte> Translate([NotNull] SvgImage instance,
                                                [NotNull] Matrix matrix)
    {
      float startX;
      float startY;
      float sourceAlignmentWidth;
      float sourceAlignmentHeight;
      this.EplTransformer.Transform(instance,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out sourceAlignmentWidth,
                                    out sourceAlignmentHeight);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;

      string variableName;
      if (this.IdToVariableNameMap.TryGetValue(instance.ID,
                                               out variableName))
      {
        var result = this.EplCommands.PrintGraphics(horizontalStart,
                                                    verticalStart,
                                                    variableName);

        return result;
      }

      using (var image = instance.GetImage() as Image)
      {
        if (image == null)
        {
          return null;
        }

        var rotationTranslation = this.EplTransformer.GetRotation(matrix);

        using (var bitmap = new Bitmap(image,
                                       (int) sourceAlignmentWidth,
                                       (int) sourceAlignmentHeight))
        {
          var rotateFlipType = (RotateFlipType) rotationTranslation;
          bitmap.RotateFlip(rotateFlipType);

          var result = this.EplCommands.GraphicDirectWrite(bitmap,
                                                           horizontalStart,
                                                           verticalStart)
                           .ToArray();

          return result;
        }
      }
    }
  }
}