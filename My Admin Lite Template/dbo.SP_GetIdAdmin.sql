CREATE PROCEDURE SP_GetIdAdmin
	@Id int 
AS
	Select * from TB_M_Admin Where Id=@Id;
return 0