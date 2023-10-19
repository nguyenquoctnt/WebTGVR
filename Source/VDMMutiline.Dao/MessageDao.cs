using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;

namespace VDMMutiline.Dao
{
    public class MessageDao : BaseDao
    {
        #region Action
        public int Insert(Message item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.Messages.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Message item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.Messages.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.IdNguoigui = item.IdNguoigui;
                    dbItem.TenNguoigui = item.TenNguoigui;
                    dbItem.RoomId = item.RoomId;
                    dbItem.ContentChat = item.ContentChat;
                    dbItem.NgayGui = item.NgayGui;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Message item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.Messages.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Messages.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(Message item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.Messages.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Daxoa = true;

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public Message GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.Messages
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public void GetUnRead(MessageParam param)
        {
            var filter = param.MessageFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.Messages
                            join u in dbContext.AspNetUsers on n.IdNguoigui equals u.Id
                            where (filter.Id.HasValue == false || !n.MessageReceiveds.Any(k => k.IdReceiver == filter.Id))
                            && n.Daxoa == Constant.IsNotDeleted 
                            select new MessageInfo
                            {
                                Id = n.Id,
                                IdNguoigui = n.IdNguoigui,
                                TenNguoigui = n.TenNguoigui,
                                RoomId = n.RoomId,
                                ContentChat = n.ContentChat,
                                Daxoa = n.Daxoa,
                                NgayGui = n.NgayGui,
                                Sender = new AspNetUserInfo()
                                {
                                    DisplayName = u.DisplayName,
                                    UserName = u.UserName,
                                    Avatar = u.Avatar
                                }
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MessageInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MessageInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }

        public MessageInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.Messages
                            where n.Id == id
                            select new MessageInfo()
                            {
                                Id = n.Id,
                                IdNguoigui = n.IdNguoigui,
                                TenNguoigui = n.TenNguoigui,
                                RoomId = n.RoomId,
                                ContentChat = n.ContentChat,
                                Daxoa = n.Daxoa,
                                NgayGui = n.NgayGui,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(MessageParam param)
        {
            var filter = param.MessageFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.Messages
                            join u in dbContext.AspNetUsers on n.IdNguoigui equals u.Id
                            where (filter.Id.HasValue == false || n.RoomId == filter.Id)
                            // && (filter.ItemId.HasValue == false || n.MessageReceiveds.)
                            && n.Daxoa == Constant.IsNotDeleted
                            select new MessageInfo
                            {
                                Id = n.Id,
                                IdNguoigui = n.IdNguoigui,
                                TenNguoigui = n.TenNguoigui,
                                RoomId = n.RoomId,
                                ContentChat = n.ContentChat,
                                Daxoa = n.Daxoa,
                                NgayGui = n.NgayGui,
                                Sender = new AspNetUserInfo()
                                {
                                    DisplayName = u.DisplayName,
                                    UserName = u.UserName,
                                    Avatar = u.Avatar
                                },
                                
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MessageInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MessageInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }
        public void GetAll(MessageParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.Messages
                            where n.Daxoa == Constant.IsNotDeleted
                            select new MessageInfo
                            {
                                Id = n.Id,
                                IdNguoigui = n.IdNguoigui,
                                TenNguoigui = n.TenNguoigui,
                                RoomId = n.RoomId,
                                ContentChat = n.ContentChat,
                                Daxoa = n.Daxoa,
                                NgayGui = n.NgayGui,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MessageInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MessageInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }


        #endregion
    }
}
