using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using System;
using System.Linq;

namespace VDMMutiline.Dao
{
    public class NotifyDao : BaseDao
    {
        #region Action
        public int Insert(Notification item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.Notifications.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.NotificationID;
            }
        }
        public void Update(Notification item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.Notifications.FirstOrDefault(sitem => sitem.NotificationID == item.NotificationID);
                if (dbItem != null)
                {

                    dbItem.ReadStatus = item.ReadStatus;
                    dbItem.ReceivedDate = item.ReceivedDate;
                    dbItem.ReceiverID = item.ReceiverID;
                    dbItem.SenderID = item.SenderID;
                    dbItem.SenderName = item.SenderName;
                    dbItem.SentDate = item.SentDate;
                    dbItem.Subject = item.Subject;
                    dbItem.Type = item.Type;
                    dbItem.ReadStatus = item.ReadStatus;
                    dbItem.Link = item.Link;
                    dbItem.Body = item.Body;
                    dbItem.IsSendAll = item.IsSendAll;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Notification item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.Notifications.FirstOrDefault(en => en.NotificationID == item.NotificationID);

                if (dbItem != null)
                {
                    dbContext.Notifications.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public Notification GetbyId(int? id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.Notifications
                            where n.NotificationID == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public NotificationInfo GetInfo(int? id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.Notifications
                            where item.NotificationID == id
                            select new NotificationInfo()
                            {
                                NotificationID = item.NotificationID,
                                ReadStatus = item.ReadStatus,
                                ReceivedDate = item.ReceivedDate,
                                ReceiverID = item.ReceiverID,
                                SenderID = item.SenderID,
                                SenderName = item.SenderName,
                                SentDate = item.SentDate,
                                Subject = item.Subject,
                                Type = item.Type,
                                Link = item.Link,
                                Body = item.Body,
                                IsSendAll = item.IsSendAll
                            };
                return query.FirstOrDefault();
            }
        }

        public void GetAllIndex(NotifyParam param)
        {
            throw new NotImplementedException();
        }
        
            public void GetListByReceiver(NotifyParam param)
        {
            var filter = param.NotifyFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.Notifications
                            where (filter.ItemId.HasValue == false || item.NotificationID == filter.ItemId)
                            && item.Daxoa == Constant.IsNotDeleted
                            && (string.IsNullOrEmpty(filter.Search) || item.SenderName.Contains(filter.Search))
                            && (!filter.UserId.HasValue || item.ReceiverID == filter.UserId)
                            select new NotificationInfo
                            {
                                NotificationID = item.NotificationID,
                                ReadStatus = item.ReadStatus,
                                ReceivedDate = item.ReceivedDate,
                                ReceiverID = item.ReceiverID,
                                SenderID = item.SenderID,
                                SenderName = item.SenderName,
                                SentDate = item.SentDate,
                                Subject = item.Subject,
                                Type = item.Type,
                                Link = item.Link,
                                Body = item.Body,
                                IsSendAll = item.IsSendAll

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.Notifications = query.OrderByDescending(z => z.SentDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.Notifications = query.OrderByDescending(z => z.SentDate).ToList();

                }
            }
        }
        public void Search(NotifyParam param)
        {
            var filter = param.NotifyFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.Notifications
                            where (filter.ItemId.HasValue == false || item.NotificationID == filter.ItemId)
                            && item.Daxoa == Constant.IsNotDeleted
                            && (string.IsNullOrEmpty(filter.Search) || item.SenderName.Contains(filter.Search))
                            select new NotificationInfo
                            {
                                NotificationID = item.NotificationID,
                                ReadStatus = item.ReadStatus,
                                ReceivedDate = item.ReceivedDate,
                                ReceiverID = item.ReceiverID,
                                SenderID = item.SenderID,
                                SenderName = item.SenderName,
                                SentDate = item.SentDate,
                                Subject = item.Subject,
                                Type = item.Type,
                                Link = item.Link,
                                Body = item.Body,
                                IsSendAll = item.IsSendAll

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.Notifications = query.OrderByDescending(z => z.SentDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.Notifications = query.OrderByDescending(z => z.SentDate).ToList();

                }
            }
        }
        public void GetAll(NotifyParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.Notifications
                            where item.Daxoa == Constant.IsNotDeleted
                            select new NotificationInfo
                            {
                                NotificationID = item.NotificationID,
                                ReadStatus = item.ReadStatus,
                                ReceivedDate = item.ReceivedDate,
                                ReceiverID = item.ReceiverID,
                                SenderID = item.SenderID,
                                SenderName = item.SenderName,
                                SentDate = item.SentDate,
                                Subject = item.Subject,
                                Type = item.Type,
                                Link = item.Link,
                                Body = item.Body,
                                IsSendAll = item.IsSendAll

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.Notifications = query.OrderByDescending(z => z.SentDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.Notifications = query.OrderByDescending(z => z.SentDate).ToList();

                }
            }
        }

        public void GetAllUnRead(NotifyParam param)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
