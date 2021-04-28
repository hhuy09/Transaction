-- integrity constraint
-- 1. Giới tính nhân viên, chủ nhà, khách hàng chỉ có thể là "Nam" hoặc "Nữ"
create trigger utg_EGender
on dbo.EMPLOYEE
for update, insert
as
begin
	declare @gender nvarchar(5)
	set @gender = (select EMPLOYEEGendER from inserted )
	if(@gender != N'Nam' and @gender != N'Nữ')
	begin
		raiserror(N'Lỗi: Giới tính chỉ có thể là "Nam" hoặc "Nữ"', 16, 1)
		rollback
	end
end
go

create trigger utg_HGender
on dbo.HOST
for update, insert
as
begin
	declare @gender nvarchar(5)
	set @gender = (select HOSTGendER from inserted )
	if(@gender != N'Nam' and @gender != N'Nữ')
	begin
		raiserror(N'Lỗi: Giới tính chỉ có thể là "Nam" hoặc "Nữ"', 16, 1)
		rollback
	end
end
go

create trigger utg_CGender
on dbo.CUSTOMER
for update, insert
as
begin
	declare @gender nvarchar(5)
	set @gender = (select CUSTOMERGendER from inserted )
	if(@gender != N'Nam' and @gender != N'Nữ')
	begin
		raiserror(N'Lỗi: Giới tính chỉ có thể là "Nam" hoặc "Nữ"', 16, 1)
		rollback
	end
end
go

-- 2. Tiền thuê/bán > 0; Tiền trả trước >= 0
create TRIGGER utg_Price
ON dbo.HOMESELL
FOR UPDATE, INSERT
as
begin
	if EXISTS(select * from inserted where PRICE <= 0)
	begin
		raiserror (N'Lỗi: Tiền không hợp lệ', 16, 1)
		rollback    
	end
end
go

create TRIGGER utg_Rent
ON dbo.HOMERENT
FOR UPDATE, INSERT
as
begin
	if EXISTS(select * from inserted where RENT <= 0)
	begin
		raiserror (N'Lỗi: Tiền không hợp lệ', 16, 1)
		rollback  
	end
end
go

create TRIGGER utg_Deposit
ON dbo.CONTRACTSELL
FOR UPDATE, INSERT
as
begin
	if EXISTS(select * from inserted where DEPOSIT < 0)
	begin
		raiserror (N'Lỗi: Tiền không hợp lệ', 16, 1)
		rollback   
	end
end
go

-- 3. Số phòng >= 0
-- RoomNumber tinyint -> 0-255

-- 4. Nhà mới đăng thì ngày đăng là ngày hiện tại, tình trạng còn trống và chưa có lược xem nào
-- Đã cài ràng buộc mặc định trên bảng

-- 5. Nhà thuê phải thuộc loại nhà thuê; Nhà bán phải thuộc loại "Nhà bán"
create trigger utg_HRID
on HOMERENT
for insert, update
as
begin
	if exists(select * from dbo.HOME H, dbo.TYPE T, inserted I where I.HOMERENTID = H.HOMEID and H.HOMETYPEID = T.TYPEID and T.TYPENAME = N'Nhà bán')
	begin
		raiserror (N'Lỗi: Tình trạng nhà không hợp lệ', 16, 1)
		rollback 
	end
end
go

create trigger utg_HSID
on HOMESELL
for insert, update
as
begin
	if exists(select * from dbo.HOME H, dbo.TYPE T, inserted I where I.HOMESELLID = H.HOMEID and H.HOMETYPEID = T.TYPEID and T.TYPENAME = N'Nhà thuê')
	begin
		raiserror (N'Lỗi: Tình trạng nhà không hợp lệ', 16, 1)
		rollback 
	end
end
go

-- 6. Loại Nhà thuê thì chỉ thuộc tình trạng “Đã thuê”, “Còn trống”, “Hết hạn”; Loại Nhà bán thì chỉ thuộc tình trạng “Đã bán”, “Còn trống”, “Hết hạn”
create trigger utg_StatusHR
on HOME
for insert, update
as
begin
	if exists(select * from dbo.STATUS S, dbo.TYPE T, inserted I 
	where  I.HOMESTATUSID = S.STATUSID and I.HOMETYPEID = T.TYPEID and T.TYPENAME = N'Nhà thuê' and S.STATUSNAME = N'Đã bán')
	begin
		raiserror (N'Lỗi: Tình trạng nhà không hợp lệ', 16, 1)
		rollback 
	end
end
go

create trigger utg_StatusHS
on HOME
for insert, update
as
begin
	if exists(select * from dbo.STATUS S, dbo.TYPE T, inserted I 
	where  I.HOMESTATUSID = S.STATUSID and I.HOMETYPEID = T.TYPEID and T.TYPENAME = N'Nhà bán' and S.STATUSNAME = N'Đã thuê')
	begin
		raiserror (N'Lỗi: Tình trạng nhà không hợp lệ', 16, 1)
		rollback 
	end
end
go

-- 7. Nhà "Còn trống" mới cho phép xem
create trigger utg_ViewInfo
on dbo. VIEWINFO
for insert
as 
begin
	declare @HomeStatusID char(5)
	declare @Stt char(5)
	set @Stt = (select STATUSID from STATUS where STATUSNAME = N'Còn trống')  
	set @HomeStatusID = (select H.HOMESTATUSID from HOME H, inserted I where I.VHOMEID = H.HOMEID)
	if @HomeStatusID != @Stt
	begin
		raiserror(N'Lỗi:Nhà không còn trống nữa.Nên không được xem',16,1)
		rollback
	end
end
go

-- 8. Ngày hết hạn phải lớn hơn ngày đăng
create trigger utg_ExpirationDate
on dbo.HOME
for update, insert
as
begin
	if exists(select * from inserted where EXPIRATIONDATE < POSTDATE)
	begin
		raiserror(N'Lỗi: Ngày hết hạn không hợp lệ', 16, 1)
		rollback tran
	end
end
go

-- 9. Ngày xem phải lớn hơn ngày đăng và nhỏ hơn ngày hết hạn; Lượt xem sẽ cập nhật khi có thông tin xem
create trigger utg_ViewDate
on dbo.VIEWINFO
for update, insert
as 
begin
	if exists(select * from HOME H, inserted I where I.VHOMEID = H.HOMEID and (I.VIEWDATE < H.POSTDATE or I.VIEWDATE >= H.EXPIRATIONDATE))
	begin
		raiserror(N'Lỗi: Ngày xem nhà không hợp lệ', 16, 1)
		rollback
	end
end
go

--10. Ngày ký hợp đồng phải lớn hơn ngày xem gần nhất trên một nhà của một khách hàng
create trigger utr_SignDate
on dbo.CONTRACT
for update, insert
as
begin
	declare @Max datetime
	select @Max = max(V.VIEWDATE) from dbo.VIEWINFO V, inserted I where I.CCUSTOMERID = V.VCUSTOMERID and I.CHOMEID = V.VHOMEID
	if exists(select * from inserted where CVIEWDATE != @Max)
	begin
		raiserror(N'Lỗi: Hợp đồng không hợp lệ', 16, 1)
		rollback
	end
	if exists(select * from inserted where SIGNDATE < CVIEWDATE)
	begin
		raiserror(N'Lỗi: Ngày ký hợp đồng không hợp lệ', 16, 1)
		rollback
	end
end
go

--11. Ngày hết hợp đồng thuê phải lớn hơn ngày ký hợp đồng
create trigger trg_endDate
on dbo.CONTRACTRENT
for update, insert
as
begin 
	if exists(select * from dbo.CONTRACT C, inserted I where C.CONTRACTID = I.CONTRACTRENTID and I.endDATE < C.SIGNDATE)
	begin
		raiserror(N'Lỗi: Ngày hết hợp đồng không hợp lệ', 16, 1)
		rollback
	end
end
go

--12. Hợp đồng bán thì loại nhà là "Nhà bán"; Hợp đồng thuê thì loại nhà là "Nhà thuê"
create trigger utg_ContractSell
on dbo.CONTRACTSELL
for update, insert
as
begin
	declare @TypeRent char(5)
	select @TypeRent = TYPEID from dbo.TYPE where TYPENAME = N'Nhà thuê'
	if exists(select * from dbo.CONTRACT C, dbo.HOME H, inserted I 
	where I.CONTRACTSELLID = C.CONTRACTID and C.CHOMEID = H.HOMEID and H.HOMETYPEID = @TypeRent)
	begin
		raiserror(N'Lỗi: Hợp đồng không hợp lệ', 16, 1)
		rollback
	end
end
go

create trigger utg_ContractRent
on dbo.CONTRACTRENT
for update, insert
as
begin
	declare @TypeSell char(5)
	select @TypeSell = TYPEID from dbo.TYPE where TYPENAME = N'Nhà bán'
	if exists(select * from dbo.CONTRACT C, dbo.HOME H, inserted I 
	where I.CONTRACTRENTID = C.CONTRACTID and C.CHOMEID = H.HOMEID and H.HOMETYPEID = @TypeSell)
	begin
		raiserror(N'Lỗi: Hợp đồng không hợp lệ', 16, 1)
		rollback
	end
end
go

--13. Nếu nhà được ký hợp đồng thì phải cập nhật lại tình trạng nhà
create trigger trg_UpdateStatus
on dbo.CONTRACT
for update, insert
as
begin 
	declare @ID char(5)
	declare @Type char(50)
	declare @Stt char(5)
	if exists(select * from inserted)
	begin
		select @ID = CHomeID from inserted
		select @Type = TYPENAME from dbo.HOME, dbo.TYPE where HOMEID = @ID and HOMETYPEID = TYPEID
		if (@Type = N'Nhà thuê')
		begin
			select @Stt = STATUSID from dbo.STATUS where STATUSNAME = N'Đã thuê'
			update HOME 
			set HOMESTATUSID = @Stt 
			where HOMEID = @ID
		end
		if (@Type = N'Nhà bán')
		begin
			select @Stt = STATUSID from dbo.STATUS where STATUSNAME = N'Đã bán'
			update HOME 
			set HOMESTATUSID = @Stt 
			where HOMEID = @ID
		end	
	end
end
go

--14. Khách hàng yêu cầu loại nhà nào thì chỉ xem loại nhà đó
drop trigger utg_RequestType
create trigger utg_RequestType
on dbo.VIEWINFO
for update, insert
as
begin
	if not exists (select * from dbo.CUSTOMER C, dbo.REQUEST R, dbo.HOME H, inserted I 
	where I.VCUSTOMERID = C.CUSTOMERID and C.CUSTOMERID = R.RCUSTOMERID and I.VHOMEID = H.HOMEID and H.HOMETYPEID = R.RHOMETYPEID)
	begin
		raiserror(N'Lỗi: Khách hàng không yêu cầu loại nhà này', 16, 1)
		rollback
	end
	else
		update dbo.HOME
		set HVIEW += 1
		where HOMEID = (select VHOMEID from inserted)
end
go

--15. Nhà được phụ trách bởi nhân viên làm việc trong chi nhánh quản lý nhà đó
create trigger utg_Home
on dbo.HOME
for update, insert
as
begin
	if exists (select * from dbo.EMPLOYEE E, dbo.HOST H, inserted I where I.HEMPLOYEEID = E.EMPLOYEEID and I.HHOSTID = H.HOSTID and H.HBRANCHID != E.EBRANCHID)
	begin
		raiserror(N'Lỗi: Nhân viên không thể phụ trách nhà này', 16, 1)
		rollback
	end
end
go

--16. Nếu ngày hiện tại lớn hơn ngày hết hạn thì cập nhật lại tình trạng là "Hết hạn"
--Tạo Job kiểm tra hàng ngày
create procedure update_expiration
as
	update dbo.HOME
	set HOMESTATUSID = (select STATUSID from dbo.STATUS where STATUSNAME = N'Hết hạn')
	where EXPIRATIONDATE < getdate()
go

--17. Lượt xem sẽ cập nhật khi có thông tin xem
--Đủ 3 điều kiện để insert/update thông tin xem thì sẽ tăng View, đã cài ở 7. 9. 14.