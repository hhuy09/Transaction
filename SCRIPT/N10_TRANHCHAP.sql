﻿--TÌNH HUỐNG 1: DIRTY READ			- HIẾU
--T1: CHỨC NĂNG THÊM HỢP ĐỒNG THUÊ (NHÂN VIÊN)
CREATE PROCEDURE USP_INSERT_CONTRACTRENT @ID CHAR(5), @CUSID CHAR(5), @HOMEID CHAR(5), @DATE DATE, @SIGN DATE, @END DATE
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	IF NOT EXISTS(SELECT * FROM DBO.VIEWINFO WHERE VCUSTOMERID = @CUSID AND VHOMEID = @HOMEID)
	BEGIN
		PRINT N'KHÔNG CÓ THÔNG TIN XEM NHÀ'
		ROLLBACK TRAN
		RETURN
	END

	INSERT INTO DBO.CONTRACT 
	VALUES (@ID, @CUSID, @HOMEID, @DATE, @SIGN)
	INSERT INTO DBO.CONTRACTRENT
	VALUES (@ID, @END)

	WAITFOR DELAY '00:00:05'

	DECLARE @TYPE NVARCHAR(50)
	SELECT @TYPE = TYPENAME FROM DBO.TYPE, DBO.HOME WHERE HOMEID = @HOMEID AND HOMETYPEID = TYPEID

	IF ((@@ERROR <> 0) OR (@SIGN < @DATE) OR (@END < @SIGN)
	OR @TYPE = N'Nhà bán')
	BEGIN
		PRINT N'THÊM HỢP ĐỒNG THUÊ THẤT BẠI'
		ROLLBACK TRAN
		RETURN
	END
	PRINT N'THÊM HỢP ĐỒNG THUÊ THÀNH CÔNG'

	UPDATE DBO.HOME
	SET HOMESTATUSID = (SELECT TYPEID FROM DBO.TYPE WHERE TYPENAME = N'Đã thuê')
	WHERE HOMEID = @HOMEID
	COMMIT TRAN 
END
GO



EXEC USP_INSERT_CONTRACTRENT 'X0004', 'C0005', 'H0005', '2020-12-05', '2020-12-01', '2021-12-25'
GO
SELECT * FROM DBO.CONTRACT
GO


--T2:CHỨC NĂNG XEM DANH SÁCH HỢP ĐỒNG (QUẢN LÝ)
CREATE PROCEDURE USP_SELECT_CONTRACT
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ UNCOMMITTED
	BEGIN TRAN
	SELECT * FROM DBO.CONTRACT
	COMMIT TRAN
END
GO

EXEC USP_SELECT_CONTRACT
GO


--EXEC USP_SELECT_VIEWINFO 'C0003'
--GO

--TÌNH HUỐNG 2: LOST UPDATE			- HIẾU
--T1 & T2: CHỨC NĂNG TĂNG GIÁ THUÊ NHÀ
CREATE PROCEDURE USP_INCREASE_RENT @ID CHAR(5), @ADD MONEY
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN

	IF NOT EXISTS(SELECT * FROM DBO.HOMERENT WHERE HOMERENTID = @ID)
	BEGIN
		PRINT N'NHÀ KHÔNG TỒN TẠI'
		ROLLBACK TRAN
		RETURN
	END

	DECLARE @RENT MONEY 
	SELECT @RENT = RENT FROM DBO.HOMERENT WHERE HOMERENTID = @ID

	WAITFOR DELAY '00:00:05'

	SET @RENT = @RENT + @ADD
	UPDATE DBO.HOMERENT
	SET RENT = @RENT
	WHERE HOMERENTID = @ID
	PRINT N'CẬP NHẬT THÀNH CÔNG'

	COMMIT TRAN
END
GO

SELECT * FROM DBO.HOMERENT
GO
EXEC USP_INCREASE_RENT 'H0019', 500000
GO
SELECT * FROM DBO.HOMERENT
GO

--TÌNH HUỐNG 3: DIRTY READ			- Q HUY
--T1: CHỨC NĂNG THÊM LỊCH XEM (NHÂN VIÊN)
CREATE PROCEDURE USP_INSERT_VIEWINFO @CUSID CHAR(5), @HOMEID CHAR(5), @DATE DATETIME, @CMT NVARCHAR(100)
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	IF NOT EXISTS(SELECT * FROM DBO.CUSTOMER WHERE CUSTOMERID = @CUSID)
	OR NOT EXISTS(SELECT * FROM DBO.HOME WHERE HOMEID = @HOMEID)
	BEGIN
		PRINT N'THÔNG TIN KHÔNG HỢP LỆ'
		ROLLBACK TRAN
		RETURN
	END
	DECLARE @NUM INT = (SELECT HVIEW FROM DBO.HOME WHERE HOMEID = @HOMEID)
	INSERT INTO DBO.VIEWINFO ( VCUSTOMERID, VHOMEID, VIEWDATE, CUSTOMERCOMMENT)
	VALUES (@CUSID, @HOMEID, @DATE, @CMT)

	WAITFOR DELAY '00:00:05'

	IF ((@@ERROR <> 0) OR (NOT EXISTS (SELECT * FROM DBO.CUSTOMER C, DBO.REQUEST R, DBO.HOME H, DBO.VIEWINFO V 
	WHERE V.VCUSTOMERID = @CUSID AND V.VHOMEID = @HOMEID AND V.VIEWDATE = @DATE AND V.VCUSTOMERID = C.CUSTOMERID 
	AND C.CUSTOMERID = R.RCUSTOMERID AND V.VHOMEID = H.HOMEID AND H.HOMETYPEID = R.RHOMETYPEID)))
	BEGIN
		PRINT N'THÊM LỊCH XEM THẤT BẠI'
		ROLLBACK TRAN
		RETURN
	END
	PRINT N'THÊM LỊCH XEM THÀNH CÔNG'
	
	WAITFOR DELAY '00:00:05'

	SET @NUM = @NUM + 1;
	UPDATE DBO.HOME
	SET HVIEW = @NUM
	WHERE HOMEID = @HOMEID

	COMMIT TRAN 
END
GO

EXEC USP_INSERT_VIEWINFO 'C0003', 'H0002', '2020-11-28', N' '
GO
SELECT * FROM DBO.VIEWINFO
GO

--T2:CHỨC NĂNG XEM LỊCH XEM NHÀ (KHÁCH HÀNG)
CREATE PROCEDURE USP_SELECT_VIEWINFO @CUSID CHAR(5)
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ UNCOMMITTED
	BEGIN TRAN
	IF NOT EXISTS(SELECT * FROM DBO.CUSTOMER WHERE CUSTOMERID = @CUSID)
	BEGIN
		PRINT N'THÔNG TIN KHÔNG HỢP LỆ'
		ROLLBACK TRAN
		RETURN
	END
	
	SELECT * FROM DBO.VIEWINFO WHERE VCUSTOMERID = @CUSID
	COMMIT TRAN
END
GO

--EXEC USP_SELECT_VIEWINFO 'C0003'
--GO

--TÌNH HUỐNG 4: UNREPEATABLE READ	- Q HUY
--T1: CHỨC NĂNG TÌM NHÂN VIÊN THEO KHU VỰC (QUẢN LÝ)
CREATE PROCEDURE USP_SELECT_EMP_AREA @AREA NVARCHAR(50), @NUMRESULT INT OUT
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	SELECT @NUMRESULT = COUNT(*) FROM DBO.EMPLOYEE WHERE EMPLOYEEADDRESS LIKE @AREA + '%'
	IF(@NUMRESULT = 0)
	BEGIN
		PRINT N'KHÔNG TÌM THẤY KẾT QUẢ NÀO'
		ROLLBACK TRAN
		RETURN
	END
	ELSE
	BEGIN
		PRINT N'SỐ KẾT QUẢ TÌM THẤY: ' + CAST(@NUMRESULT AS CHAR(10))
	END

	WAITFOR DELAY '00:00:05'
	
	SELECT * FROM DBO.EMPLOYEE WHERE EMPLOYEEADDRESS LIKE @AREA + '%'
	COMMIT TRAN
END
GO

EXEC USP_SELECT_EMP_AREA N'Thủ Đức', 0
GO

--T2: CẬP NHẬT THÔNG TIN CÁ NHÂN
CREATE PROCEDURE USP_UPDATE_EMP_AREA @EMPID CHAR(5), @AREA NVARCHAR(50)
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN

	IF NOT EXISTS(SELECT * FROM DBO.EMPLOYEE WHERE EMPLOYEEID = @EMPID)
	BEGIN
		PRINT N'NHÂN VIÊN KHÔNG HỢP LỆ'
		ROLLBACK TRAN
	END

	UPDATE DBO.EMPLOYEE
	SET EMPLOYEEADDRESS = @AREA
	WHERE EMPLOYEEID = @EMPID

	COMMIT TRAN
END
GO

--SELECT * FROM DBO.EMPLOYEE
--GO
--EXEC USP_UPDATE_EMP 'E0001', N'Quận 2'
--GO
--SELECT * FROM DBO.EMPLOYEE
--GO

--TÌNH HUỐNG 5: LOST UPDATE			- H HUY
--T1 & T2: CHỨC NĂNG THÊM LỊCH XEM (NHÂN VIÊN, QUẢN LÝ)
	--SELECT HOMEID, HVIEW FROM DBO.HOME
	--EXEC USP_INSERT_VIEWINFO 'C0009', 'H0012', '2020-12-31', 'Ok'
	--SELECT HOMEID, HVIEW FROM DBO.HOME

	--SELECT HOMEID, HVIEW FROM DBO.HOME
	--EXEC USP_INSERT_VIEWINFO 'C0013', 'H0012', '2020-12-07', ''
	--SELECT HOMEID, HVIEW FROM DBO.HOME

--TÌNH HUỐNG 6: CONVERSION DEADLOCK	- H HUY
--T1 & T2 CHỨC NĂNG THÊM LỊCH XEM (NHÂN VIÊN, QUẢN LÝ)
CREATE PROCEDURE USP_CONVER_INSERT_VIEWINFO @CUSID CHAR(5), @HOMEID CHAR(5), @DATE DATETIME, @CMT NVARCHAR(100)
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL REPEATABLE READ 
	BEGIN TRAN
	IF NOT EXISTS(SELECT * FROM DBO.CUSTOMER WHERE CUSTOMERID = @CUSID)
	OR NOT EXISTS(SELECT * FROM DBO.HOME WHERE HOMEID = @HOMEID)
	BEGIN
		PRINT N'THÔNG TIN KHÔNG HỢP LỆ'
		ROLLBACK TRAN
		RETURN
	END
	DECLARE @NUM INT = (SELECT HVIEW FROM DBO.HOME WHERE HOMEID = @HOMEID)
	INSERT INTO DBO.VIEWINFO ( VCUSTOMERID, VHOMEID, VIEWDATE, CUSTOMERCOMMENT)
	VALUES (@CUSID, @HOMEID, @DATE, @CMT)

	WAITFOR DELAY '00:00:05'

	IF ((@@ERROR <> 0) OR (NOT EXISTS (SELECT * FROM DBO.CUSTOMER C, DBO.REQUEST R, DBO.HOME H, DBO.VIEWINFO V 
	WHERE V.VCUSTOMERID = @CUSID AND V.VHOMEID = @HOMEID AND V.VIEWDATE = @DATE AND V.VCUSTOMERID = C.CUSTOMERID 
	AND C.CUSTOMERID = R.RCUSTOMERID AND V.VHOMEID = H.HOMEID AND H.HOMETYPEID = R.RHOMETYPEID)))
	BEGIN
		PRINT N'THÊM LỊCH XEM THẤT BẠI'
		ROLLBACK TRAN
		RETURN
	END
	PRINT N'THÊM LỊCH XEM THÀNH CÔNG'
	
	WAITFOR DELAY '00:00:05'

	SET @NUM = @NUM + 1;
	UPDATE DBO.HOME
	SET HVIEW = @NUM
	WHERE HOMEID = @HOMEID

	COMMIT TRAN 
END
GO

--SELECT HOMEID, HVIEW FROM DBO.HOME
--EXEC USP_CONVER_INSERT_VIEWINFO 'C0009', 'H0012', '2020-12-07', ''
--SELECT HOMEID, HVIEW FROM DBO.HOME

--SELECT HOMEID, HVIEW FROM DBO.HOME
--EXEC USP_CONVER_INSERT_VIEWINFO 'C0013', 'H0012', '2020-12-07', ''
--SELECT HOMEID, HVIEW FROM DBO.HOME

--TÌNH HUỐNG 7: PHANTOM				- H HUY
--T1: CHỨC NĂNG THỐNG KÊ SỐ HỢP ĐỒNG MỖI NHÂN VIÊN
CREATE PROCEDURE USP_COUNT_CONTRACT @TOTAL INT OUT
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	SELECT @TOTAL = COUNT(*) FROM DBO.CONTRACT
	PRINT N'TỔNG SỐ HỢP ĐỒNG: ' + CAST(@TOTAL AS CHAR(10))

	WAITFOR DELAY '00:00:05'

	SELECT E.EMPLOYEEID, E.EMPLOYEENAME, COUNT(E.EMPLOYEEID) 'AMOUNT CONTRACT'
	FROM DBO.EMPLOYEE E, DBO.CONTRACT C, DBO.HOME H
	WHERE (C.CHOMEID = H.HOMEID AND H.HEMPLOYEEID = E.EMPLOYEEID)
	GROUP BY E.EMPLOYEEID, E.EMPLOYEENAME
	UNION 
	SELECT EMPLOYEEID, EMPLOYEENAME, 0 'AMOUNT CONTRACT'
	FROM DBO.EMPLOYEE
	WHERE EMPLOYEEID NOT IN (SELECT E.EMPLOYEEID
	FROM DBO.EMPLOYEE E, DBO.CONTRACT C, DBO.HOME H
	WHERE (C.CHOMEID = H.HOMEID AND H.HEMPLOYEEID = E.EMPLOYEEID))

	COMMIT TRAN
END
GO

EXEC USP_COUNT_CONTRACT 0
GO

--T2: CHỨC NĂNG THÊM HỢP ĐỒNG BÁN (NHÂN VIÊN)
CREATE PROCEDURE USP_INSERT_CONTRACTSELL @ID CHAR(5), @CUSID CHAR(5),  @HOMEID CHAR(5), @DATE DATE, @SIGN DATE, @DEPO MONEY
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	IF NOT EXISTS(SELECT * FROM DBO.VIEWINFO WHERE VCUSTOMERID = @CUSID AND VHOMEID = @HOMEID)
	BEGIN
		PRINT N'KHÔNG CÓ THÔNG TIN XEM NHÀ'
		ROLLBACK TRAN
		RETURN
	END

	INSERT INTO DBO.CONTRACT 
	VALUES (@ID, @CUSID, @HOMEID, @DATE, @SIGN)
	INSERT INTO DBO.CONTRACTSELL
	VALUES (@ID, @DEPO)

	DECLARE @TYPE NVARCHAR(50)
	SELECT @TYPE = TYPENAME FROM DBO.TYPE, DBO.HOME WHERE HOMEID = @HOMEID AND HOMETYPEID = TYPEID

	IF ((@@ERROR <> 0) OR (@SIGN < @DATE) OR (@TYPE = N'Nhà thuê'))
	BEGIN
		PRINT N'THÊM HỢP ĐỒNG BÁN THẤT BẠI'
		ROLLBACK TRAN
		RETURN
	END
	PRINT N'THÊM HỢP ĐỒNG BÁN THÀNH CÔNG'

	UPDATE DBO.HOME
	SET HOMESTATUSID = (SELECT STATUSID FROM DBO.STATUS WHERE STATUSNAME = N'Đã bán')
	WHERE HOMEID = @HOMEID

	COMMIT TRAN 
END
GO


--TÌNH HUỐNG 8: UNREPEATABLE READ	- H HUY
--T1: CHỨC NĂNG TÌM NHÀ BÁN/THUÊ THEO KHU VỰC (KHÁCH HÀNG)
CREATE PROCEDURE USP_SELECT_HOME_AREA @AREA NVARCHAR(50), @TYPE NVARCHAR(50), @NUMRESULT INT OUT
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	DECLARE @TYPEID CHAR(5)
	SELECT @TYPEID = TYPEID FROM DBO.TYPE WHERE @TYPE = TYPENAME

	SELECT @NUMRESULT = COUNT(*) FROM DBO.HOME WHERE HADDRESSDISTRICT = @AREA  AND HOMETYPEID = @TYPEID
	IF(@NUMRESULT = 0)
	BEGIN
		PRINT N'KHÔNG TÌM THẤY KẾT QUẢ NÀO'
		ROLLBACK TRAN
		RETURN
	END
	ELSE
	BEGIN
		PRINT N'SỐ KẾT QUẢ TÌM THẤY: ' + CAST(@NUMRESULT AS CHAR(10))
	END

	WAITFOR DELAY '00:00:05'
	
	SELECT * FROM DBO.HOME WHERE HADDRESSDISTRICT = @AREA AND HOMETYPEID = @TYPEID
	COMMIT TRAN
END
GO

--EXEC USP_SELECT_HOME_AREA N'Quận 5', N'Nhà bán', 0
--GO


--T2: CHỨC NĂNG XÓA NHÀ (CHỦ NHÀ)
CREATE PROCEDURE USP_DELETE_HOME @ID CHAR(5)
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	IF NOT EXISTS (SELECT * FROM DBO.HOME WHERE HOMEID = @ID)
	BEGIN
		PRINT N'NHÀ KHÔNG TỒN TẠI'
		ROLLBACK TRAN
		RETURN
	END

	IF EXISTS (SELECT * FROM DBO.HOMERENT WHERE HOMERENTID = @ID)
	BEGIN
		DELETE DBO.HOMERENT WHERE HOMERENTID = @ID
	END

	IF EXISTS (SELECT * FROM DBO.HOMESELL WHERE HOMESELLID = @ID)
	BEGIN
		DELETE DBO.HOMESELL WHERE HOMESELLID = @ID
	END
	
	DELETE DBO.HOME WHERE HOMEID = @ID

	COMMIT TRAN
END
GO


--TÌNH HUỐNG 9:	CYCLE_DEADLOCK		- H HUY
--T1: CHỨC NĂNG THÊM LỊCH XEM NHÀ
--T2: CHỨC NĂNG XÓA LỊCH XEM
CREATE PROCEDURE USP_DELETE_VIEWINFO @HOME CHAR(5), @CUS CHAR(5), @DATE DATE
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	IF NOT EXISTS(SELECT * FROM DBO.VIEWINFO WHERE VHOMEID = @HOME AND VCUSTOMERID = @CUS AND VIEWDATE = @DATE)
	BEGIN
		PRINT N'KHÔNG CÓ THÔNG TIN XEM'
		ROLLBACK TRAN
		RETURN
	END

	UPDATE DBO.HOME
	SET HVIEW = HVIEW - 1
	WHERE HOMEID = @HOME

	DELETE DBO.VIEWINFO
	WHERE VHOMEID = @HOME AND VCUSTOMERID = @CUS AND VIEWDATE = @DATE
	
END
GO


--TÌNH HUỐNG 10:DIRTY READ			- HUYỀN
--T1: CHỨC NĂNG THÊM NHÀ THUÊ (NHÂN VIÊN)
CREATE PROCEDURE USP_INSERT_HOMERENT @HOST CHAR(5), @EMP CHAR(5), @STR NVARCHAR(50), @DIS NVARCHAR(50), @CITY NVARCHAR(50), @ROOM INT, @EDATE DATE, @RENT MONEY
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ UNCOMMITTED
	BEGIN TRAN
	DECLARE @ID CHAR(5)
	EXEC @ID = AUTO_IDHOME

	IF EXISTS (SELECT * FROM DBO.HOME WHERE HOMEID = @ID)
	BEGIN
		PRINT N'MÃ NHÀ KHÔNG HỢP LỆ'
		ROLLBACK TRAN
		RETURN
	END

	DECLARE @STTID CHAR(5)
	SELECT @STTID = STATUSID FROM DBO.STATUS WHERE STATUSNAME = N'Còn trống'
	DECLARE @TYPEID CHAR(5)
	SELECT @TYPEID = TYPEID FROM DBO.TYPE WHERE TYPENAME = N'Nhà thuê'
	
	INSERT INTO DBO.HOME
	VALUES (@ID, @HOST, @EMP, @STR, @DIS, @CITY, @STTID, @TYPEID, @ROOM, GETDATE(), @EDATE, 0)
	INSERT INTO DBO.HOMERENT
	VALUES (@ID, @RENT)

	WAITFOR DELAY '00:00:05'
	
	IF ((@@ERROR <> 0) OR (EXISTS (SELECT * FROM DBO.HOME WHERE EXPIRATIONDATE < POSTDATE)))
	BEGIN
		PRINT N'THÊM NHÀ THUÊ THẤT BẠI'
		ROLLBACK TRAN
		RETURN
	END
	
	PRINT N'THÊM NHÀ THUÊ THÀNH CÔNG'
	COMMIT TRAN
END
GO

--EXEC USP_INSERT_HOMERENT 'H0101', 'F0010', 'E0001',  '', '', '', 2,'2020-10-10', 1000000
--GO
--EXEC USP_INSERT_HOMERENT 'H0010', 'F0010', 'E0008', '536 Trường Chinh', 'Quận 1', 'Tp Hồ Chí Minh', 2,'2021-10-10', 1000000
--GO
--SELECT * FROM HOME
--GO

--T2: CHỨC NĂNG XEM DANH SÁCH NHÀ (KHÁCH HÀNG)
CREATE PROCEDURE USP_SELECT_HOME @TYPE NVARCHAR(50)
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ UNCOMMITTED
	BEGIN TRAN

	DECLARE @STT CHAR(5)
	SELECT @STT = STATUSID FROM DBO.STATUS WHERE STATUSNAME = N'Còn trống'
	DECLARE @TYPEID CHAR(5)
	SELECT @TYPEID = TYPEID FROM DBO.TYPE WHERE TYPENAME = @TYPE

	SELECT HOMEID, HADDRESSDISTRICT, ROOMNUMBER, POSTDATE, HVIEW, RENT FROM DBO.HOME, DBO.HOMERENT
	WHERE HOMEID = HOMERENTID AND HOMETYPEID = @TYPEID AND HOMESTATUSID = @STT

	COMMIT TRAN
END
GO
--EXEC USP_SELECT_HOME 'Nhà thuê'
--GO

--TÌNH HUỐNG 11:PHANTOM				- HUYỀN
--T1:TÌM NHÀ THEO KHU VỰC (KHÁCH HÀNG)
--EXEC USP_SELECT_HOME_AREA N'Thủ Đức', N'Nhà thuê'
--GO
--T2:THÊM NHÀ BÁN (NHÂN VIÊN)
CREATE PROCEDURE USP_INSERT_HOMESELL @HOST CHAR(5), @EMP CHAR(5), @STR NVARCHAR(50), @DIS NVARCHAR(50), @CITY NVARCHAR(50), @ROOM INT, @EDATE DATE, @SELL MONEY, @RQH NVARCHAR(100)
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL READ COMMITTED
	BEGIN TRAN
	DECLARE @ID CHAR(5)
	EXEC @ID = AUTO_IDHOME

	IF EXISTS (SELECT * FROM DBO.HOME WHERE HOMEID = @ID)
	BEGIN
		PRINT N'MÃ NHÀ KHÔNG HỢP LỆ'
		ROLLBACK TRAN
		RETURN
	END

	DECLARE @STTID CHAR(5)
	SELECT @STTID = STATUSID FROM DBO.STATUS WHERE STATUSNAME = N'Còn trống'
	DECLARE @TYPEID CHAR(5)
	SELECT @TYPEID = TYPEID FROM DBO.TYPE WHERE TYPENAME = N'Nhà bán'

	INSERT INTO DBO.HOME
	VALUES (@ID, @HOST, @EMP, @STR, @DIS, @CITY, @STTID, @TYPEID, @ROOM, GETDATE(), @EDATE, 0)
	INSERT INTO DBO.HOMESELL
	VALUES (@ID, @SELL, @RQH)

	COMMIT TRAN
END
GO

--TÌNH HUỐNG 12:DEADLOCK_CONVER		- KHOA
--T1 & T2: CHỨC NĂNG TĂNG GIÁ THUÊ NHÀ
CREATE PROCEDURE USP_CONVER_INCREASE_RENT @ID CHAR(5), @ADD MONEY
AS
BEGIN
	SET TRANSACTION ISOLATION
	LEVEL REPEATABLE READ
	BEGIN TRAN

	IF NOT EXISTS(SELECT * FROM DBO.HOMERENT WHERE HOMERENTID = @ID)
	BEGIN
		PRINT N'NHÀ KHÔNG TỒN TẠI'
		ROLLBACK TRAN
		RETURN
	END

	DECLARE @RENT MONEY 
	SELECT @RENT = RENT FROM DBO.HOMERENT WHERE HOMERENTID = @ID

	WAITFOR DELAY '00:00:05'

	SET @RENT = @RENT + @ADD
	UPDATE DBO.HOMERENT
	SET RENT = @RENT
	WHERE HOMERENTID = @ID
	PRINT N'CẬP NHẬT THÀNH CÔNG'

	COMMIT TRAN
END
GO

--SELECT * FROM DBO.HOMERENT
--GO
--EXEC USP_CONVER_INCREASE_RENT 'H0002', 10000
--GO
--SELECT * FROM DBO.HOMERENT
--GO
