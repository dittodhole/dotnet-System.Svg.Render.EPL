﻿using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class ZplRenderer : RendererBase<ZplContainer>
  {
    public ZplRenderer([NotNull] ZplCommands zplCommands,
                       CharacterSet characterSet = CharacterSet.ZebraCodePage850)
    {
      this.ZplCommands = zplCommands;
      this.CharacterSet = characterSet;
    }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    protected CharacterSet CharacterSet { get; }

    [NotNull]
    [ItemNotNull]
    private IDictionary<CharacterSet, int> CharacterSetMappings { get; } = new Dictionary<CharacterSet, int>
                                                                           {
                                                                             {
                                                                               CharacterSet.ZebraCodePage1252, 1252
                                                                             },
                                                                             {
                                                                               CharacterSet.ZebraCodePage850, 850
                                                                             }
                                                                           };

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual Encoding GetEncoding()
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      var codepage = this.CharacterSetMappings[this.CharacterSet];
      // ReSharper restore ExceptionNotDocumentedOptional
      // ReSharper disable ExceptionNotDocumentedOptional
      var encoding = Encoding.GetEncoding(codepage);
      // ReSharper restore ExceptionNotDocumentedOptional

      return encoding;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public override ZplContainer GetTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix viewMatrix)
    {
      var sourceMatrix = new Matrix();
      var zplContainer = new ZplContainer();
      this.AddBodyToTranslation(svgDocument,
                                sourceMatrix,
                                viewMatrix,
                                zplContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  zplContainer);
      this.AddFooterToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  zplContainer);

      return zplContainer;
    }

    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] ZplContainer container)
    {
      container.Header.Add(this.ZplCommands.ChangeInternationalFont(this.CharacterSet));
    }

    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix sourceMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                [NotNull] ZplContainer container)
    {
      container.Body.Add(this.ZplCommands.StartFormat());
      container.Body.Add(this.ZplCommands.LabelHome(18,
                                                    8));
      container.Body.Add(this.ZplCommands.PrintOrientation(PrintOrientation.Normal));
      this.TranslateSvgElementAndChildren(svgDocument,
                                          sourceMatrix,
                                          viewMatrix,
                                          container);
    }

    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] ZplContainer container)
    {
      container.Footer.Add(this.ZplCommands.EndFormat());
    }
  }
}