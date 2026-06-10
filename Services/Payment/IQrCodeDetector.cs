namespace RetailECommerce.Services.Payment;

/// <summary>
/// Boolean QR detector used by the mock QR payment flow the websiteuploads a paymentproof image, and the order proceeds only if a QR code actually
/// exists in that image, you need to use real tng and grabpay, dw it wont charge you.
/// </summary>
public interface IQrCodeDetector
{
    /// <summary>
    /// Returns true if the given image contains a decodable QR code. Returns false for images without a QR code or unreadable files.
    /// </summary>
    bool ContainsQrCode(Stream imageStream);
}
