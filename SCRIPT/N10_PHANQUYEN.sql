--• Tạo 4 login: Employee 'E'|BranchManagement 'B'| Host 'H'| Customer 'C'
sp_addlogin 'LoginEmployee', 'E';  
GO 
sp_addlogin 'LoginBranchManagement', 'B';  
GO 
sp_addlogin 'LoginHost', 'H';  
GO 
sp_addlogin 'LoginCustomer', 'C';  
GO 


--• Tạo user cho 5 login trên csdl QL Thuê bán nhà:
--• UserEmployee
--• UserBranchManagement
--• UserHost
--• UserCustomer
CREATE USER UserEmployee for login LoginEmployee
CREATE USER UserBranchManagement for login LoginBranchManagement
CREATE USER UserHost for login LoginHost
CREATE USER UserCustomer for login LoginCustomer


--• Tạo 3 nhóm quyền:
--• Employee (Insert,delete, update, select) trên bảng Home,update trên bảng nhân viên
--• BranchManagement xem thông tin khách hàng, nhân viên,thông tin do chi nhánh quản lý
--• Host (Insert,Update,select) trên bảng HOME
--• Customer (select) xem thông tin nhà thuê/bán


sp_addrole 'Employee'
sp_addrole 'BranchManagement'
sp_addrole 'Host'
sp_addrole 'Customer'

--CẤP QUYỀN CHO ROLE Employee 
-- Trên các bảng HOMERENT,HOMESELL,CONTRACT, CONTRACTRENT,CONTRACTSELL,HOME
--TYPE
--REQUEST, VIEWINFO,CUSTOMER,HOST,(BRANCH,EMPLOYEE)
GRANT INSERT,UPDATE,SELECT ON HOME TO Employee

GRANT INSERT,UPDATE,SELECT ON CONTRACT TO Employee

GRANT INSERT,UPDATE,SELECT ON CONTRACTRENT TO Employee

GRANT INSERT,UPDATE,SELECT ON CONTRACTSELL TO Employee

GRANT INSERT,UPDATE,SELECT ON HOMESELL TO Employee

GRANT INSERT,UPDATE,SELECT ON HOMERENT TO Employee

GRANT INSERT,UPDATE,SELECT ON TYPE TO Employee

GRANT INSERT,UPDATE,SELECT ON REQUEST TO Employee

GRANT INSERT,UPDATE,SELECT ON VIEWINFO TO Employee

GRANT INSERT,UPDATE,SELECT ON HOST TO Employee

GRANT INSERT,UPDATE,SELECT ON STATUS TO Employee


--CẤP QUYỀN CHO ROLE HOST 
-- Trên các bảng HOMERENT,HOMESELL,CHOME
--TYPE
--REQUEST, VIEWINFO,CUSTOMER,HOST,(BRANCH,EMPLOYEE)

GRANT UPDATE ON HOST TO HOST

GRANT SELECT ON HOMERENT TO HOST

GRANT SELECT ON HOMESELL TO HOST

GRANT SELECT ON HOME TO HOST

GRANT SELECT ON CONTRACT TO HOST

GRANT SELECT ON CONTRACTRENT TO HOST

GRANT SELECT ON CONTRACTSELL TO HOST

GRANT SELECT ON TYPE TO HOST

GRANT SELECT ON REQUEST TO HOST

GRANT SELECT ON VIEWINFO TO HOST

GRANT SELECT ON HOST TO HOST

GRANT SELECT ON STATUS TO HOST

GRANT SELECT ON EMPLOYEE TO HOST

GRANT SELECT ON BRANCH TO HOST

--CẤP QUYỀN CHO ROLE CUSTOMER
GRANT UPDATE ON CUSTOMER to CUSTOMER

GRANT SELECT ON HOMERENT to CUSTOMER

GRANT SELECT ON HOMESELL to CUSTOMER

GRANT SELECT ON HOME to CUSTOMER

GRANT SELECT ON CONTRACT TO CUSTOMER

GRANT SELECT ON CONTRACTRENT TO CUSTOMER

GRANT SELECT ON CONTRACTSELL TO CUSTOMER

GRANT SELECT ON TYPE TO CUSTOMER

GRANT SELECT ON REQUEST TO CUSTOMER

GRANT SELECT ON VIEWINFO TO CUSTOMER

GRANT SELECT ON HOST TO CUSTOMER

GRANT SELECT ON STATUS TO CUSTOMER

GRANT SELECT ON EMPLOYEE TO CUSTOMER

GRANT SELECT ON BRANCH TO CUSTOMER

--CẤP QUYỀN CHO ROLE BRANCHMANAGEMENT
--1. Gán các quyền cơ bản giống với nhân viên
GRANT INSERT,UPDATE,SELECT ON HOME TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON CONTRACT TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON CONTRACTRENT TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON CONTRACTSELL TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON HOMESELL TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON HOMERENT TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON TYPE TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON REQUEST TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON VIEWINFO TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON HOST TO BranchManagement

GRANT INSERT,UPDATE,SELECT ON STATUS TO BranchManagement

-- 2. gán thêm các quyền đối với các procedure đặc thù của quản lý chi nhánh như thống kê



--Gán các user vào roles vừa tạo:
--• UserEmployee: thành viên của Employee
--• UserBranchManagemnet: thành viên của BranchManagement
--• UserHost: thành viên của Host
--• UserCustomer : thành viên của Customer

sp_addRoleMember 'Employee', 'UserEmployee'
sp_addRoleMember 'BranchManagement', 'UserBranchManagemnet'
sp_addRoleMember 'Host', 'UserHost'
sp_addRoleMember 'Customer', 'UserCustomer'

