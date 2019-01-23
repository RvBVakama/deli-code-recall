using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class SendEmail : MonoBehaviour
{
    public static void SendMail(string filename)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("unityemailcommunication@gmail.com");
        mail.To.Add("unityemailcommunication@gmail.com");
        mail.Subject = "IMAGE";
        mail.Body = "woot";
        mail.Attachments.Add(new Attachment(Application.persistentDataPath + "/images/" + filename + ".png"));
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("unityemailcommunication@gmail.com", "passhere") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");
    }
}
