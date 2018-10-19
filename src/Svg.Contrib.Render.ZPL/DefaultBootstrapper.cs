using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    protected virtual SvgUnitReader CreateSvgUnitReader() => new SvgUnitReader();

    [NotNull]
    [Pure]
    public virtual ZplTransformer CreateZplTransformer()
    {
      var svgUnitReader = this.CreateSvgUnitReader();
      var zplTransformer = this.CreateZplTransformer(svgUnitReader);

      return zplTransformer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual ZplTransformer CreateZplTransformer([NotNull] SvgUnitReader svgUnitReader)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }

      var zplTransformer = new ZplTransformer(svgUnitReader);

      return zplTransformer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual Matrix CreateViewMatrix([NotNull] ZplTransformer zplTransformer,
                                           float sourceDpi,
                                           float destinationDpi,
                                           ViewRotation viewRotation = ViewRotation.Normal)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }

      var magnificationFactor = destinationDpi / sourceDpi;

      var viewMatrix = zplTransformer.CreateViewMatrix(magnificationFactor,
                                                       viewRotation);

      return viewMatrix;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual ZplRenderer CreateZplRenderer([NotNull] ZplCommands zplCommands,
                                                    CharacterSet characterSet = CharacterSet.Utf8)
    {
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }

      var zplRenderer = new ZplRenderer(zplCommands,
                                        characterSet);

      return zplRenderer;
    }

    [NotNull]
    [Pure]
    protected virtual ZplCommands CreateZplCommands() => new ZplCommands();

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] ZplTransformer zplTransformer,
                                                                [NotNull] ZplCommands zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }

      var svgLineTranslator = new SvgLineTranslator(zplTransformer,
                                                    zplCommands);

      return svgLineTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] ZplTransformer zplTransformer,
                                                                          [NotNull] ZplCommands zplCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }

      var svgRectangleTranslator = new SvgRectangleTranslator(zplTransformer,
                                                              zplCommands,
                                                              svgUnitReader);

      return svgRectangleTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] ZplTransformer zplTransformer,
                                                                             [NotNull] ZplCommands zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }

      var svgTextBaseTranslator = new SvgTextBaseTranslator<SvgText>(zplTransformer,
                                                                     zplCommands);

      return svgTextBaseTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] ZplTransformer zplTransformer,
                                                                                     [NotNull] ZplCommands zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }

      var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(zplTransformer,
                                                                         zplCommands);

      return svgTextSpanTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] ZplTransformer zplTransformer,
                                                                [NotNull] ZplCommands zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }

      var svgPathTranslator = new SvgPathTranslator(zplTransformer,
                                                    zplCommands);

      return svgPathTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                                                                  [NotNull] ZplCommands zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }

      var svgImageTranslator = new SvgImageTranslator(zplTransformer,
                                                      zplCommands);

      return svgImageTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual ZplRenderer CreateZplRenderer([NotNull] ZplTransformer zplTransformer,
                                                 CharacterSet characterSet = CharacterSet.Utf8)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }

      var svgUnitReader = this.CreateSvgUnitReader();
      var zplCommands = this.CreateZplCommands();
      var zplRenderer = this.CreateZplRenderer(zplCommands,
                                               characterSet);
      var svgLineTranslator = this.CreateSvgLineTranslator(zplTransformer,
                                                           zplCommands);
      var svgRectangleTranslator = this.CreateSvgRectangleTranslator(zplTransformer,
                                                                     zplCommands,
                                                                     svgUnitReader);
      var svgTextTranslator = this.CreateSvgTextTranslator(zplTransformer,
                                                           zplCommands);
      var svgTextSpanTranslator = this.CreateSvgTextSpanTranslator(zplTransformer,
                                                                   zplCommands);
      var svgPathTranslator = this.CreateSvgPathTranslator(zplTransformer,
                                                           zplCommands);
      var svgImageTranslator = this.CreateSvgImageTranslator(zplTransformer,
                                                             zplCommands);

      zplRenderer.RegisterTranslator(svgLineTranslator);
      zplRenderer.RegisterTranslator(svgRectangleTranslator);
      zplRenderer.RegisterTranslator(svgTextTranslator);
      zplRenderer.RegisterTranslator(svgTextSpanTranslator);
      zplRenderer.RegisterTranslator(svgPathTranslator);
      zplRenderer.RegisterTranslator(svgImageTranslator);

      return zplRenderer;
    }
  }
}
