using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    protected virtual SvgUnitReader CreateSvgUnitReader() => new SvgUnitReader();

    [NotNull]
    [Pure]
    public virtual FingerPrintTransformer CreateFingerPrintTransformer()
    {
      var svgUnitReader = this.CreateSvgUnitReader();
      var fingerPrintTransformer = this.CreateFingerPrintTransformer(svgUnitReader);

      return fingerPrintTransformer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual FingerPrintTransformer CreateFingerPrintTransformer([NotNull] SvgUnitReader svgUnitReader)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }

      var fingerPrintTransformer = new FingerPrintTransformer(svgUnitReader);

      return fingerPrintTransformer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual Matrix CreateViewMatrix([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                           float sourceDpi,
                                           float destinationDpi,
                                           ViewRotation viewRotation = ViewRotation.Normal)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }

      var magnificationFactor = destinationDpi / sourceDpi;

      var viewMatrix = fingerPrintTransformer.CreateViewMatrix(magnificationFactor,
                                                               viewRotation);

      return viewMatrix;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual FingerPrintRenderer CreateFingerPrintRenderer([NotNull] FingerPrintCommands fingerPrintCommands,
                                                                    CharacterSet characterSet = CharacterSet.Utf8)
    {
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var fingerPrintRenderer = new FingerPrintRenderer(fingerPrintCommands,
                                                        characterSet);

      return fingerPrintRenderer;
    }

    [NotNull]
    [Pure]
    protected virtual FingerPrintCommands CreateFingerPrintCommands() => new FingerPrintCommands();

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgLineTranslator = new SvgLineTranslator(fingerPrintTransformer,
                                                    fingerPrintCommands);

      return svgLineTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                          [NotNull] FingerPrintCommands fingerPrintCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }

      var svgRectangleTranslator = new SvgRectangleTranslator(fingerPrintTransformer,
                                                              fingerPrintCommands,
                                                              svgUnitReader);

      return svgRectangleTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                             [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgTextBaseTranslator = new SvgTextBaseTranslator<SvgText>(fingerPrintTransformer,
                                                                     fingerPrintCommands);

      return svgTextBaseTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                                     [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(fingerPrintTransformer,
                                                                         fingerPrintCommands);

      return svgTextSpanTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgPathTranslator = new SvgPathTranslator(fingerPrintTransformer,
                                                    fingerPrintCommands);

      return svgPathTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                  [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgImageTranslator = new SvgImageTranslator(fingerPrintTransformer,
                                                      fingerPrintCommands);

      return svgImageTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual FingerPrintRenderer CreateFingerPrintRenderer([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                 CharacterSet characterSet = CharacterSet.Utf8)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }

      var svgUnitReader = this.CreateSvgUnitReader();
      var fingerPrintCommands = this.CreateFingerPrintCommands();
      var fingerPrintRenderer = this.CreateFingerPrintRenderer(fingerPrintCommands,
                                                               characterSet);
      var svgLineTranslator = this.CreateSvgLineTranslator(fingerPrintTransformer,
                                                           fingerPrintCommands);
      var svgRectangleTranslator = this.CreateSvgRectangleTranslator(fingerPrintTransformer,
                                                                     fingerPrintCommands,
                                                                     svgUnitReader);
      var svgTextTranslator = this.CreateSvgTextTranslator(fingerPrintTransformer,
                                                           fingerPrintCommands);
      var svgTextSpanTranslator = this.CreateSvgTextSpanTranslator(fingerPrintTransformer,
                                                                   fingerPrintCommands);
      var svgPathTranslator = this.CreateSvgPathTranslator(fingerPrintTransformer,
                                                           fingerPrintCommands);
      var svgImageTranslator = this.CreateSvgImageTranslator(fingerPrintTransformer,
                                                             fingerPrintCommands);

      fingerPrintRenderer.RegisterTranslator(svgLineTranslator);
      fingerPrintRenderer.RegisterTranslator(svgRectangleTranslator);
      fingerPrintRenderer.RegisterTranslator(svgTextTranslator);
      fingerPrintRenderer.RegisterTranslator(svgTextSpanTranslator);
      fingerPrintRenderer.RegisterTranslator(svgPathTranslator);
      fingerPrintRenderer.RegisterTranslator(svgImageTranslator);

      return fingerPrintRenderer;
    }
  }
}
