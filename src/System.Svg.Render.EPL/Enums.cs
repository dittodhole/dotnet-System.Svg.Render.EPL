namespace System.Svg.Render.EPL
{
  public enum PrinterCodepage
  {
    Dos347,
    Dos850,
    Dos852,
    Dos860,
    Dos863,
    Dos865,
    Dos857,
    Dos861,
    Dos862,
    Dos855,
    Dos866,
    Dos737,
    Dos851,
    Dos869,
    Windows1252,
    Windows1250,
    Windows1251,
    Windows1253,
    Windows1254,
    Windows1255
  }

  public enum PrintOrientation
  {
    Top,
    Bottom
  }

  public enum BarCodeSelection
  {
    // TODO implement other barcode types
    Code39,
    Code39WithCheckDigit,
    Code93,
    Code128UCC,
    Code128Auto,
    Code128A,
    Code128B,
    Code128C,
    //Code128DeutschePost,
    //Codebar,
    //EAN8,
    //EAN82,
    //EAN85,
    //EAN13,
    //EAN132,
    //EAN135,
    //GermanPostCode,
    Interleaved2Of5,
    Interleaved2Of5WithMod10CheckDigit,
    Interleaved2Of5WithHumanReadableCheckDigit,
    //Postnet5And9And11And13Digit,
    //Planet11And13Digit,
    //JapanesePostnet,
    //EAN128,
    //UCC = BarCodeSelection.EAN128,
    //UPCA,
    //UPCA2,
    //UPCA5,
    //UPCE,
    //UPCE2,
    //UPCE5,
    //UPCInterleaved2Of5,
    //Plessey,
    //MSI3
  }

  public enum PrintHumanReadable
  {
    Yes,
    No
  }
}