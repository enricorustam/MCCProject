CREATE PROCEDURE SP_GetIdEmployee
	@Id int 
AS
	Select * from TB_M_Employee Where Id=@Id;
return 0