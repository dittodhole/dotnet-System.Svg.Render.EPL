using System;
using System.IO;
using System.Linq;
using System.Reflection;

// namespace
namespace Svg.Contrib.Render.FingerPrint.Demo
{
  // class definition
  public partial class label
  {
    /// <exception cref="FileNotFoundException">The embedded SVG resource "label.svg" could not be found.</exception>
    public label()
    {
      var svgFileName = "label.svg";
      var manifestResourceName = Assembly.GetExecutingAssembly()
                                         .GetManifestResourceNames()
                                         .FirstOrDefault(arg => arg.EndsWith(svgFileName,
                                                                             StringComparison.InvariantCultureIgnoreCase));
      if (manifestResourceName == null)
      {
        throw new FileNotFoundException("Could not find embedded SVG resource",
                                       svgFileName);
      }

      using (var manifestResourceStream = Assembly.GetExecutingAssembly()
                                                  .GetManifestResourceStream(manifestResourceName))
      {
        var svgDocument = SvgDocument.Open<SvgDocument>(manifestResourceStream);
        this.SvgDocument = svgDocument;
      }
    }

    public SvgDocument SvgDocument { get; }

    protected virtual string GetText(string id)
    {
      var svgTextBase = this.SvgDocument.GetElementById<SvgTextBase>(id);
      var text = svgTextBase.Text;

      return text;
    }

    protected virtual void SetText(string id,
                                   string text)
    {
      var svgTextBase = this.SvgDocument.GetElementById<SvgTextBase>(id);
      svgTextBase.Text = text;
    }

    protected virtual bool GetVisible(string id)
    {
      var svgVisualElement = this.SvgDocument.GetElementById<SvgVisualElement>(id);
      var visible = svgVisualElement.Visible;

      return visible;
    }

    protected virtual void SetVisible(string id,
                                      bool visible)
    {
      var svgVisualElement = this.SvgDocument.GetElementById<SvgVisualElement>(id);
      svgVisualElement.Visible = visible;
    }

    protected virtual string GetBarcode(string id)
    {
      var svgElement = this.SvgDocument.GetElementById(id);
      var barcode = svgElement.CustomAttributes["data-barcode"];

      return barcode;
    }

    protected virtual void SetBarcode(string id,
                                      string barcode)
    {
      var svgElement = this.SvgDocument.GetElementById(id);
      svgElement.CustomAttributes["data-barcode"] = barcode;
    }

    public virtual bool Visible_PredictImg
    {
      get
      {
        return this.GetVisible("PredictImg");
      }
      set
      {
        this.SetVisible("PredictImg", value);
      }
    }

    public virtual bool Visible_image3108
    {
      get
      {
        return this.GetVisible("image3108");
      }
      set
      {
        this.SetVisible("image3108", value);
      }
    }

    public virtual string LabelInfo
    {
      get
      {
        return this.GetText("LabelInfo");
      }
      set
      {
        this.SetText("LabelInfo", value);
      }
    }

    public virtual bool Visible_LabelInfo
    {
      get
      {
        return this.GetVisible("LabelInfo");
      }
      set
      {
        this.SetVisible("LabelInfo", value);
      }
    }

    public virtual string tspan3100
    {
      get
      {
        return this.GetText("tspan3100");
      }
      set
      {
        this.SetText("tspan3100", value);
      }
    }

    public virtual bool Visible_tspan3100
    {
      get
      {
        return this.GetVisible("tspan3100");
      }
      set
      {
        this.SetVisible("tspan3100", value);
      }
    }

    public virtual string vinfo
    {
      get
      {
        return this.GetText("vinfo");
      }
      set
      {
        this.SetText("vinfo", value);
      }
    }

    public virtual bool Visible_vinfo
    {
      get
      {
        return this.GetVisible("vinfo");
      }
      set
      {
        this.SetVisible("vinfo", value);
      }
    }

    public virtual string tspan3104
    {
      get
      {
        return this.GetText("tspan3104");
      }
      set
      {
        this.SetText("tspan3104", value);
      }
    }

    public virtual bool Visible_tspan3104
    {
      get
      {
        return this.GetVisible("tspan3104");
      }
      set
      {
        this.SetVisible("tspan3104", value);
      }
    }

    public virtual string tspan3099
    {
      get
      {
        return this.GetText("tspan3099");
      }
      set
      {
        this.SetText("tspan3099", value);
      }
    }

    public virtual bool Visible_tspan3099
    {
      get
      {
        return this.GetVisible("tspan3099");
      }
      set
      {
        this.SetVisible("tspan3099", value);
      }
    }

    public virtual string ReceiverName1
    {
      get
      {
        return this.GetText("ReceiverName1");
      }
      set
      {
        this.SetText("ReceiverName1", value);
      }
    }

    public virtual bool Visible_ReceiverName1
    {
      get
      {
        return this.GetVisible("ReceiverName1");
      }
      set
      {
        this.SetVisible("ReceiverName1", value);
      }
    }

    public virtual string tspan3111
    {
      get
      {
        return this.GetText("tspan3111");
      }
      set
      {
        this.SetText("tspan3111", value);
      }
    }

    public virtual bool Visible_tspan3111
    {
      get
      {
        return this.GetVisible("tspan3111");
      }
      set
      {
        this.SetVisible("tspan3111", value);
      }
    }

    public virtual string tspan3109
    {
      get
      {
        return this.GetText("tspan3109");
      }
      set
      {
        this.SetText("tspan3109", value);
      }
    }

    public virtual bool Visible_tspan3109
    {
      get
      {
        return this.GetVisible("tspan3109");
      }
      set
      {
        this.SetVisible("tspan3109", value);
      }
    }

    public virtual string tspan3107
    {
      get
      {
        return this.GetText("tspan3107");
      }
      set
      {
        this.SetText("tspan3107", value);
      }
    }

    public virtual bool Visible_tspan3107
    {
      get
      {
        return this.GetVisible("tspan3107");
      }
      set
      {
        this.SetVisible("tspan3107", value);
      }
    }

    public virtual string tspan3103
    {
      get
      {
        return this.GetText("tspan3103");
      }
      set
      {
        this.SetText("tspan3103", value);
      }
    }

    public virtual bool Visible_tspan3103
    {
      get
      {
        return this.GetVisible("tspan3103");
      }
      set
      {
        this.SetVisible("tspan3103", value);
      }
    }

    public virtual string tspan3105
    {
      get
      {
        return this.GetText("tspan3105");
      }
      set
      {
        this.SetText("tspan3105", value);
      }
    }

    public virtual bool Visible_tspan3105
    {
      get
      {
        return this.GetVisible("tspan3105");
      }
      set
      {
        this.SetVisible("tspan3105", value);
      }
    }

    public virtual string Barcode_ReceiverBc
    {
      get
      {
        return this.GetBarcode("ReceiverBc");
      }
      set
      {
        this.SetBarcode("ReceiverBc", value);
      }
    }

    public virtual bool Visible_ReceiverBc
    {
      get
      {
        return this.GetVisible("ReceiverBc");
      }
      set
      {
        this.SetVisible("ReceiverBc", value);
      }
    }

    public virtual string Barcode_RouteBc
    {
      get
      {
        return this.GetBarcode("RouteBc");
      }
      set
      {
        this.SetBarcode("RouteBc", value);
      }
    }

    public virtual bool Visible_RouteBc
    {
      get
      {
        return this.GetVisible("RouteBc");
      }
      set
      {
        this.SetVisible("RouteBc", value);
      }
    }

    public virtual string Barcode_CargoIdBc
    {
      get
      {
        return this.GetBarcode("CargoIdBc");
      }
      set
      {
        this.SetBarcode("CargoIdBc", value);
      }
    }

    public virtual bool Visible_CargoIdBc
    {
      get
      {
        return this.GetVisible("CargoIdBc");
      }
      set
      {
        this.SetVisible("CargoIdBc", value);
      }
    }

    public virtual string text3865_1
    {
      get
      {
        return this.GetText("text3865-1");
      }
      set
      {
        this.SetText("text3865-1", value);
      }
    }

    public virtual bool Visible_text3865_1
    {
      get
      {
        return this.GetVisible("text3865-1");
      }
      set
      {
        this.SetVisible("text3865-1", value);
      }
    }

    public virtual string tspan3867_7
    {
      get
      {
        return this.GetText("tspan3867-7");
      }
      set
      {
        this.SetText("tspan3867-7", value);
      }
    }

    public virtual bool Visible_tspan3867_7
    {
      get
      {
        return this.GetVisible("tspan3867-7");
      }
      set
      {
        this.SetVisible("tspan3867-7", value);
      }
    }

    public virtual string text3865
    {
      get
      {
        return this.GetText("text3865");
      }
      set
      {
        this.SetText("text3865", value);
      }
    }

    public virtual bool Visible_text3865
    {
      get
      {
        return this.GetVisible("text3865");
      }
      set
      {
        this.SetVisible("text3865", value);
      }
    }

    public virtual string tspan3867
    {
      get
      {
        return this.GetText("tspan3867");
      }
      set
      {
        this.SetText("tspan3867", value);
      }
    }

    public virtual bool Visible_tspan3867
    {
      get
      {
        return this.GetVisible("tspan3867");
      }
      set
      {
        this.SetVisible("tspan3867", value);
      }
    }

    public virtual string RefNr
    {
      get
      {
        return this.GetText("RefNr");
      }
      set
      {
        this.SetText("RefNr", value);
      }
    }

    public virtual bool Visible_RefNr
    {
      get
      {
        return this.GetVisible("RefNr");
      }
      set
      {
        this.SetVisible("RefNr", value);
      }
    }

    public virtual string tspan3128
    {
      get
      {
        return this.GetText("tspan3128");
      }
      set
      {
        this.SetText("tspan3128", value);
      }
    }

    public virtual bool Visible_tspan3128
    {
      get
      {
        return this.GetVisible("tspan3128");
      }
      set
      {
        this.SetVisible("tspan3128", value);
      }
    }

    public virtual string ShipperPlzCity
    {
      get
      {
        return this.GetText("ShipperPlzCity");
      }
      set
      {
        this.SetText("ShipperPlzCity", value);
      }
    }

    public virtual bool Visible_ShipperPlzCity
    {
      get
      {
        return this.GetVisible("ShipperPlzCity");
      }
      set
      {
        this.SetVisible("ShipperPlzCity", value);
      }
    }

    public virtual string tspan3118
    {
      get
      {
        return this.GetText("tspan3118");
      }
      set
      {
        this.SetText("tspan3118", value);
      }
    }

    public virtual bool Visible_tspan3118
    {
      get
      {
        return this.GetVisible("tspan3118");
      }
      set
      {
        this.SetVisible("tspan3118", value);
      }
    }

    public virtual string ShipperStreet
    {
      get
      {
        return this.GetText("ShipperStreet");
      }
      set
      {
        this.SetText("ShipperStreet", value);
      }
    }

    public virtual bool Visible_ShipperStreet
    {
      get
      {
        return this.GetVisible("ShipperStreet");
      }
      set
      {
        this.SetVisible("ShipperStreet", value);
      }
    }

    public virtual string tspan3114
    {
      get
      {
        return this.GetText("tspan3114");
      }
      set
      {
        this.SetText("tspan3114", value);
      }
    }

    public virtual bool Visible_tspan3114
    {
      get
      {
        return this.GetVisible("tspan3114");
      }
      set
      {
        this.SetVisible("tspan3114", value);
      }
    }

    public virtual string MultiColli
    {
      get
      {
        return this.GetText("MultiColli");
      }
      set
      {
        this.SetText("MultiColli", value);
      }
    }

    public virtual bool Visible_MultiColli
    {
      get
      {
        return this.GetVisible("MultiColli");
      }
      set
      {
        this.SetVisible("MultiColli", value);
      }
    }

    public virtual string tspan3106
    {
      get
      {
        return this.GetText("tspan3106");
      }
      set
      {
        this.SetText("tspan3106", value);
      }
    }

    public virtual bool Visible_tspan3106
    {
      get
      {
        return this.GetVisible("tspan3106");
      }
      set
      {
        this.SetVisible("tspan3106", value);
      }
    }

    public virtual string DepotName
    {
      get
      {
        return this.GetText("DepotName");
      }
      set
      {
        this.SetText("DepotName", value);
      }
    }

    public virtual bool Visible_DepotName
    {
      get
      {
        return this.GetVisible("DepotName");
      }
      set
      {
        this.SetVisible("DepotName", value);
      }
    }

    public virtual string tspan3455
    {
      get
      {
        return this.GetText("tspan3455");
      }
      set
      {
        this.SetText("tspan3455", value);
      }
    }

    public virtual bool Visible_tspan3455
    {
      get
      {
        return this.GetVisible("tspan3455");
      }
      set
      {
        this.SetVisible("tspan3455", value);
      }
    }

    public virtual string tspan3453
    {
      get
      {
        return this.GetText("tspan3453");
      }
      set
      {
        this.SetText("tspan3453", value);
      }
    }

    public virtual bool Visible_tspan3453
    {
      get
      {
        return this.GetVisible("tspan3453");
      }
      set
      {
        this.SetVisible("tspan3453", value);
      }
    }

    public virtual string tspan3451
    {
      get
      {
        return this.GetText("tspan3451");
      }
      set
      {
        this.SetText("tspan3451", value);
      }
    }

    public virtual bool Visible_tspan3451
    {
      get
      {
        return this.GetVisible("tspan3451");
      }
      set
      {
        this.SetVisible("tspan3451", value);
      }
    }

    public virtual string tspan3449
    {
      get
      {
        return this.GetText("tspan3449");
      }
      set
      {
        this.SetText("tspan3449", value);
      }
    }

    public virtual bool Visible_tspan3449
    {
      get
      {
        return this.GetVisible("tspan3449");
      }
      set
      {
        this.SetVisible("tspan3449", value);
      }
    }

    public virtual string tspan3447
    {
      get
      {
        return this.GetText("tspan3447");
      }
      set
      {
        this.SetText("tspan3447", value);
      }
    }

    public virtual bool Visible_tspan3447
    {
      get
      {
        return this.GetVisible("tspan3447");
      }
      set
      {
        this.SetVisible("tspan3447", value);
      }
    }

    public virtual string tspan3081
    {
      get
      {
        return this.GetText("tspan3081");
      }
      set
      {
        this.SetText("tspan3081", value);
      }
    }

    public virtual bool Visible_tspan3081
    {
      get
      {
        return this.GetVisible("tspan3081");
      }
      set
      {
        this.SetVisible("tspan3081", value);
      }
    }

    public virtual string text3133
    {
      get
      {
        return this.GetText("text3133");
      }
      set
      {
        this.SetText("text3133", value);
      }
    }

    public virtual bool Visible_text3133
    {
      get
      {
        return this.GetVisible("text3133");
      }
      set
      {
        this.SetVisible("text3133", value);
      }
    }

    public virtual string tspan3135
    {
      get
      {
        return this.GetText("tspan3135");
      }
      set
      {
        this.SetText("tspan3135", value);
      }
    }

    public virtual bool Visible_tspan3135
    {
      get
      {
        return this.GetVisible("tspan3135");
      }
      set
      {
        this.SetVisible("tspan3135", value);
      }
    }

    public virtual string ParcelNumber
    {
      get
      {
        return this.GetText("ParcelNumber");
      }
      set
      {
        this.SetText("ParcelNumber", value);
      }
    }

    public virtual bool Visible_ParcelNumber
    {
      get
      {
        return this.GetVisible("ParcelNumber");
      }
      set
      {
        this.SetVisible("ParcelNumber", value);
      }
    }

    public virtual string tspan3131
    {
      get
      {
        return this.GetText("tspan3131");
      }
      set
      {
        this.SetText("tspan3131", value);
      }
    }

    public virtual bool Visible_tspan3131
    {
      get
      {
        return this.GetVisible("tspan3131");
      }
      set
      {
        this.SetVisible("tspan3131", value);
      }
    }

    public virtual string text3125
    {
      get
      {
        return this.GetText("text3125");
      }
      set
      {
        this.SetText("text3125", value);
      }
    }

    public virtual bool Visible_text3125
    {
      get
      {
        return this.GetVisible("text3125");
      }
      set
      {
        this.SetVisible("text3125", value);
      }
    }

    public virtual string tspan3127
    {
      get
      {
        return this.GetText("tspan3127");
      }
      set
      {
        this.SetText("tspan3127", value);
      }
    }

    public virtual bool Visible_tspan3127
    {
      get
      {
        return this.GetVisible("tspan3127");
      }
      set
      {
        this.SetVisible("tspan3127", value);
      }
    }

    public virtual string CargoId
    {
      get
      {
        return this.GetText("CargoId");
      }
      set
      {
        this.SetText("CargoId", value);
      }
    }

    public virtual bool Visible_CargoId
    {
      get
      {
        return this.GetVisible("CargoId");
      }
      set
      {
        this.SetVisible("CargoId", value);
      }
    }

    public virtual string tspan3123
    {
      get
      {
        return this.GetText("tspan3123");
      }
      set
      {
        this.SetText("tspan3123", value);
      }
    }

    public virtual bool Visible_tspan3123
    {
      get
      {
        return this.GetVisible("tspan3123");
      }
      set
      {
        this.SetVisible("tspan3123", value);
      }
    }

    public virtual bool Visible_rect3028_2_1
    {
      get
      {
        return this.GetVisible("rect3028-2-1");
      }
      set
      {
        this.SetVisible("rect3028-2-1", value);
      }
    }

    public virtual string text3099
    {
      get
      {
        return this.GetText("text3099");
      }
      set
      {
        this.SetText("text3099", value);
      }
    }

    public virtual bool Visible_text3099
    {
      get
      {
        return this.GetVisible("text3099");
      }
      set
      {
        this.SetVisible("text3099", value);
      }
    }

    public virtual string tspan3101
    {
      get
      {
        return this.GetText("tspan3101");
      }
      set
      {
        this.SetText("tspan3101", value);
      }
    }

    public virtual bool Visible_tspan3101
    {
      get
      {
        return this.GetVisible("tspan3101");
      }
      set
      {
        this.SetVisible("tspan3101", value);
      }
    }

    public virtual string ShippingDate
    {
      get
      {
        return this.GetText("ShippingDate");
      }
      set
      {
        this.SetText("ShippingDate", value);
      }
    }

    public virtual bool Visible_ShippingDate
    {
      get
      {
        return this.GetVisible("ShippingDate");
      }
      set
      {
        this.SetVisible("ShippingDate", value);
      }
    }

    public virtual string tspan4665
    {
      get
      {
        return this.GetText("tspan4665");
      }
      set
      {
        this.SetText("tspan4665", value);
      }
    }

    public virtual bool Visible_tspan4665
    {
      get
      {
        return this.GetVisible("tspan4665");
      }
      set
      {
        this.SetVisible("tspan4665", value);
      }
    }

    public virtual string Weight
    {
      get
      {
        return this.GetText("Weight");
      }
      set
      {
        this.SetText("Weight", value);
      }
    }

    public virtual bool Visible_Weight
    {
      get
      {
        return this.GetVisible("Weight");
      }
      set
      {
        this.SetVisible("Weight", value);
      }
    }

    public virtual string tspan4661
    {
      get
      {
        return this.GetText("tspan4661");
      }
      set
      {
        this.SetText("tspan4661", value);
      }
    }

    public virtual bool Visible_tspan4661
    {
      get
      {
        return this.GetVisible("tspan4661");
      }
      set
      {
        this.SetVisible("tspan4661", value);
      }
    }

    public virtual string Plz
    {
      get
      {
        return this.GetText("Plz");
      }
      set
      {
        this.SetText("Plz", value);
      }
    }

    public virtual bool Visible_Plz
    {
      get
      {
        return this.GetVisible("Plz");
      }
      set
      {
        this.SetVisible("Plz", value);
      }
    }

    public virtual string tspan4657
    {
      get
      {
        return this.GetText("tspan4657");
      }
      set
      {
        this.SetText("tspan4657", value);
      }
    }

    public virtual bool Visible_tspan4657
    {
      get
      {
        return this.GetVisible("tspan4657");
      }
      set
      {
        this.SetVisible("tspan4657", value);
      }
    }

    public virtual string EmMandnr
    {
      get
      {
        return this.GetText("EmMandnr");
      }
      set
      {
        this.SetText("EmMandnr", value);
      }
    }

    public virtual bool Visible_EmMandnr
    {
      get
      {
        return this.GetVisible("EmMandnr");
      }
      set
      {
        this.SetVisible("EmMandnr", value);
      }
    }

    public virtual string tspan4645
    {
      get
      {
        return this.GetText("tspan4645");
      }
      set
      {
        this.SetText("tspan4645", value);
      }
    }

    public virtual bool Visible_tspan4645
    {
      get
      {
        return this.GetVisible("tspan4645");
      }
      set
      {
        this.SetVisible("tspan4645", value);
      }
    }

    public virtual bool Visible_path4523
    {
      get
      {
        return this.GetVisible("path4523");
      }
      set
      {
        this.SetVisible("path4523", value);
      }
    }

    public virtual bool Visible_rect5694
    {
      get
      {
        return this.GetVisible("rect5694");
      }
      set
      {
        this.SetVisible("rect5694", value);
      }
    }

    public virtual bool Visible_grRoute2
    {
      get
      {
        return this.GetVisible("grRoute2");
      }
      set
      {
        this.SetVisible("grRoute2", value);
      }
    }

    public virtual string Um2Mand
    {
      get
      {
        return this.GetText("Um2Mand");
      }
      set
      {
        this.SetText("Um2Mand", value);
      }
    }

    public virtual bool Visible_Um2Mand
    {
      get
      {
        return this.GetVisible("Um2Mand");
      }
      set
      {
        this.SetVisible("Um2Mand", value);
      }
    }

    public virtual string tspan4558_8
    {
      get
      {
        return this.GetText("tspan4558-8");
      }
      set
      {
        this.SetText("tspan4558-8", value);
      }
    }

    public virtual bool Visible_tspan4558_8
    {
      get
      {
        return this.GetVisible("tspan4558-8");
      }
      set
      {
        this.SetVisible("tspan4558-8", value);
      }
    }

    public virtual bool Visible_rect4554_1
    {
      get
      {
        return this.GetVisible("rect4554-1");
      }
      set
      {
        this.SetVisible("rect4554-1", value);
      }
    }

    public virtual bool Visible_SortBar2
    {
      get
      {
        return this.GetVisible("SortBar2");
      }
      set
      {
        this.SetVisible("SortBar2", value);
      }
    }

    public virtual bool Visible_SortBar
    {
      get
      {
        return this.GetVisible("SortBar");
      }
      set
      {
        this.SetVisible("SortBar", value);
      }
    }

    public virtual string Um2UmemRoutkrit2
    {
      get
      {
        return this.GetText("Um2UmemRoutkrit2");
      }
      set
      {
        this.SetText("Um2UmemRoutkrit2", value);
      }
    }

    public virtual bool Visible_Um2UmemRoutkrit2
    {
      get
      {
        return this.GetVisible("Um2UmemRoutkrit2");
      }
      set
      {
        this.SetVisible("Um2UmemRoutkrit2", value);
      }
    }

    public virtual string tspan4621
    {
      get
      {
        return this.GetText("tspan4621");
      }
      set
      {
        this.SetText("tspan4621", value);
      }
    }

    public virtual bool Visible_tspan4621
    {
      get
      {
        return this.GetVisible("tspan4621");
      }
      set
      {
        this.SetVisible("tspan4621", value);
      }
    }

    public virtual string Um1UmemRoutkrit2
    {
      get
      {
        return this.GetText("Um1UmemRoutkrit2");
      }
      set
      {
        this.SetText("Um1UmemRoutkrit2", value);
      }
    }

    public virtual bool Visible_Um1UmemRoutkrit2
    {
      get
      {
        return this.GetVisible("Um1UmemRoutkrit2");
      }
      set
      {
        this.SetVisible("Um1UmemRoutkrit2", value);
      }
    }

    public virtual string tspan4617
    {
      get
      {
        return this.GetText("tspan4617");
      }
      set
      {
        this.SetText("tspan4617", value);
      }
    }

    public virtual bool Visible_tspan4617
    {
      get
      {
        return this.GetVisible("tspan4617");
      }
      set
      {
        this.SetVisible("tspan4617", value);
      }
    }

    public virtual bool Visible_grRoute1
    {
      get
      {
        return this.GetVisible("grRoute1");
      }
      set
      {
        this.SetVisible("grRoute1", value);
      }
    }

    public virtual string Um1Mand
    {
      get
      {
        return this.GetText("Um1Mand");
      }
      set
      {
        this.SetText("Um1Mand", value);
      }
    }

    public virtual bool Visible_Um1Mand
    {
      get
      {
        return this.GetVisible("Um1Mand");
      }
      set
      {
        this.SetVisible("Um1Mand", value);
      }
    }

    public virtual string tspan4558
    {
      get
      {
        return this.GetText("tspan4558");
      }
      set
      {
        this.SetText("tspan4558", value);
      }
    }

    public virtual bool Visible_tspan4558
    {
      get
      {
        return this.GetVisible("tspan4558");
      }
      set
      {
        this.SetVisible("tspan4558", value);
      }
    }

    public virtual bool Visible_rect4554
    {
      get
      {
        return this.GetVisible("rect4554");
      }
      set
      {
        this.SetVisible("rect4554", value);
      }
    }

    public virtual string RlNote
    {
      get
      {
        return this.GetText("RlNote");
      }
      set
      {
        this.SetText("RlNote", value);
      }
    }

    public virtual bool Visible_RlNote
    {
      get
      {
        return this.GetVisible("RlNote");
      }
      set
      {
        this.SetVisible("RlNote", value);
      }
    }

    public virtual string tspan4552
    {
      get
      {
        return this.GetText("tspan4552");
      }
      set
      {
        this.SetText("tspan4552", value);
      }
    }

    public virtual bool Visible_tspan4552
    {
      get
      {
        return this.GetVisible("tspan4552");
      }
      set
      {
        this.SetVisible("tspan4552", value);
      }
    }

    public virtual string EmRouteNr
    {
      get
      {
        return this.GetText("EmRouteNr");
      }
      set
      {
        this.SetText("EmRouteNr", value);
      }
    }

    public virtual bool Visible_EmRouteNr
    {
      get
      {
        return this.GetVisible("EmRouteNr");
      }
      set
      {
        this.SetVisible("EmRouteNr", value);
      }
    }

    public virtual string tspan4548
    {
      get
      {
        return this.GetText("tspan4548");
      }
      set
      {
        this.SetText("tspan4548", value);
      }
    }

    public virtual bool Visible_tspan4548
    {
      get
      {
        return this.GetVisible("tspan4548");
      }
      set
      {
        this.SetVisible("tspan4548", value);
      }
    }

    public virtual string EmLine
    {
      get
      {
        return this.GetText("EmLine");
      }
      set
      {
        this.SetText("EmLine", value);
      }
    }

    public virtual bool Visible_EmLine
    {
      get
      {
        return this.GetVisible("EmLine");
      }
      set
      {
        this.SetVisible("EmLine", value);
      }
    }

    public virtual string tspan4544
    {
      get
      {
        return this.GetText("tspan4544");
      }
      set
      {
        this.SetText("tspan4544", value);
      }
    }

    public virtual bool Visible_tspan4544
    {
      get
      {
        return this.GetVisible("tspan4544");
      }
      set
      {
        this.SetVisible("tspan4544", value);
      }
    }

    public virtual string CargoId3
    {
      get
      {
        return this.GetText("CargoId3");
      }
      set
      {
        this.SetText("CargoId3", value);
      }
    }

    public virtual bool Visible_CargoId3
    {
      get
      {
        return this.GetVisible("CargoId3");
      }
      set
      {
        this.SetVisible("CargoId3", value);
      }
    }

    public virtual string tspan5686_2_3
    {
      get
      {
        return this.GetText("tspan5686-2-3");
      }
      set
      {
        this.SetText("tspan5686-2-3", value);
      }
    }

    public virtual bool Visible_tspan5686_2_3
    {
      get
      {
        return this.GetVisible("tspan5686-2-3");
      }
      set
      {
        this.SetVisible("tspan5686-2-3", value);
      }
    }

    public virtual string CargoId2
    {
      get
      {
        return this.GetText("CargoId2");
      }
      set
      {
        this.SetText("CargoId2", value);
      }
    }

    public virtual bool Visible_CargoId2
    {
      get
      {
        return this.GetVisible("CargoId2");
      }
      set
      {
        this.SetVisible("CargoId2", value);
      }
    }

    public virtual string tspan5686_2
    {
      get
      {
        return this.GetText("tspan5686-2");
      }
      set
      {
        this.SetText("tspan5686-2", value);
      }
    }

    public virtual bool Visible_tspan5686_2
    {
      get
      {
        return this.GetVisible("tspan5686-2");
      }
      set
      {
        this.SetVisible("tspan5686-2", value);
      }
    }

    public virtual string ShipperName
    {
      get
      {
        return this.GetText("ShipperName");
      }
      set
      {
        this.SetText("ShipperName", value);
      }
    }

    public virtual bool Visible_ShipperName
    {
      get
      {
        return this.GetVisible("ShipperName");
      }
      set
      {
        this.SetVisible("ShipperName", value);
      }
    }

    public virtual string tspan4276_5
    {
      get
      {
        return this.GetText("tspan4276-5");
      }
      set
      {
        this.SetText("tspan4276-5", value);
      }
    }

    public virtual bool Visible_tspan4276_5
    {
      get
      {
        return this.GetVisible("tspan4276-5");
      }
      set
      {
        this.SetVisible("tspan4276-5", value);
      }
    }

    public virtual string text4288_2
    {
      get
      {
        return this.GetText("text4288-2");
      }
      set
      {
        this.SetText("text4288-2", value);
      }
    }

    public virtual bool Visible_text4288_2
    {
      get
      {
        return this.GetVisible("text4288-2");
      }
      set
      {
        this.SetVisible("text4288-2", value);
      }
    }

    public virtual string tspan4290_2
    {
      get
      {
        return this.GetText("tspan4290-2");
      }
      set
      {
        this.SetText("tspan4290-2", value);
      }
    }

    public virtual bool Visible_tspan4290_2
    {
      get
      {
        return this.GetVisible("tspan4290-2");
      }
      set
      {
        this.SetVisible("tspan4290-2", value);
      }
    }

    public virtual string ReceiverCity
    {
      get
      {
        return this.GetText("ReceiverCity");
      }
      set
      {
        this.SetText("ReceiverCity", value);
      }
    }

    public virtual bool Visible_ReceiverCity
    {
      get
      {
        return this.GetVisible("ReceiverCity");
      }
      set
      {
        this.SetVisible("ReceiverCity", value);
      }
    }

    public virtual string tspan4306
    {
      get
      {
        return this.GetText("tspan4306");
      }
      set
      {
        this.SetText("tspan4306", value);
      }
    }

    public virtual bool Visible_tspan4306
    {
      get
      {
        return this.GetVisible("tspan4306");
      }
      set
      {
        this.SetVisible("tspan4306", value);
      }
    }

    public virtual string ReceiverZip
    {
      get
      {
        return this.GetText("ReceiverZip");
      }
      set
      {
        this.SetText("ReceiverZip", value);
      }
    }

    public virtual bool Visible_ReceiverZip
    {
      get
      {
        return this.GetVisible("ReceiverZip");
      }
      set
      {
        this.SetVisible("ReceiverZip", value);
      }
    }

    public virtual string tspan4302
    {
      get
      {
        return this.GetText("tspan4302");
      }
      set
      {
        this.SetText("tspan4302", value);
      }
    }

    public virtual bool Visible_tspan4302
    {
      get
      {
        return this.GetVisible("tspan4302");
      }
      set
      {
        this.SetVisible("tspan4302", value);
      }
    }

    public virtual string ReceiverStreet
    {
      get
      {
        return this.GetText("ReceiverStreet");
      }
      set
      {
        this.SetText("ReceiverStreet", value);
      }
    }

    public virtual bool Visible_ReceiverStreet
    {
      get
      {
        return this.GetVisible("ReceiverStreet");
      }
      set
      {
        this.SetVisible("ReceiverStreet", value);
      }
    }

    public virtual string tspan4298
    {
      get
      {
        return this.GetText("tspan4298");
      }
      set
      {
        this.SetText("tspan4298", value);
      }
    }

    public virtual bool Visible_tspan4298
    {
      get
      {
        return this.GetVisible("tspan4298");
      }
      set
      {
        this.SetVisible("tspan4298", value);
      }
    }

    public virtual string text4288
    {
      get
      {
        return this.GetText("text4288");
      }
      set
      {
        this.SetText("text4288", value);
      }
    }

    public virtual bool Visible_text4288
    {
      get
      {
        return this.GetVisible("text4288");
      }
      set
      {
        this.SetVisible("text4288", value);
      }
    }

    public virtual string tspan4290
    {
      get
      {
        return this.GetText("tspan4290");
      }
      set
      {
        this.SetText("tspan4290", value);
      }
    }

    public virtual bool Visible_tspan4290
    {
      get
      {
        return this.GetVisible("tspan4290");
      }
      set
      {
        this.SetVisible("tspan4290", value);
      }
    }

    public virtual bool Visible_rect3028_2
    {
      get
      {
        return this.GetVisible("rect3028-2");
      }
      set
      {
        this.SetVisible("rect3028-2", value);
      }
    }

    public virtual bool Visible_rect3028_5
    {
      get
      {
        return this.GetVisible("rect3028-5");
      }
      set
      {
        this.SetVisible("rect3028-5", value);
      }
    }

    public virtual bool Visible_rect3028
    {
      get
      {
        return this.GetVisible("rect3028");
      }
      set
      {
        this.SetVisible("rect3028", value);
      }
    }

    public virtual string AgbText
    {
      get
      {
        return this.GetText("AgbText");
      }
      set
      {
        this.SetText("AgbText", value);
      }
    }

    public virtual bool Visible_AgbText
    {
      get
      {
        return this.GetVisible("AgbText");
      }
      set
      {
        this.SetVisible("AgbText", value);
      }
    }

    public virtual string tspan5682
    {
      get
      {
        return this.GetText("tspan5682");
      }
      set
      {
        this.SetText("tspan5682", value);
      }
    }

    public virtual bool Visible_tspan5682
    {
      get
      {
        return this.GetVisible("tspan5682");
      }
      set
      {
        this.SetVisible("tspan5682", value);
      }
    }

    public virtual string tspan5676
    {
      get
      {
        return this.GetText("tspan5676");
      }
      set
      {
        this.SetText("tspan5676", value);
      }
    }

    public virtual bool Visible_tspan5676
    {
      get
      {
        return this.GetVisible("tspan5676");
      }
      set
      {
        this.SetVisible("tspan5676", value);
      }
    }

    public virtual string tspan5670
    {
      get
      {
        return this.GetText("tspan5670");
      }
      set
      {
        this.SetText("tspan5670", value);
      }
    }

    public virtual bool Visible_tspan5670
    {
      get
      {
        return this.GetVisible("tspan5670");
      }
      set
      {
        this.SetVisible("tspan5670", value);
      }
    }

    public virtual string tspan5668
    {
      get
      {
        return this.GetText("tspan5668");
      }
      set
      {
        this.SetText("tspan5668", value);
      }
    }

    public virtual bool Visible_tspan5668
    {
      get
      {
        return this.GetVisible("tspan5668");
      }
      set
      {
        this.SetVisible("tspan5668", value);
      }
    }
  } // class
} // namespace
