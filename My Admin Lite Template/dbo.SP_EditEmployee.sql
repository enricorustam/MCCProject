CREATE PROCEDURE SP_EditEmployee
	@Id int, @NIP int, @name varchar(50)
AS
	Update TB_M_Employee
	set NIP = @NIP,
		Name = @name
	where Id = @Id
RETURN 0