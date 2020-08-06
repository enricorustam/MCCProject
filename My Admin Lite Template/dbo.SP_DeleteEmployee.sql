CREATE PROCEDURE SP_DeleteEmployee
	@Id int 
AS
	Delete from TB_M_Employee Where Id=@Id;
return 0