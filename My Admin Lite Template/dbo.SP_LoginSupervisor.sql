CREATE PROCEDURE SP_LoginSupervisor
	@Name VARCHAR(50),
	@Pass VARCHAR(50)
AS
	Select * from TB_M_Supervisor Where Name= @Name AND Password = @Pass;
return 0