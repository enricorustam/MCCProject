CREATE PROCEDURE SP_GetIdDepartment
	@Id int 
AS
	Select * from TB_M_Department Where Id=@Id;
return 0