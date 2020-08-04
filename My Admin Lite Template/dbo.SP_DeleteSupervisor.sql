CREATE PROCEDURE SP_DeleteSupervisor
	@Id int 
AS
	Delete from TB_M_Supervisor Where Id=@Id;
return 0