CREATE PROCEDURE SP_EditSupervisor
	@Id int, @name varchar(50)
AS
	Update TB_M_Supervisor
	set Name = @name
	where Id = @Id
RETURN 0