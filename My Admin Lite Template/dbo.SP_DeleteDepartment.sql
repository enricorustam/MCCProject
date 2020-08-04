CREATE PROCEDURE SP_DeleteDepartment
	@Id int 
AS
	Delete from TB_M_Department Where Id=@Id;
return 0