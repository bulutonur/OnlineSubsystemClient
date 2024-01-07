using UnityEngine.Networking;

namespace OnlineSubsystem
{
    /// <summary>
    /// https://discussions.unity.com/t/curl-error-60-cert-verify-failed-unitytls_x509verify_flag_expired-ssl-ca-certificate-error/249557
    /// For each web request you make with an ssl server, you have to set the certificate handler that always validates the certificate.
    /// For example:
    /// var request = UnityWebRequest.Delete(API + aCommand);
    /// request.certificateHandler = new CertificateAcceptor();
    /// </summary>
    public class CertificateAcceptor : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
