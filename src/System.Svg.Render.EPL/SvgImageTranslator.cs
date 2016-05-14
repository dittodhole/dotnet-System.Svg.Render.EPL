using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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

      IEnumerable<byte> result;

      string variableName;
      if (this.IdToVariableNameMap.TryGetValue(instance.ID,
                                               out variableName))
      {
        result = this.EplCommands.PrintGraphics(horizontalStart,
                                                verticalStart,
                                                variableName);
      }
      else
      {
        result = this.TranslateGeneric(instance,
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

    public override IEnumerable<byte> TranslateForStoring([NotNull] SvgImage instance,
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

      var variableName = Convert.ToBase64String(Guid.NewGuid()
                                                    .ToByteArray())
                                .Substring(0,
                                           8);

      // TODO evaluate the best option ... :smoking:
      /*
                         Guid.NewGuid()
                             .ToString("N")
                             .Substring(0,
                                        8);
      */

      this.IdToVariableNameMap[instance.ID] = variableName;

      var result = this.TranslateGeneric(instance,
                                         matrix,
                                         (int) sourceAlignmentWidth,
                                         (int) sourceAlignmentHeight,
                                         bitmap => this.EplCommands.StoreGraphics(bitmap,
                                                                                  variableName)
                                                       .ToArray());

      return result;
    }

    private IEnumerable<byte> TranslateGeneric([NotNull] SvgImage instance,
                                               [NotNull] Matrix matrix,
                                               int sourceAlignmentWidth,
                                               int sourceAlignmentHeight,
                                               [NotNull] Func<Bitmap, byte[]> translationFn)
    {
      using (var image = instance.GetImage() as Image)
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