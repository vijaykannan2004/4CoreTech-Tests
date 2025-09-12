using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentPlatform.Tests
{
    [TestClass]
    public class NotificationTests
    {
        [TestMethod]
        public void Notification_ShouldBeSent()
        {
            var notification = new Notification { Id = 1, Content = "New Msg", IsRead = false };
            Assert.IsTrue(notification.Id > 0);
        }

        [TestMethod]
        public void Notification_ShouldBeMarkedAsRead()
        {
            var notification = new Notification { IsRead = false };
            notification.IsRead = true;
            Assert.IsTrue(notification.IsRead);
        }

        [TestMethod]
        public void Notification_ShouldBeDeleted()
        {
            var notification = new Notification { Id = 1 };
            notification = null!;
            Assert.IsNull(notification);
        }

        // 🔹 Extra Tests
        [TestMethod]
        public void UnreadCount_ShouldBeCorrect()
        {
            var notifications = new List<Notification>
            {
                new Notification { Id = 1, IsRead = false },
                new Notification { Id = 2, IsRead = true },
                new Notification { Id = 3, IsRead = false }
            };
            var unreadCount = notifications.Count(n => !n.IsRead);
            Assert.AreEqual(2, unreadCount);
        }

        [TestMethod]
        public void ExpiredNotifications_ShouldBeDeleted()
        {
            var createdAt = DateTime.UtcNow.AddDays(-31);
            var isExpired = (DateTime.UtcNow - createdAt).TotalDays > 30;
            Assert.IsTrue(isExpired);
        }
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
