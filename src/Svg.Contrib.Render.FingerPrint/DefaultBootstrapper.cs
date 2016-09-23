﻿using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberNeverOverriden.Global

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

    [NotNull]
    [Pure]
    protected virtual FingerPrintTransformer CreateFingerPrintTransformer([NotNull] SvgUnitReader svgUnitReader) => new FingerPrintTransformer(svgUnitReader);

    [NotNull]
    [Pure]
    public virtual Matrix CreateViewMatrix([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                           float sourceDpi,
                                           float destinationDpi,
                                           ViewRotation viewRotation = ViewRotation.Normal)
    {
      var magnificationFactor = destinationDpi / sourceDpi;

      var viewMatrix = fingerPrintTransformer.CreateViewMatrix(magnificationFactor,
                                                               viewRotation);

      return viewMatrix;
    }

    [NotNull]
    [Pure]
    protected virtual FingerPrintRenderer CreateFingerPrintRenderer([NotNull] FingerPrintCommands fingerPrintCommands) => new FingerPrintRenderer(fingerPrintCommands);

    [NotNull]
    [Pure]
    protected virtual FingerPrintCommands CreateFingerPrintCommands() => new FingerPrintCommands();

    [NotNull]
    [Pure]
    protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgLineTranslator(fingerPrintTransformer,
                                                                                                                                            fingerPrintCommands);

    [NotNull]
    [Pure]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                          [NotNull] FingerPrintCommands fingerPrintCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader) => new SvgRectangleTranslator(fingerPrintTransformer,
                                                                                                                                               fingerPrintCommands,
                                                                                                                                               svgUnitReader);

    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                             [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgText>(fingerPrintTransformer,
                                                                                                                                                                      fingerPrintCommands);

    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                                     [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgTextSpan>(fingerPrintTransformer,
                                                                                                                                                                                  fingerPrintCommands);

    [NotNull]
    [Pure]
    protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgPathTranslator(fingerPrintTransformer,
                                                                                                                                            fingerPrintCommands);

    [NotNull]
    [Pure]
    protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                  [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgImageTranslator(fingerPrintTransformer,
                                                                                                                                               fingerPrintCommands);

    [NotNull]
    [Pure]
    public virtual FingerPrintRenderer CreateFingerPrintRenderer([NotNull] FingerPrintTransformer fingerPrintTransformer)
    {
      var svgUnitReader = this.CreateSvgUnitReader();
      var fingerPrintCommands = this.CreateFingerPrintCommands();
      var fingerPrintRenderer = this.CreateFingerPrintRenderer(fingerPrintCommands);
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