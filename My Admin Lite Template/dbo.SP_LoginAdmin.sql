CREATE PROCEDURE SP_LoginAdmin
	@Username VARCHAR(50),
	@Pass VARCHAR(50)
AS
	Select * from TB_M_Admin Where Username= @Username AND Password = @Pass;
return 0