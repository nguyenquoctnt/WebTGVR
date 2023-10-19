using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
using static VDMMutiline.SharedComponent.Constants.Constant;
using System;

namespace VDMMutiline.Dao
{
    public class MessageRoomDao : BaseDao
    {
        #region Action
        public int Insert(MessageRoom item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.MessageRooms.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(MessageRoom item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.MessageRooms.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Tenhom = item.Tenhom;
                    dbItem.Ngaytao = item.Ngaytao;
                    dbItem.TypeRoom = item.TypeRoom;
                    dbItem.Trangthai = item.Trangthai;
                    dbItem.Thanhvien = item.Thanhvien;
                    dbItem.IsDefault = item.IsDefault;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(MessageRoom item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.MessageRooms.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.MessageRooms.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(MessageRoom item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.MessageRooms.FirstOrDefault(en => en.Id == item.Id);
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
        public MessageRoom GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageRooms
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public MessageRoomInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageRooms
                            where n.Id == id
                            select new MessageRoomInfo()
                            {
                                Id = n.Id,
                                Tenhom = n.Tenhom,
                                Ngaytao = n.Ngaytao,
                                Nguoitao = n.Nguoitao,
                                TypeRoom = n.TypeRoom,
                                Trangthai = n.Trangthai,
                                Thanhvien = n.Thanhvien,
                                Daxoa = n.Daxoa,
                                IsDefault = n.IsDefault,

                            };
                return query.FirstOrDefault();
            }
        }
        public MessageRoomInfo GetDefaultRoomInfo()
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageRooms
                            where n.IsDefault == true && n.TypeRoom == (int)RoomType.Publish && n.Daxoa != true
                            select new MessageRoomInfo()
                            {
                                Id = n.Id,
                                Tenhom = n.Tenhom,
                                Ngaytao = n.Ngaytao,
                                Nguoitao = n.Nguoitao,
                                TypeRoom = n.TypeRoom,
                                Trangthai = n.Trangthai,
                                Thanhvien = n.Thanhvien,
                                Daxoa = n.Daxoa,
                                IsDefault = n.IsDefault
                            };
                var equal = query.FirstOrDefault();
                if (equal == null)
                {
                    MessageRoom _defaultroom = new MessageRoom()
                    {
                        Tenhom = "CTCDefault",
                        Ngaytao = DateTime.Now,
                        Nguoitao = "System",
                        TypeRoom = (int)RoomType.Publish,
                        Daxoa = Constant.IsNotDeleted,
                        IsDefault = true
                    };
                    dbContext.MessageRooms.InsertOnSubmit(_defaultroom);
                    dbContext.SubmitChanges();
                    equal = query.FirstOrDefault();
                }

                return equal;
            }
        }
        public void Search(MessageRoomParam param)
        {
            var filter = param.MessageRoomFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageRooms
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && n.Daxoa == Constant.IsNotDeleted
                            select new MessageRoomInfo
                            {
                                Id = n.Id,
                                Tenhom = n.Tenhom,
                                Ngaytao = n.Ngaytao,
                                Nguoitao = n.Nguoitao,
                                TypeRoom = n.TypeRoom,
                                Trangthai = n.Trangthai,
                                Thanhvien = n.Thanhvien,
                                Daxoa = n.Daxoa,
                                IsDefault = n.IsDefault,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MessageRoomInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MessageRoomInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }
        public void GetAll(MessageRoomParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageRooms
                            where n.Daxoa == Constant.IsNotDeleted
                            select new MessageRoomInfo
                            {
                                Id = n.Id,
                                Tenhom = n.Tenhom,
                                Ngaytao = n.Ngaytao,
                                Nguoitao = n.Nguoitao,
                                TypeRoom = n.TypeRoom,
                                Trangthai = n.Trangthai,
                                Thanhvien = n.Thanhvien,
                                Daxoa = n.Daxoa,
                                IsDefault = n.IsDefault,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MessageRoomInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MessageRoomInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }


        #endregion
    }
}
