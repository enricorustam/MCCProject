CREATE PROCEDURE SP_EditSupervisor
	@Id int, @Name varchar(50), @Pass varchar(50)
AS
	Update TB_M_Supervisor
	set Name = @Name, Password = @Pass
	where Id = @Id
RETURN 0