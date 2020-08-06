CREATE PROCEDURE SP_DeleteAdmin
	@Id int 
AS
	Delete from TB_M_Admin Where Id=@Id;
return 0