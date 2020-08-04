CREATE PROCEDURE SP_GetIdSupervisor
	@Id int 
AS
	Select * from TB_M_Supervisor Where Id=@Id;
return 0