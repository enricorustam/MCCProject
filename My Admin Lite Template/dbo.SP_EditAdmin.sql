CREATE PROCEDURE SP_EditAdmin
	@Id int, @Username varchar(50), @Password varchar(50)
AS
	Update TB_M_Admin
	set Username = @Username,
		Password = @Password
	where Id = @Id
RETURN 0