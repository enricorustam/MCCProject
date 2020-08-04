CREATE PROCEDURE SP_EditDepartment
	@Id int, @name varchar(50)
AS
	Update TB_M_Department
	set Name = @name
	where Id = @Id
RETURN 0