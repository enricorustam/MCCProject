CREATE PROCEDURE SP_LoginEmployee
	@Name VARCHAR(50),
	@Nip int
AS
	Select * from TB_M_Employee Where Name= @Name AND NIP = @Nip;
return 0