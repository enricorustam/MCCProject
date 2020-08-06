CREATE PROCEDURE SP_InsertAdmin
	@Username varchar(50),
	@Password varchar(50) 
AS
	insert into TB_M_Admin(Username, Password) values(@Username,@Password);
return 0