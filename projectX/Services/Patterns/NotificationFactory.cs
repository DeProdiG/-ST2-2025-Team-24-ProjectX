// NotificationFactory.cs
using projectX.Services.Patterns;

namespace projectX.Services
{
    public interface INotification
    {
        void Send(string recipient, string message);
    }

    public class EmailNotification : INotification
    {
        public void Send(string recipient, string message)
        {
            LoggerService.Instance.LogInformation($"Email sent to {recipient}: {message}");
            // Actual email sending logic would go here
        }
    }

    public class SMSNotification : INotification
    {
        public void Send(string recipient, string message)
        {
            LoggerService.Instance.LogInformation($"SMS sent to {recipient}: {message}");
            // Actual SMS sending logic would go here
        }
    }

    public class PushNotification : INotification
    {
        public void Send(string recipient, string message)
        {
            LoggerService.Instance.LogInformation($"Push notification sent to {recipient}: {message}");
            // Actual push notification logic would go here
        }
    }

    public enum NotificationType
    {
        Email,
        SMS,
        Push
    }

    public class NotificationFactory
    {
        public INotification CreateNotification(NotificationType type)
        {
            LoggerService.Instance.LogInformation($"Creating {type} notification");

            return type switch
            {
                NotificationType.Email => new EmailNotification(),
                NotificationType.SMS => new SMSNotification(),
                NotificationType.Push => new PushNotification(),
                _ => throw new ArgumentException("Invalid notification type")
            };
        }
    }
}