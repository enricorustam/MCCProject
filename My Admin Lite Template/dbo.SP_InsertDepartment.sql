CREATE PROCEDURE SP_InsertDepartment
	@Name varchar(50) 
AS
	insert into TB_M_Department(Name) values(@Name);
return 0