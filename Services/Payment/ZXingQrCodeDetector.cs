namespace RetailECommerce.Services.Payment;

using System.Drawing;
using System.Runtime.Versioning;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

/// <summary>
/// ZXing.Net implementation of the QR detector. Decodes the uploaded image viaSystem.Drawing (Windows compatibility bindings) and reports whether a QR
/// code is present. The QR's contents don't matter for the mock payment but you need to use real tng and grab pay qr codes, dw it wont charge you.
/// </summary>
[SupportedOSPlatform("windows")]
public class ZXingQrCodeDetector : IQrCodeDetector
{
    public bool ContainsQrCode(Stream imageStream)
    {
        try
        {
            using var bitmap = (Bitmap)Image.FromStream(imageStream);

            var reader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
                    TryHarder = true
                }
            };

            return reader.Decode(bitmap) != null;
        }
        catch
        {
            // Not an image / corrupted upload - treat as "no QR code found".
            return false;
        }
    }
}
