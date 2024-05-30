using BuildMentor.Database.Entities;
using System.Net.Mail;
using System.Net;
using BuildMentor.Models;

namespace BuildMentor.Services
{
    public class SmtpService
    {
        private readonly IConfiguration _configuration;

        public SmtpService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string subject, string messageHtml)
        {
            string htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Welcome to Build Mentor</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            width: 80%;
            margin: auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
            background-color: #f9f9f9;
        }}
        .header {{
            text-align: center;
        }}
        .content {{
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""https://yourwebsite.com/logo.png"" alt=""Build Mentor Logo"" />
        </div>
        <div class=""content"">
        {messageHtml}
        </div>
    </div>
</body>
</html>
";
            var smtpclient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_configuration["NetworkCredentials:Email"], _configuration["NetworkCredentials:Password"]),
                EnableSsl = true,

            };
            var mail = new MailMessage()
            {
                IsBodyHtml = true,
                From = new MailAddress("smtp2807@gmail.com", "Support"),
                Subject = "Build Mentor",
                Body = htmlContent,
            };
            mail.To.Add($"{subject}");
            await smtpclient.SendMailAsync(mail);
        }
        public async  Task<bool> WelcomeEmail(User user)
        {
            try
            {
                await SendEmailAsync(user.Email, $@"            <h1>Welcome to Build Mentor!</h1>
            <div><p>Dear {user.Name},</p>
            <p>We're excited to have you on board. Thank you for registering at Build Mentor!</p>
            <p>We're here to support you in your journey. If you have any questions, feel free to reach out to us at any time.</p>
            <p>Best regards,</p>
            <p>The Build Mentor Team</p></div>");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ChangeEmailAsync(User user)
        {
            try
            {
                await SendEmailAsync(user.Email, $@"<p>Dear {user.Name},</p>
            <p>your email has been changed at {DateTime.Now:HH:mm:ss dd.MM.yyyy}</p>
            <p>If you did not make this change, please contact us immediately.</p>
            <p>Best regards,</p>
            <p>The Build Mentor Team</p>");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task NotifyAdminAsync(string html, Notification notification)
        {
            var message = $@"<p>Dear {notification.Admin.Name},</p>
            <p>You have a new notification from {notification.UserTool.User.Email}.</p>";
            message += html;
            message += $@"<p>The user tool ID is {notification.UserTool.Id}</p>";
            await SendEmailAsync(notification.Admin.Email, message);
        }

        public async Task NotifyAdminNeedsMaintenanceAsync(Notification notification)
        {
            await NotifyAdminAsync($"<p>{notification.UserTool.Tool.Name} needs maintenance</p>", notification);
        }

        public async Task NotifyAdminUnderMaintenanceAsync(Notification notification)
        {
            await NotifyAdminAsync(@$"<p>{notification.UserTool.Tool.Name} is under maintenance</p>", notification);
        }

        public async Task NotifyAdminMaintenanceCompletedAsync(Notification notification)
        {
            await NotifyAdminAsync($"<p>{notification.UserTool.Tool.Name} maintenance is completed</p>", notification);
        }

        public async Task RequestDeniedAsync(ToolPermissionRequest request, string adminComment)
        {
            await SendEmailAsync(request.User.Email, $@"<p>Dear {request.User.Name},</p>
            <p>Your request for tool {request.Tool.Name} has been denied.</p>
            <p>Reason: {adminComment}</p>
            <p>If you think this was an error,contact <a href=""https:\\mail.google.com"">mori.steamer@gmail.com</a></p>");
        }

        public async Task RequestApprovedAsync(ToolPermissionRequest request, string adminComment)
        {
            await SendEmailAsync(request.User.Email, $@"<p>Dear {request.User.Name},</p>
            <p>Your request for tool {request.Tool.Name} has been approved.</p>
            <p>Admin Message: {adminComment}</p>
            <p>The tool should now appear in your home menu</p>");
        }

        public async Task PermissionRemovedAsync(ToolPermission permission,string adminComment)
        {
            await SendEmailAsync(permission.User.Email, $@"<p>Dear {permission.User.Name},</p>
            <p>Your permission for tool {permission.Tool.Name} has been removed.</p>
            <p>Message by Admin: {adminComment}</p>
            <p>If you think this was an error,contact tech support>");

        }

        public async Task UserRemovedAsync(User user, string adminComment)
        {
            await SendEmailAsync(user.Email, $@"<p>Dear {user.Name},</p>
            <p>Your account has been removed from Build Mentor.</p>
            <p>Message by Admin: {adminComment}</p>
            <p>If you think this was an error,contact tech support>");
        }

        public async Task AdminRequestRejectedAsync(User user, string adminComment)
        {
            await SendEmailAsync(user.Email, $@"<p>Dear {user.Name},</p>
            <p>Your request to become an admin has been rejected.</p>
            <p>Message by Admin: {adminComment}</p>
            <p>If you think this was an error,contact tech support>");
        }

        public async Task AdminRequestApprovedAsync(User user, string adminComment)
        {
            await SendEmailAsync(user.Email, $@"<p>Dear {user.Name},</p>
            <p>Your request to become an admin has been approved.</p>
            <p>Message by Admin: {adminComment}</p>
            <p>You should now have access to the admin panel</p>");
        }
    }
}