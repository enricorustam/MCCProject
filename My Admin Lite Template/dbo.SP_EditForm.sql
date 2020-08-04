CREATE PROCEDURE SP_EditForm
	@Id int,
	@Name varchar(50), 
	@StartDate DateTime,
	@EndDate DateTime,
	@Duration int,
	@supervisor int,
	@department int
AS
	Update TB_M_Form
	set
		Name = @Name,
		StartDate = @StartDate,
		EndDate = @EndDate,
		Duration = @Duration,
		supervisorId = @supervisor,
		departmentId = @department
	where Id = @Id
RETURN 0